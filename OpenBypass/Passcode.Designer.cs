
namespace OpenBypass
{
    partial class Passcode
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
            this.dumpButton = new System.Windows.Forms.Button();
            this.ActivateDevice = new System.Windows.Forms.Button();
            this.aboutButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // dumpButton
            // 
            this.dumpButton.Location = new System.Drawing.Point(246, 119);
            this.dumpButton.Name = "dumpButton";
            this.dumpButton.Size = new System.Drawing.Size(127, 64);
            this.dumpButton.TabIndex = 0;
            this.dumpButton.Text = "Dump Files";
            this.dumpButton.UseVisualStyleBackColor = true;
            this.dumpButton.Click += new System.EventHandler(this.dumpButton_Click);
            // 
            // ActivateDevice
            // 
            this.ActivateDevice.Location = new System.Drawing.Point(246, 204);
            this.ActivateDevice.Name = "ActivateDevice";
            this.ActivateDevice.Size = new System.Drawing.Size(127, 64);
            this.ActivateDevice.TabIndex = 1;
            this.ActivateDevice.Text = "Activate Device";
            this.ActivateDevice.UseVisualStyleBackColor = true;
            this.ActivateDevice.Click += new System.EventHandler(this.ActivateDevice_Click);
            // 
            // aboutButton
            // 
            this.aboutButton.Location = new System.Drawing.Point(269, 357);
            this.aboutButton.Name = "aboutButton";
            this.aboutButton.Size = new System.Drawing.Size(75, 23);
            this.aboutButton.TabIndex = 2;
            this.aboutButton.Text = "Credit";
            this.aboutButton.UseVisualStyleBackColor = true;
            this.aboutButton.Click += new System.EventHandler(this.aboutButton_Click);
            // 
            // Passcode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(609, 392);
            this.Controls.Add(this.aboutButton);
            this.Controls.Add(this.ActivateDevice);
            this.Controls.Add(this.dumpButton);
            this.Name = "Passcode";
            this.Text = "Passcode";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button dumpButton;
        private System.Windows.Forms.Button ActivateDevice;
        private System.Windows.Forms.Button aboutButton;
    }
}