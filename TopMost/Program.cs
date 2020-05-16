using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;

using TopMost2;
using Utilities;

namespace TopMost
{
    static class Program
    {
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
        public static globalKeyboardHook gkh = new globalKeyboardHook();

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
            Console.WriteLine(Program.GetWindowTitle(hwnd) + " set to " + topmost);
            //if (TopmostWindowsLog.IndexOf(hwnd) != -1)
            if (topmost)
                if (TopmostWindowsLog.IndexOf(hwnd) == -1) 
                    TopmostWindowsLog.Add(hwnd);
            else
                TopmostWindowsLog.Remove(hwnd);
            SetWindowPos(hwnd, topmost ? HWND_TOPMOST : HWND_NOTOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_SHOWWINDOW);
        }



        [STAThread]
        static void Main()
        {

            dele = new WinEventDelegate(WinEventProc);
            IntPtr m_hhook = SetWinEventHook(EVENT_SYSTEM_FOREGROUND, EVENT_SYSTEM_FOREGROUND, IntPtr.Zero, dele, 0, 0, WINEVENT_OUTOFCONTEXT);

            cureentHwnd = GetForegroundWindow();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);


            new OptionsForm(); // Dont use `Application.Run(...);`. We want to hide the form

            Application.Run();
        }

    }
}
