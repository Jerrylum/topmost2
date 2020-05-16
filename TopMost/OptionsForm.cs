using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;
using TopMost;

namespace TopMost2
{
    public partial class OptionsForm : Form
    {
        private static KeysConverter kc = new KeysConverter();

        public enum ListeningStatus
        {
            RUNNING,
            RECORDING
        }

        private String GetKeyCombinationBreif(HashSet<Keys> keyset)
        {
            string msg = "";
            foreach (Keys k in keyset)
            {
                if (msg != "") msg += " + ";

                msg += kc.ConvertToString(k);
            }
            return msg + "\n"; // important
        }

        public ListeningStatus ListenStatus = ListeningStatus.RUNNING;
        public HashSet<Keys> RecordingCombinationAlpha;
        public HashSet<Keys> RecordingCombinationBeta;
        public HashSet<Keys> ShortcutCombination;

        public OptionsForm()
        {
            InitializeComponent();
            Program.gkh.KeyDown += new KeyEventHandler(gkh_KeyDown);
            Program.gkh.KeyUp += new KeyEventHandler(gkh_KeyUp);
            RecordingCombinationAlpha = new HashSet<Keys>();
            RecordingCombinationBeta = new HashSet<Keys>();
            ShortcutCombination = new HashSet<Keys>();
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
            Program.ToggleTopMost();
        }

        private void NotifyIconMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            Console.WriteLine("Menu Opening");
        }

        private void ResetAllStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.RestAllTopMost();
        }

        private void clearAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process[] procs = Process.GetProcesses();
            IntPtr hWnd;
            foreach (Process proc in procs)
            {
                if ((hWnd = proc.MainWindowHandle) != IntPtr.Zero)
                {
                    Program.SetTopMost(hWnd, false);
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

                ShortcutDisplay.Text = GetKeyCombinationBreif(ShortcutCombination);
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

        void gkh_KeyUp(object sender, KeyEventArgs e)
        {
            if (ListenStatus == ListeningStatus.RECORDING)
            {
                RecordingCombinationAlpha.Remove(e.KeyCode);
                //e.Handled = true;
            }
            else
            {

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


                ShortcutDisplay.Text = GetKeyCombinationBreif(RecordingCombinationAlpha);
            }
            else
            {

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
                ShortcutDisplay.Text = GetKeyCombinationBreif(ShortcutCombination);
            }
        }
    }
}
