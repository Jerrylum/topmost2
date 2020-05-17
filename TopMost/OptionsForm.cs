using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace TopMost2
{
    public partial class OptionsForm : Form
    {

        public enum ListeningStatus
        {
            RUNNING,
            RECORDING
        }

        public ListeningStatus ListenStatus = ListeningStatus.RUNNING;
        public bool RecordingReleasing = false; // For set config use
        public HashSet<Keys> RecordingCombinationAlpha; // For set config use
        public HashSet<Keys> RecordingCombinationBeta; // For set config use
        public HashSet<Keys> ListeningCombination; // For trigger use
        public HashSet<Keys> TargetCombination;
        public bool IsShortcutEnable;

        public Icon RedIcon;
        public Icon GreenIcon;

        public OptionsForm()
        {
            InitializeComponent();
            API.gkh.KeyDown += new KeyEventHandler(gkh_KeyDown);
            API.gkh.KeyUp += new KeyEventHandler(gkh_KeyUp);
            RecordingCombinationAlpha = new HashSet<Keys>();
            RecordingCombinationBeta = new HashSet<Keys>();
            TargetCombination = Reg.ShortcutCombination;
            AutoStartupCB.Checked = Reg.IsAutoStart;
            AutoStartupCB.CheckedChanged += new System.EventHandler(AutoStartupCB_CheckedChanged);
            ShortcutEnableCB.Checked = IsShortcutEnable = Reg.IsShortcutEnable;
            ShortcutEnableCB.CheckedChanged += new System.EventHandler(ShortcutEnableCB_CheckedChanged);
            ListeningCombination = new HashSet<Keys>();

            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OptionsForm));
            RedIcon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            GreenIcon = Properties.Resources.IconGreen;

            NotifyIcon1.Icon = RedIcon;
            UpdateNotifyIcon();
        }

        public void UpdateNotifyIcon()
        {
            // Taskbar -> Always RED
            // Other Program -> Read the value
            //Icon NewIcon = !API.IsTaskBar(API.cureentHwnd) && API.IsTopMost(API.cureentHwnd) ? GreenIcon : RedIcon;
            Icon NewIcon = API.IsTopMost(API.cureentHwnd) ? GreenIcon : RedIcon;

            if (NotifyIcon1.Icon != NewIcon)
                NotifyIcon1.Icon = NewIcon;
        }

        public void StartRecord()
        {
            ShortcutDisplay.Text = "Recording";
            SetShortcutBtn.Text = "Done";
            ListenStatus = ListeningStatus.RECORDING;
            RecordingCombinationAlpha.Clear();
            RecordingCombinationBeta.Clear();
        }

        public void StopRecord()
        {
            SetShortcutBtn.Text = "Edit";
            ListenStatus = ListeningStatus.RUNNING;

            if (RecordingCombinationBeta.Count != 0)
            {
                TargetCombination.Clear();
                foreach (Keys k in RecordingCombinationBeta)
                    TargetCombination.Add(k);
            }
            else
            {
                foreach (Keys k in TargetCombination)
                    RecordingCombinationBeta.Add(k);
            }

            ShortcutDisplay.Text = API.GetKeyCombinationBreif(TargetCombination);
            Reg.ShortcutCombination = TargetCombination;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            API.Shutdown();
        }

        private void NotifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            // current Hwnd is the task bar, we dont need that

            // current Hwnd, TEST
            API.ToggleTopMost(API.cureentHwnd);
        }

        private void NotifyIconMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            Console.WriteLine("Menu Opening");

            windowListToolStripMenuItem.DropDownItems.Clear();

            Process[] procs = Process.GetProcesses();
            IntPtr hWnd;
            foreach (Process proc in procs)
            {
                if ((hWnd = proc.MainWindowHandle) != IntPtr.Zero && !API.IsWindowsCore(hWnd))
                {
                    //Console.WriteLine(API.GetWindowTitle(hWnd));
                    var WindowListItem = new ToolStripMenuItem();
                    WindowListItem.Name = "";
                    WindowListItem.Checked = API.IsTopMost(hWnd);
                    WindowListItem.Size = new System.Drawing.Size(180, 22);
                    WindowListItem.Text = API.GetWindowTitle(hWnd);
                    WindowListItem.Tag = hWnd;
                    WindowListItem.Click += new System.EventHandler(this.WindowsListMenuItem_Click);
                    windowListToolStripMenuItem.DropDownItems.Add(WindowListItem);
                }
            }
        }

        private void ResetAllStripMenuItem_Click(object sender, EventArgs e)
        {
            API.RestAllTopMost();
        }

        private void clearAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process[] procs = Process.GetProcesses();
            IntPtr hWnd;
            foreach (Process proc in procs)
            {
                if ((hWnd = proc.MainWindowHandle) != IntPtr.Zero)
                {
                    API.SetTopMost(hWnd, false, false);
                }
            }
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Show();
        }

        private void SetShortcutBtn_Click(object sender, EventArgs e)
        {
            if (ListenStatus == ListeningStatus.RECORDING) // end recording
            {
                StopRecord();
            }
            else  // start recording
            {
                StartRecord();
            }

            //SetShortcutBtn
        }

        private void AutoStartupCB_CheckedChanged(object sender, EventArgs e)
        {
            Reg.IsAutoStart = AutoStartupCB.Checked;
            AutoStartupCB.Checked = Reg.IsAutoStart;
        }

        private void ShortcutEnableCB_CheckedChanged(object sender, EventArgs e)
        {
            Reg.IsShortcutEnable = ShortcutEnableCB.Checked;
            ShortcutEnableCB.Checked = IsShortcutEnable = Reg.IsShortcutEnable;
            ListeningCombination.Clear();
        }

        private void WindowsListMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;

            IntPtr hWnd = (IntPtr)item.Tag;

            API.SetTopMost(hWnd, !item.Checked);
        }

        private void gkh_KeyUp(object sender, KeyEventArgs e)
        {

            if (ListenStatus == ListeningStatus.RECORDING)
            {
                RecordingCombinationAlpha.Remove(e.KeyCode);

                RecordingReleasing = true;

                e.Handled = true;
            }
            else // if recording, should not listening short cut
            {
                if (IsShortcutEnable && ListeningCombination.SetEquals(TargetCombination))
                    if (!API.IsWindowsCore(API.cureentHwnd))
                        API.ToggleTopMost(API.cureentHwnd);
            }

            ListeningCombination.Remove(e.KeyCode);

        }

        private void gkh_KeyDown(object sender, KeyEventArgs e)
        {

            if (ListenStatus == ListeningStatus.RECORDING)
            {
                if (RecordingCombinationAlpha.Count == 0)
                    RecordingCombinationBeta.Clear();

                if (RecordingReleasing)
                    RecordingCombinationBeta = new HashSet<Keys>(RecordingCombinationAlpha);

                RecordingReleasing = false;

                RecordingCombinationAlpha.Add(e.KeyCode);
                RecordingCombinationBeta.Add(e.KeyCode);
                e.Handled = true;


                ShortcutDisplay.Text = API.GetKeyCombinationBreif(RecordingCombinationAlpha);
            }
            ListeningCombination.Add(e.KeyCode);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            if (e.CloseReason == CloseReason.WindowsShutDown) return;

            e.Cancel = true; // Dont close the form, hide it

            Hide();
        }

        private void OptionsForm_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                ShortcutDisplay.Text = API.GetKeyCombinationBreif(TargetCombination);

                AutoStartupCB.Checked = Reg.IsAutoStart;

                ShortcutEnableCB.Checked = IsShortcutEnable = Reg.IsShortcutEnable;
            }
        }

        private void OptionsForm_Leave(object sender, EventArgs e)
        {

        }

        private void OptionsForm_Validating(object sender, CancelEventArgs e)
        {

        }

        private void OptionsForm_Deactivate(object sender, EventArgs e)
        {
            if (ListenStatus == ListeningStatus.RECORDING) // end recording
            {
                StopRecord();
            }
        }
    }
}
