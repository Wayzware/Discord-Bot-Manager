namespace Discord_Bot_Manager
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.TextBoxConsoleA = new System.Windows.Forms.TextBox();
            this.ButtonStartBot = new System.Windows.Forms.Button();
            this.CheckBoxAutoScroll = new System.Windows.Forms.CheckBox();
            this.ButtonStopBot = new System.Windows.Forms.Button();
            this.ButtonKillBot = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // TextBoxConsoleA
            // 
            this.TextBoxConsoleA.AcceptsReturn = true;
            this.TextBoxConsoleA.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.TextBoxConsoleA.Location = new System.Drawing.Point(13, 13);
            this.TextBoxConsoleA.Multiline = true;
            this.TextBoxConsoleA.Name = "TextBoxConsoleA";
            this.TextBoxConsoleA.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TextBoxConsoleA.Size = new System.Drawing.Size(536, 314);
            this.TextBoxConsoleA.TabIndex = 2;
            this.TextBoxConsoleA.WordWrap = false;
            // 
            // ButtonStartBot
            // 
            this.ButtonStartBot.Location = new System.Drawing.Point(13, 333);
            this.ButtonStartBot.Name = "ButtonStartBot";
            this.ButtonStartBot.Size = new System.Drawing.Size(264, 23);
            this.ButtonStartBot.TabIndex = 1;
            this.ButtonStartBot.Text = "Start Household Bot";
            this.ButtonStartBot.UseVisualStyleBackColor = true;
            this.ButtonStartBot.Click += new System.EventHandler(this.ButtonStartBot_Click);
            // 
            // CheckBoxAutoScroll
            // 
            this.CheckBoxAutoScroll.AutoSize = true;
            this.CheckBoxAutoScroll.Location = new System.Drawing.Point(438, 362);
            this.CheckBoxAutoScroll.Name = "CheckBoxAutoScroll";
            this.CheckBoxAutoScroll.Size = new System.Drawing.Size(111, 19);
            this.CheckBoxAutoScroll.TabIndex = 3;
            this.CheckBoxAutoScroll.Text = "Stop Auto Scroll";
            this.CheckBoxAutoScroll.UseVisualStyleBackColor = true;
            // 
            // ButtonStopBot
            // 
            this.ButtonStopBot.Location = new System.Drawing.Point(13, 362);
            this.ButtonStopBot.Name = "ButtonStopBot";
            this.ButtonStopBot.Size = new System.Drawing.Size(264, 23);
            this.ButtonStopBot.TabIndex = 1;
            this.ButtonStopBot.Text = "Stop Household Bot";
            this.ButtonStopBot.UseVisualStyleBackColor = true;
            this.ButtonStopBot.Click += new System.EventHandler(this.ButtonStopBot_Click);
            // 
            // ButtonKillBot
            // 
            this.ButtonKillBot.Location = new System.Drawing.Point(283, 362);
            this.ButtonKillBot.Name = "ButtonKillBot";
            this.ButtonKillBot.Size = new System.Drawing.Size(75, 23);
            this.ButtonKillBot.TabIndex = 4;
            this.ButtonKillBot.Text = "Kill";
            this.ButtonKillBot.UseVisualStyleBackColor = true;
            this.ButtonKillBot.Click += new System.EventHandler(this.ButtonKillBot_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(561, 397);
            this.Controls.Add(this.ButtonKillBot);
            this.Controls.Add(this.ButtonStopBot);
            this.Controls.Add(this.CheckBoxAutoScroll);
            this.Controls.Add(this.ButtonStartBot);
            this.Controls.Add(this.TextBoxConsoleA);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(577, 436);
            this.MinimumSize = new System.Drawing.Size(577, 436);
            this.Name = "Form1";
            this.Text = "Wayzbot v1.0";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox TextBoxConsoleA;
        private System.Windows.Forms.Button ButtonStartBot;
        private System.Windows.Forms.CheckBox CheckBoxAutoScroll;
        private System.Windows.Forms.Button ButtonStopBot;
        private System.Windows.Forms.Button ButtonKillBot;
    }
}

