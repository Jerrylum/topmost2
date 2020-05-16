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
            NotifyIcon1.Icon = SystemIcons.Application;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NotifyIcon1.Icon = null; // remove the icon
            Application.Exit();
        }

        private void NotifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            // current Hwnd is the task bar, we dont need that
            Console.WriteLine(Program.GetWindowTitle(Program.lastHwnd));
            Program.SetTopMost();
        }

        private void NotifyIconMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            Console.WriteLine("me");


        }

        private void ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void ResetAllStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
