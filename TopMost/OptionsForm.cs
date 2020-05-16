using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using TopMost;

namespace TopMost2
{
    public partial class OptionsForm : Form
    {

        public OptionsForm()
        {
            InitializeComponent();
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

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            if (e.CloseReason == CloseReason.WindowsShutDown) return;

            e.Cancel = true; // Dont close the form, hide it

            Hide();
        }
    }
}
