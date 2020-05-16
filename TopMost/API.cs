using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
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

        public static class Config
        {
            const string AUTORUN_SUBKEY = "Software\\Microsoft\\Windows\\CurrentVersion\\Run";
            const string APPCONFIG_SUBKEY = "Software\\TopMost2";

            public static bool IsAutoStart
            {
                get
                {
                    string expected = '"' + GetExeLocation() + '"';

                    return GetValueFromRegistry(AUTORUN_SUBKEY, "TopMost2") == expected;
                }

                set
                {
                    if (value)
                        SetValueToRegistry(AUTORUN_SUBKEY, "TopMost2", '"' + GetExeLocation() + '"');
                    else
                        DeleteValueFromRegistry(AUTORUN_SUBKEY, "TopMost2");
                }
            }

            public static bool IsShortcutEnable
            {
                get
                {
                    return GetValueFromRegistry(APPCONFIG_SUBKEY, "ShortcutEnable") == "1";
                }
                set
                {
                    SetValueToRegistry(APPCONFIG_SUBKEY, "ShortcutEnable", value ? "1" : "0");
                }
            }

            public static HashSet<Keys> ShortcutCombination
            {
                get
                {
                    HashSet<Keys> rtn = new HashSet<Keys>();

                    string raw = GetValueFromRegistry(APPCONFIG_SUBKEY, "ShortcutKeys");

                    if (raw != null)
                    {
                        string[] splitted = raw.Split(',');
                        foreach (string token in splitted)
                        {
                            rtn.Add((Keys)Convert.ToInt32(token));
                        }
                    }

                    if (rtn.Count == 0)
                    {
                        // make default
                        rtn.Add(Keys.LControlKey);
                        rtn.Add(Keys.LMenu);
                        rtn.Add(Keys.Space);
                    }

                    return rtn;
                }
                set
                {
                    string write = "";

                    foreach (Keys k in value)
                    {
                        if (write != "") write += ",";
                        write += (int)k;
                    }

                    SetValueToRegistry(APPCONFIG_SUBKEY, "ShortcutKeys", write);
                }
            }
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


        public static WinEventDelegate dele = null;
        public static ArrayList TopmostWindowsLog = new ArrayList();
        public static IntPtr lastHwnd;
        public static IntPtr cureentHwnd;
        public static GlobalKeyboardHook gkh = new GlobalKeyboardHook();

        private static KeysConverter kc = new KeysConverter();


        public delegate void WinEventDelegate(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);


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

        public static void RestAllTopMost()
        {
            foreach (IntPtr hwnd in TopmostWindowsLog)
                SetTopMost(hwnd, false);
        }

        public static void ToggleTopMost()
        {
            // current Hwnd is the task bar, we dont need that
            bool now_topmost = (GetWindowLong(lastHwnd, GWL_EXSTYLE) & WS_EX_TOPMOST) != 0;

            SetTopMost(lastHwnd, !now_topmost);
        }

        public static void SetTopMost(IntPtr hwnd, bool topmost)
        {
            Console.WriteLine(GetWindowTitle(hwnd) + " set to " + topmost);
            //if (TopmostWindowsLog.IndexOf(hwnd) != -1)
            if (topmost)
                if (TopmostWindowsLog.IndexOf(hwnd) == -1)
                    TopmostWindowsLog.Add(hwnd);
                else
                    TopmostWindowsLog.Remove(hwnd);
            SetWindowPos(hwnd, topmost ? HWND_TOPMOST : HWND_NOTOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_SHOWWINDOW);
        }

        public static string GetKeyCombinationBreif(HashSet<Keys> keyset)
        {
            string msg = "";
            foreach (Keys k in keyset)
            {
                if (msg != "") msg += " + ";

                msg += kc.ConvertToString(k);
            }
            return msg + "\n"; // important
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
    }
}
