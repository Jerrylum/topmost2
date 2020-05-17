using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Media;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Windows.Forms;
using Utilities;

namespace TopMost2
{
    public static class API
    {

        public static void Init()
        {
            dele = new WinEventDelegate(WinEventProc);
            IntPtr m_hhook = SetWinEventHook(EVENT_SYSTEM_FOREGROUND, EVENT_SYSTEM_FOREGROUND, IntPtr.Zero, dele, 0, 0, WINEVENT_OUTOFCONTEXT);

            cureentHwnd = GetForegroundWindow();
        }

        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern int GetWindowTextLength(IntPtr hWnd);
        [DllImport("user32.dll")]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
        [DllImport("user32.dll")]
        static extern IntPtr SetWinEventHook(uint eventMin, uint eventMax, IntPtr hmodWinEventProc, WinEventDelegate lpfnWinEventProc, uint idProcess, uint idThread, uint dwFlags);
        [DllImport("user32.dll")]
        static extern int ToAscii(uint uVirtKey, uint uScanCode,
                                    byte[] lpKeyState,
                                    [Out] StringBuilder lpChar,
                                    uint uFlags);

        [DllImport("user32.dll", SetLastError = true)]
        static extern UInt32 GetWindowLong(IntPtr hWnd, IntPtr nIndex);

        static readonly IntPtr GWL_EXSTYLE = new IntPtr(-20);
        static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);

        static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);
        static readonly UInt32 WS_EX_TOPMOST = 0x0008;
        static readonly UInt32 SWP_NOSIZE = 0x0001;
        static readonly UInt32 SWP_NOMOVE = 0x0002;
        static readonly UInt32 SWP_SHOWWINDOW = 0x0040;
        static readonly UInt32 WINEVENT_OUTOFCONTEXT = 0;
        static readonly UInt32 EVENT_SYSTEM_FOREGROUND = 3;
        static readonly byte HighBit = 0x80;

        public static WinEventDelegate dele = null;
        public static ArrayList TopmostWindowsLog = new ArrayList();
        public static IntPtr lastHwnd;
        public static IntPtr cureentHwnd;
        public static GlobalKeyboardHook gkh = new GlobalKeyboardHook();

        private static KeysConverter kc = new KeysConverter();


        public delegate void WinEventDelegate(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);

        public static bool IsAlwaysRunning()
        {
            Process current = Process.GetCurrentProcess();
            string currentName = current.ProcessName;
            int currentId = current.Id;
            foreach (Process clsProcess in Process.GetProcesses())
            {
                if (clsProcess.ProcessName == currentName && clsProcess.Id != currentId)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool IsAdministrator()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        public static string GetWindowTitle(IntPtr handle)
        {
            var length = GetWindowTextLength(handle);
            var title = new StringBuilder(length);
            GetWindowText(handle, title, length);
            return title.ToString();
        }

        public static void WinEventProc(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
        {
            lastHwnd = cureentHwnd;
            cureentHwnd = hwnd;
        }

        public static bool IsTopMost(IntPtr hwnd)
        {
            return (GetWindowLong(hwnd, GWL_EXSTYLE) & WS_EX_TOPMOST) != 0;
        }

        public static void RestAllTopMost()
        {
            foreach (IntPtr hwnd in TopmostWindowsLog)
                SetTopMost(hwnd, false);
        }

        public static void ToggleTopMost(IntPtr hwnd)
        {
            bool now_topmost = IsTopMost(hwnd);

            SetTopMost(hwnd, !now_topmost);
        }

        public static void SetTopMost(IntPtr hwnd, bool topmost)
        {
            Console.WriteLine(hwnd.ToString("X") + " set to " + topmost);

            //if (TopmostWindowsLog.IndexOf(hwnd) != -1)



            if (topmost)
                if (TopmostWindowsLog.IndexOf(hwnd) == -1)
                    TopmostWindowsLog.Add(hwnd);
                else
                    TopmostWindowsLog.Remove(hwnd);

            SetWindowPos(hwnd, topmost ? HWND_TOPMOST : HWND_NOTOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_SHOWWINDOW);

            if (IsTopMost(hwnd) != topmost)
            {
                if (!IsAdministrator())
                {
                    Process cureent = Process.GetCurrentProcess();
                    string exeName = cureent.MainModule.FileName;
                    ProcessStartInfo startInfo = new ProcessStartInfo(exeName)
                    {
                        Verb = "runas",
                        Arguments = String.Format("--autostart -{0} 0x{1}", (topmost ? "S" : "R"), hwnd.ToString("X"))
                    };
                    try
                    {
                        Process.Start(startInfo);
                        Shutdown();
                    } catch
                    {
                        Console.WriteLine("User cancel permission upgrade");
                    }
                } else
                {
                    SystemSounds.Asterisk.Play(); // Failed to set top most
                }
            }
        }

        public static string GetKeyCombinationBreif(HashSet<Keys> keyset)
        {
            string msg = "";
            foreach (Keys k in keyset)
            {
                if (msg != "") msg += " + ";

                msg += GetKeyName(k);
            }
            return msg + "\n"; // important
        }

        public static char ToAscii(Keys key, Keys modifiers)
        {
            var outputBuilder = new StringBuilder(2);
            int result = ToAscii((uint)key, 0, GetKeyState(modifiers),
                                 outputBuilder, 0);
            if (result == 1)
                return outputBuilder[0];
            else
                throw new Exception("Invalid key");
        }

        private static byte[] GetKeyState(Keys modifiers)
        {
            var keyState = new byte[256];
            foreach (Keys key in Enum.GetValues(typeof(Keys)))
            {
                if ((modifiers & key) == key)
                {
                    keyState[(int)key] = HighBit;
                }
            }
            return keyState;
        }

        public static string GetKeyName(Keys k)
        {
            switch (k)
            {
                case Keys.LControlKey:
                    return "Ctrl";
                case Keys.LShiftKey:
                    return "Shift";
                case Keys.LMenu:
                    return "Alt";
                case Keys.RControlKey:
                    return "R Ctrl";
                case Keys.RShiftKey:
                    return "R Shift";
                case Keys.RMenu:
                    return "R Alt";
                case Keys.Back:
                    return "Backspace";
                default:
                    string rtn = kc.ConvertToString(k);
                    if (rtn.StartsWith("Oem"))
                        rtn = ToAscii(k, 0) + "";
                    return rtn;
            }

        }

        public static string GetExeLocation()
        {
            return System.Reflection.Assembly.GetEntryAssembly().Location;
        }

        public static string GetValueFromRegistry(string subkey, string key)
        {
            string rtn = null;

            try
            {
                RegistryKey RegK = Registry.CurrentUser.OpenSubKey(subkey);


                if (RegK != null)
                {
                    rtn = (string)RegK.GetValue(key);

                    RegK.Close();
                }
            }
            catch
            {
                Console.WriteLine("Failed to read registry");
            }
            return rtn;
        }

        public static bool DeleteValueFromRegistry(string subkey, string key)
        {
            try
            {
                RegistryKey RegK = Registry.CurrentUser.OpenSubKey(subkey, true);


                if (RegK != null)
                {
                    RegK.DeleteValue(key, false);

                    RegK.Close();
                }

                return true;
            }
            catch
            {
                Console.WriteLine("Failed to delete registry");
                return false;
            }
        }

        public static bool SetValueToRegistry(string subkey, string key, object val)
        {
            try
            {
                RegistryKey RegK = Registry.CurrentUser.OpenSubKey(subkey, true);
                if (RegK == null)
                {
                    RegK = Registry.CurrentUser.CreateSubKey(subkey, true);
                }
                RegK.SetValue(key, val);
                RegK.Close();

                return true;
            }
            catch
            {
                Console.WriteLine("Failed to set registry");
                return false;
            }
        }


        public static void Shutdown(int exitcode = 0)
        {
            Program.OptionsForm.NotifyIcon1.Icon = null; // remove the icon
            Application.Exit();
            //Application.ExitThread();
            Environment.Exit(exitcode);
        }
    }
}
