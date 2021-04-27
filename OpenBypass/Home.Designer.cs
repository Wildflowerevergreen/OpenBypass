
namespace OpenBypass
{
    partial class Home
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Home));
            this.openMDM = new FontAwesome.Sharp.IconButton();
            this.openExtras = new FontAwesome.Sharp.IconButton();
            this.mainPanel = new System.Windows.Forms.Panel();
            this.openGit = new FontAwesome.Sharp.IconButton();
            this.openDiscord = new FontAwesome.Sharp.IconButton();
            this.label1 = new System.Windows.Forms.Label();
            this.panelDesktop = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.mainPanel.SuspendLayout();
            this.panelDesktop.SuspendLayout();
            this.SuspendLayout();
            // 
            // openMDM
            // 
            this.openMDM.BackColor = System.Drawing.Color.Transparent;
            this.openMDM.FlatAppearance.BorderSize = 0;
            this.openMDM.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.openMDM.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.openMDM.ForeColor = System.Drawing.Color.White;
            this.openMDM.IconChar = FontAwesome.Sharp.IconChar.Tasks;
            this.openMDM.IconColor = System.Drawing.Color.White;
            this.openMDM.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.openMDM.IconSize = 38;
            this.openMDM.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.openMDM.Location = new System.Drawing.Point(12, 97);
            this.openMDM.Name = "openMDM";
            this.openMDM.Padding = new System.Windows.Forms.Padding(10, 0, 20, 0);
            this.openMDM.Size = new System.Drawing.Size(189, 47);
            this.openMDM.TabIndex = 0;
            this.openMDM.Text = "MDM Bypass";
            this.openMDM.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.openMDM.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.openMDM.UseVisualStyleBackColor = false;
            this.openMDM.Click += new System.EventHandler(this.iconButton1_Click);
            // 
            // openExtras
            // 
            this.openExtras.BackColor = System.Drawing.Color.Transparent;
            this.openExtras.FlatAppearance.BorderSize = 0;
            this.openExtras.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.openExtras.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.openExtras.ForeColor = System.Drawing.Color.White;
            this.openExtras.IconChar = FontAwesome.Sharp.IconChar.Wrench;
            this.openExtras.IconColor = System.Drawing.Color.White;
            this.openExtras.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.openExtras.IconSize = 38;
            this.openExtras.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.openExtras.Location = new System.Drawing.Point(12, 163);
            this.openExtras.Name = "openExtras";
            this.openExtras.Padding = new System.Windows.Forms.Padding(10, 0, 20, 0);
            this.openExtras.Size = new System.Drawing.Size(189, 47);
            this.openExtras.TabIndex = 4;
            this.openExtras.Text = "Extras";
            this.openExtras.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.openExtras.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.openExtras.UseVisualStyleBackColor = false;
            this.openExtras.Click += new System.EventHandler(this.iconButton4_Click);
            // 
            // mainPanel
            // 
            this.mainPanel.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.mainPanel.Controls.Add(this.openGit);
            this.mainPanel.Controls.Add(this.openDiscord);
            this.mainPanel.Controls.Add(this.label1);
            this.mainPanel.Controls.Add(this.openExtras);
            this.mainPanel.Controls.Add(this.openMDM);
            this.mainPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.mainPanel.Location = new System.Drawing.Point(0, 0);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(198, 450);
            this.mainPanel.TabIndex = 0;
            // 
            // openGit
            // 
            this.openGit.BackColor = System.Drawing.Color.Transparent;
            this.openGit.FlatAppearance.BorderSize = 0;
            this.openGit.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.openGit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.openGit.ForeColor = System.Drawing.Color.White;
            this.openGit.IconChar = FontAwesome.Sharp.IconChar.Github;
            this.openGit.IconColor = System.Drawing.Color.White;
            this.openGit.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.openGit.IconSize = 38;
            this.openGit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.openGit.Location = new System.Drawing.Point(88, 400);
            this.openGit.Name = "openGit";
            this.openGit.Padding = new System.Windows.Forms.Padding(10, 0, 20, 0);
            this.openGit.Size = new System.Drawing.Size(63, 47);
            this.openGit.TabIndex = 6;
            this.openGit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.openGit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.openGit.UseVisualStyleBackColor = false;
            this.openGit.Click += new System.EventHandler(this.iconButton2_Click);
            // 
            // openDiscord
            // 
            this.openDiscord.BackColor = System.Drawing.Color.Transparent;
            this.openDiscord.FlatAppearance.BorderSize = 0;
            this.openDiscord.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.openDiscord.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.openDiscord.ForeColor = System.Drawing.Color.White;
            this.openDiscord.IconChar = FontAwesome.Sharp.IconChar.Discord;
            this.openDiscord.IconColor = System.Drawing.Color.White;
            this.openDiscord.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.openDiscord.IconSize = 38;
            this.openDiscord.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.openDiscord.Location = new System.Drawing.Point(19, 400);
            this.openDiscord.Name = "openDiscord";
            this.openDiscord.Padding = new System.Windows.Forms.Padding(10, 0, 20, 0);
            this.openDiscord.Size = new System.Drawing.Size(63, 47);
            this.openDiscord.TabIndex = 5;
            this.openDiscord.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.openDiscord.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.openDiscord.UseVisualStyleBackColor = false;
            this.openDiscord.Click += new System.EventHandler(this.aboutButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Myanmar Text", 17.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label1.Location = new System.Drawing.Point(12, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(148, 41);
            this.label1.TabIndex = 0;
            this.label1.Text = "OpenBypass";
            // 
            // panelDesktop
            // 
            this.panelDesktop.Controls.Add(this.label4);
            this.panelDesktop.Controls.Add(this.label3);
            this.panelDesktop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDesktop.Location = new System.Drawing.Point(198, 0);
            this.panelDesktop.Name = "panelDesktop";
            this.panelDesktop.Size = new System.Drawing.Size(602, 450);
            this.panelDesktop.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Myanmar Text", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(231, 173);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(110, 37);
            this.label4.TabIndex = 1;
            this.label4.Text = "Welcome!";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Myanmar Text", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(138, 210);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(298, 37);
            this.label3.TabIndex = 0;
            this.label3.Text = "Select an option to get started";
            // 
            // Home
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.panelDesktop);
            this.Controls.Add(this.mainPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Home";
            this.Text = " ";
            this.Load += new System.EventHandler(this.Main_Load);
            this.mainPanel.ResumeLayout(false);
            this.mainPanel.PerformLayout();
            this.panelDesktop.ResumeLayout(false);
            this.panelDesktop.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private FontAwesome.Sharp.IconButton openMDM;
        private FontAwesome.Sharp.IconButton openExtras;
        private System.Windows.Forms.Panel mainPanel;
        private System.Windows.Forms.Panel panelDesktop;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private FontAwesome.Sharp.IconButton openDiscord;
        private FontAwesome.Sharp.IconButton openGit;
    }
}

