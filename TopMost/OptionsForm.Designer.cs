namespace TopMost2
{
    partial class OptionsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OptionsForm));
            this.NotifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.NotifyIconMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ResetAllStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ShortcutDisplay = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SetShortcutBtn = new System.Windows.Forms.Button();
            this.ShortcutEnableCB = new System.Windows.Forms.CheckBox();
            this.AutoStartupCB = new System.Windows.Forms.CheckBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.NotifyIconMenuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // NotifyIcon1
            // 
            this.NotifyIcon1.ContextMenuStrip = this.NotifyIconMenuStrip1;
            this.NotifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("NotifyIcon1.Icon")));
            this.NotifyIcon1.Text = "TopMost2";
            this.NotifyIcon1.Visible = true;
            this.NotifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.NotifyIcon1_MouseDoubleClick);
            // 
            // NotifyIconMenuStrip1
            // 
            this.NotifyIconMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem,
            this.optionsToolStripMenuItem,
            this.clearAllToolStripMenuItem,
            this.ResetAllStripMenuItem});
            this.NotifyIconMenuStrip1.Name = "NotifyIconMenuStrip1";
            this.NotifyIconMenuStrip1.Size = new System.Drawing.Size(120, 92);
            this.NotifyIconMenuStrip1.TabStop = true;
            this.NotifyIconMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.NotifyIconMenuStrip1_Opening);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.optionsToolStripMenuItem.Text = "Options";
            this.optionsToolStripMenuItem.Click += new System.EventHandler(this.optionsToolStripMenuItem_Click);
            // 
            // clearAllToolStripMenuItem
            // 
            this.clearAllToolStripMenuItem.Name = "clearAllToolStripMenuItem";
            this.clearAllToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.clearAllToolStripMenuItem.Text = "Clear All";
            this.clearAllToolStripMenuItem.Click += new System.EventHandler(this.clearAllToolStripMenuItem_Click);
            // 
            // ResetAllStripMenuItem
            // 
            this.ResetAllStripMenuItem.Name = "ResetAllStripMenuItem";
            this.ResetAllStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.ResetAllStripMenuItem.Text = "Reset All";
            this.ResetAllStripMenuItem.Click += new System.EventHandler(this.ResetAllStripMenuItem_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.SystemColors.ControlLight;
            this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(109, 140);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 17);
            this.label3.TabIndex = 5;
            this.label3.Text = "TopMost2";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.SystemColors.ControlLight;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(18, 164);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(249, 17);
            this.label4.TabIndex = 6;
            this.label4.Text = "Copyright (c) 2020 Jerrylum - MIT License";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.SystemColors.ControlLight;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(29, 182);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(228, 17);
            this.label5.TabIndex = 7;
            this.label5.Text = "https://github.com/jerrylum/topmost2";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.Controls.Add(this.ShortcutDisplay);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.SetShortcutBtn);
            this.panel1.Controls.Add(this.ShortcutEnableCB);
            this.panel1.Controls.Add(this.AutoStartupCB);
            this.panel1.Location = new System.Drawing.Point(-8, -1);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(300, 130);
            this.panel1.TabIndex = 8;
            // 
            // ShortcutDisplay
            // 
            this.ShortcutDisplay.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.ShortcutDisplay.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ShortcutDisplay.Location = new System.Drawing.Point(33, 86);
            this.ShortcutDisplay.Name = "ShortcutDisplay";
            this.ShortcutDisplay.Size = new System.Drawing.Size(174, 21);
            this.ShortcutDisplay.TabIndex = 13;
            this.ShortcutDisplay.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.SystemColors.Control;
            this.label1.Location = new System.Drawing.Point(31, 69);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 12);
            this.label1.TabIndex = 10;
            this.label1.Text = "Shortcut:";
            // 
            // SetShortcutBtn
            // 
            this.SetShortcutBtn.Location = new System.Drawing.Point(213, 84);
            this.SetShortcutBtn.Name = "SetShortcutBtn";
            this.SetShortcutBtn.Size = new System.Drawing.Size(56, 23);
            this.SetShortcutBtn.TabIndex = 9;
            this.SetShortcutBtn.Text = "Edit";
            this.SetShortcutBtn.UseVisualStyleBackColor = true;
            this.SetShortcutBtn.Click += new System.EventHandler(this.SetShortcutBtn_Click);
            // 
            // ShortcutEnableCB
            // 
            this.ShortcutEnableCB.AutoSize = true;
            this.ShortcutEnableCB.BackColor = System.Drawing.SystemColors.Control;
            this.ShortcutEnableCB.Location = new System.Drawing.Point(32, 42);
            this.ShortcutEnableCB.Name = "ShortcutEnableCB";
            this.ShortcutEnableCB.Size = new System.Drawing.Size(215, 16);
            this.ShortcutEnableCB.TabIndex = 7;
            this.ShortcutEnableCB.Text = "Keyboard shortcut for toggling top most ";
            this.ShortcutEnableCB.UseVisualStyleBackColor = false;
            
            // 
            // AutoStartupCB
            // 
            this.AutoStartupCB.AutoSize = true;
            this.AutoStartupCB.BackColor = System.Drawing.SystemColors.Control;
            this.AutoStartupCB.Location = new System.Drawing.Point(32, 20);
            this.AutoStartupCB.Name = "AutoStartupCB";
            this.AutoStartupCB.Size = new System.Drawing.Size(215, 16);
            this.AutoStartupCB.TabIndex = 6;
            this.AutoStartupCB.Text = "Automatically start with Windows startup";
            this.AutoStartupCB.UseVisualStyleBackColor = false;
            
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.ControlLight;
            this.panel2.Location = new System.Drawing.Point(-8, 128);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(300, 100);
            this.panel2.TabIndex = 9;
            // 
            // OptionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 211);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OptionsForm";
            this.Padding = new System.Windows.Forms.Padding(8);
            this.Text = "TopMost2 - Options";
            this.VisibleChanged += new System.EventHandler(this.OptionsForm_VisibleChanged);
            this.NotifyIconMenuStrip1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NotifyIcon NotifyIcon1;
        private System.Windows.Forms.ContextMenuStrip NotifyIconMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ResetAllStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearAllToolStripMenuItem;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button SetShortcutBtn;
        private System.Windows.Forms.CheckBox ShortcutEnableCB;
        private System.Windows.Forms.CheckBox AutoStartupCB;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label ShortcutDisplay;
    }
}