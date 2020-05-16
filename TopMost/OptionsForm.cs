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
        public HashSet<Keys> RecordingCombinationAlpha;
        public HashSet<Keys> RecordingCombinationBeta;
        public HashSet<Keys> ShortcutCombination;
        public bool IsShortcutEnable;

        public OptionsForm()
        {
            InitializeComponent();
            API.gkh.KeyDown += new KeyEventHandler(gkh_KeyDown);
            API.gkh.KeyUp += new KeyEventHandler(gkh_KeyUp);
            RecordingCombinationAlpha = new HashSet<Keys>();
            RecordingCombinationBeta = new HashSet<Keys>();
            ShortcutCombination = API.Config.ShortcutCombination;
            AutoStartupCB.Checked = API.Config.IsAutoStart;
            AutoStartupCB.CheckedChanged += new System.EventHandler(AutoStartupCB_CheckedChanged);
            ShortcutEnableCB.Checked = IsShortcutEnable = API.Config.IsShortcutEnable;
            ShortcutEnableCB.CheckedChanged += new System.EventHandler(ShortcutEnableCB_CheckedChanged);

            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OptionsForm));
            NotifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NotifyIcon1.Icon = null; // remove the icon
            Application.Exit();
        }

        private void NotifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            API.ToggleTopMost();
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
                    ShortcutCombination.Clear();
                    foreach (Keys k in RecordingCombinationBeta)
                        ShortcutCombination.Add(k);
                }
                else
                {
                    foreach (Keys k in ShortcutCombination)
                        RecordingCombinationBeta.Add(k);
                }

                ShortcutDisplay.Text = API.GetKeyCombinationBreif(ShortcutCombination);
                API.Config.ShortcutCombination = ShortcutCombination;
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
            API.Config.IsAutoStart = AutoStartupCB.Checked;
            AutoStartupCB.Checked = API.Config.IsAutoStart;
        }

        private void ShortcutEnableCB_CheckedChanged(object sender, EventArgs e)
        {
            API.Config.IsShortcutEnable = ShortcutEnableCB.Checked;
            ShortcutEnableCB.Checked = IsShortcutEnable = API.Config.IsShortcutEnable;
        }

        void gkh_KeyUp(object sender, KeyEventArgs e)
        {
            // TODO

            if (ListenStatus == ListeningStatus.RECORDING)
            {
                RecordingCombinationAlpha.Remove(e.KeyCode);
                //e.Handled = true;
            }
            else
            {
                if (!IsShortcutEnable) return;
            }
        }

        void gkh_KeyDown(object sender, KeyEventArgs e)
        {
            // TODO

            if (ListenStatus == ListeningStatus.RECORDING)
            {
                if (RecordingCombinationAlpha.Count == 0)
                    RecordingCombinationBeta.Clear();
                RecordingCombinationAlpha.Add(e.KeyCode);
                RecordingCombinationBeta.Add(e.KeyCode);
                //e.Handled = true;


                ShortcutDisplay.Text = API.GetKeyCombinationBreif(RecordingCombinationAlpha);
            }
            else
            {
                if (!IsShortcutEnable) return;
            }


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
                ShortcutDisplay.Text = API.GetKeyCombinationBreif(ShortcutCombination);

                AutoStartupCB.Checked = API.Config.IsAutoStart;

                ShortcutEnableCB.Checked = IsShortcutEnable = API.Config.IsShortcutEnable;
            }
        }

    }
}
