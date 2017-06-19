namespace Glouton.LogGenerator
{
    partial class Form1
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
            this.buttonOpenGroup = new System.Windows.Forms.Button();
            this.buttonCloseGroup = new System.Windows.Forms.Button();
            this.buttonSendLine = new System.Windows.Forms.Button();
            this.listLogLevel = new System.Windows.Forms.ListBox();
            this.textBoxLine = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // buttonOpenGroup
            // 
            this.buttonOpenGroup.AutoSize = true;
            this.buttonOpenGroup.Location = new System.Drawing.Point(263, 93);
            this.buttonOpenGroup.Name = "buttonOpenGroup";
            this.buttonOpenGroup.Size = new System.Drawing.Size(139, 35);
            this.buttonOpenGroup.TabIndex = 0;
            this.buttonOpenGroup.Text = "Open Group";
            this.buttonOpenGroup.UseVisualStyleBackColor = true;
            this.buttonOpenGroup.Click += new System.EventHandler(this.buttonOpenGroup_Click);
            // 
            // buttonCloseGroup
            // 
            this.buttonCloseGroup.AutoSize = true;
            this.buttonCloseGroup.Location = new System.Drawing.Point(509, 93);
            this.buttonCloseGroup.Name = "buttonCloseGroup";
            this.buttonCloseGroup.Size = new System.Drawing.Size(142, 35);
            this.buttonCloseGroup.TabIndex = 1;
            this.buttonCloseGroup.Text = "Close Group";
            this.buttonCloseGroup.UseVisualStyleBackColor = true;
            this.buttonCloseGroup.Click += new System.EventHandler(this.buttonCloseGroup_Click);
            // 
            // buttonSendLine
            // 
            this.buttonSendLine.AutoSize = true;
            this.buttonSendLine.Location = new System.Drawing.Point(576, 183);
            this.buttonSendLine.Name = "buttonSendLine";
            this.buttonSendLine.Size = new System.Drawing.Size(119, 35);
            this.buttonSendLine.TabIndex = 2;
            this.buttonSendLine.Text = "Send Line";
            this.buttonSendLine.UseVisualStyleBackColor = true;
            this.buttonSendLine.Click += new System.EventHandler(this.buttonSendLine_Click);
            // 
            // listLogLevel
            // 
            this.listLogLevel.FormattingEnabled = true;
            this.listLogLevel.ItemHeight = 25;
            this.listLogLevel.Items.AddRange(new object[] {
            "Trace",
            "Info",
            "Warn",
            "Error",
            "Fatal",
            "Debug"});
            this.listLogLevel.Location = new System.Drawing.Point(102, 72);
            this.listLogLevel.Name = "listLogLevel";
            this.listLogLevel.Size = new System.Drawing.Size(120, 154);
            this.listLogLevel.TabIndex = 3;
            // 
            // textBoxLine
            // 
            this.textBoxLine.Location = new System.Drawing.Point(263, 183);
            this.textBoxLine.Name = "textBoxLine";
            this.textBoxLine.Size = new System.Drawing.Size(294, 31);
            this.textBoxLine.TabIndex = 4;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(920, 332);
            this.Controls.Add(this.textBoxLine);
            this.Controls.Add(this.listLogLevel);
            this.Controls.Add(this.buttonSendLine);
            this.Controls.Add(this.buttonCloseGroup);
            this.Controls.Add(this.buttonOpenGroup);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonOpenGroup;
        private System.Windows.Forms.Button buttonCloseGroup;
        private System.Windows.Forms.Button buttonSendLine;
        private System.Windows.Forms.ListBox listLogLevel;
        private System.Windows.Forms.TextBox textBoxLine;
    }
}

