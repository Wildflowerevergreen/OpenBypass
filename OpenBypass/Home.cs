using System;
using System.Drawing;
using System.Diagnostics;
using FontAwesome.Sharp;
using System.IO;
using System.Windows.Forms;

namespace OpenBypass
{
    public partial class Home : Form
    {
        private Panel leftBorderBtn;
        private IconButton currentBtn;
        private Form currentChildForm;
        public Home()
        {
            InitializeComponent();
            leftBorderBtn = new Panel();
            leftBorderBtn.Size = new Size(7, 47);
            mainPanel.Controls.Add(leftBorderBtn);
        }
        private void Main_Load(object sender, System.EventArgs e)
        {
            MessageBox.Show("This app is 100% free & open source. If you want to contribute to this or join our Discord server, click the corresponding buttons on the bottom left.");
            if (File.Exists(@"%USERPROFILE%\.ssh\known_hosts"))
            {
                File.Delete(@"%USERPROFILE%\.ssh\known_hosts");
            }
            foreach (var process in Process.GetProcessesByName("iproxy"))
            {
                process.Kill();
            }
            try
            {
                foreach (Process process in Process.GetProcesses())
                {
                    if (process.ProcessName == "iTunes")
                    {
                        process.Kill();
                    }
                    if (process.ProcessName == "iTunesHelper")
                    {
                        process.Kill();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
        }
        private void OpenChildForm(Form childForm)
        {
            if (currentChildForm != null)
            {
                currentChildForm.Close();
            }
            currentChildForm = childForm;
            //End
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            panelDesktop.Controls.Add(childForm);
            panelDesktop.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }

        private void iconButton4_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColor.color1);
            OpenChildForm(new susicivus());
        }


        private void iconButton1_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColor.color1);
            OpenChildForm(new MDM());
        }
        private struct RGBColor
        {
           public static Color color1 = Color.FromArgb(172,  126,  241);
           public static Color color2 = Color.FromArgb(249,  118,  176);
           public static Color color3 = Color.FromArgb(253,  138,  114);
           public static Color color4 = Color.FromArgb(95,  77,  221);
           public static Color color5 = Color.FromArgb(249,  88,  155);
           public static Color color6 = Color.FromArgb(24,  161,  251);
        }
        private void ActivateButton(object senderBtn, Color color)
        {
            if (senderBtn != null)
            {
                DisableHighlight();
                currentBtn = (IconButton)senderBtn;
                currentBtn.BackColor = Color.FromArgb(37,36,81);
                currentBtn.ForeColor = color;
                currentBtn.TextAlign = ContentAlignment.MiddleCenter;
                currentBtn.IconColor = color;
                currentBtn.TextImageRelation = TextImageRelation.TextBeforeImage;
                currentBtn.ImageAlign = ContentAlignment.MiddleRight;

                leftBorderBtn.BackColor = color;
                leftBorderBtn.Location = new Point(0, currentBtn.Location.Y);
                leftBorderBtn.Visible = true;
                leftBorderBtn.BringToFront();
            }
        }
        private void DisableHighlight()
        {
            if (currentBtn != null)
            {
                currentBtn.BackColor = Color.FromArgb(31, 30, 68);
                currentBtn.ForeColor = Color.White;
                currentBtn.TextAlign = ContentAlignment.MiddleLeft;
                currentBtn.IconColor = Color.White;
                currentBtn.TextImageRelation = TextImageRelation.ImageBeforeText;
                currentBtn.ImageAlign = ContentAlignment.MiddleLeft;
            }
        }

        private void aboutButton_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://discord.gg/cUa7WVmx7E");
        }

        private void iconButton2_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://bit.ly/32tinEh");
        }

        private void iconButton3_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColor.color1);
            OpenChildForm(new Activation());
        }
    }
}
