using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
        public HashSet<Keys> RecordingCombinationAlpha; // For set config use
        public HashSet<Keys> RecordingCombinationBeta; // For set config use
        public HashSet<Keys> ListeningCombination; // For trigger use
        public HashSet<Keys> TargetCombination;
        public bool IsShortcutEnable;

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
            NotifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            API.Shutdown();
        }

        private void NotifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            // current Hwnd is the task bar, we dont need that
            API.ToggleTopMost(API.lastHwnd);
        }

        private void NotifyIconMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            Console.WriteLine("Menu Opening");
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
                    API.SetTopMost(hWnd, false);
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
            else  // start recording
            {
                ShortcutDisplay.Text = "Recording";
                SetShortcutBtn.Text = "Done";
                ListenStatus = ListeningStatus.RECORDING;
                RecordingCombinationAlpha.Clear();
                RecordingCombinationBeta.Clear();
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

        void gkh_KeyUp(object sender, KeyEventArgs e)
        {
            if (ListenStatus == ListeningStatus.RECORDING)
            {
                RecordingCombinationAlpha.Remove(e.KeyCode);
                e.Handled = true;
            }

            if (!IsShortcutEnable) return;

            if (ListeningCombination.SetEquals(TargetCombination))
                API.ToggleTopMost(API.cureentHwnd);


            ListeningCombination.Remove(e.KeyCode);
        }

        void gkh_KeyDown(object sender, KeyEventArgs e)
        {
            if (ListenStatus == ListeningStatus.RECORDING)
            {
                if (RecordingCombinationAlpha.Count == 0)
                    RecordingCombinationBeta.Clear();
                RecordingCombinationAlpha.Add(e.KeyCode);
                RecordingCombinationBeta.Add(e.KeyCode);
                e.Handled = true;


                ShortcutDisplay.Text = API.GetKeyCombinationBreif(RecordingCombinationAlpha);
            }

            if (!IsShortcutEnable) return;
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

    }
}
