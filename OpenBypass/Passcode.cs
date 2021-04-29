using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Threading;
using System.IO.Compression;
using System.IO;
using Renci.SshNet;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenBypass
{
	public partial class Passcode : Form
	{
		public Passcode()
		{
			InitializeComponent();
		}
		string host = "127.0.0.1";
		string pass = "alpine";
		string user = "root";

		private void dumpButton_Click(object sender, EventArgs e)
		{
            try{
				KilliProxy();
				StartiProxy();
				SshClient sshclient = new SshClient(host, user, pass);
				ScpClient scpClient = new ScpClient("127.0.0.1", "root", "alpine");
				sshclient.Connect();
				scpClient.Connect();
				scpClient = new ScpClient("127.0.0.1", "root", "alpine");
				scpClient.Connect();
				string SN = sshclient.CreateCommand("ioreg -l | grep IOPlatformSerialNumber --color=never").Execute().Replace("\"", "").Replace("\t", "").Replace(" ", "").Replace("|", "").Replace("IOPlatformSerialNumber=", "").Trim().Remove(0, 13);
				if (Directory.Exists(Environment.CurrentDirectory + "/Required/Backup/" + SN))
				{
					Directory.Delete(Environment.CurrentDirectory + "/Required/Backup/" + SN, true);
				}
				Directory.CreateDirectory(Environment.CurrentDirectory + "/Required/Backup/" + SN);
				Directory.CreateDirectory(Environment.CurrentDirectory + "/Required/Backup/" + SN + "/FairPlay/");
				sshclient.CreateCommand("mount -o rw,union,update /").Execute();
				string text = sshclient.CreateCommand("find /private/var/containers/Data/System/ -iname 'internal'").Execute().Replace("Library/internal", "").Replace("\n", "").Replace("//", "/");
				scpClient.Download(text + "Library/internal/data_ark.plist", new FileInfo(Environment.CurrentDirectory + "/Required/Backup/" + SN + "/data_ark.plist"));
				scpClient.Download(text + "Library/activation_records/activation_record.plist", new FileInfo(Environment.CurrentDirectory + "/Required/Backup/" + SN + "/activation_record.plist"));
				scpClient.Download("/private/var/mobile/Library/FairPlay", new DirectoryInfo(Environment.CurrentDirectory + "/Required/Backup/" + SN + "/FairPlay/"));
				scpClient.Download("/private/var/wireless/Library/Preferences/com.apple.commcenter.device_specific_nobackup.plist", new FileInfo(Environment.CurrentDirectory + "/Required/Backup/" + SN + "/com.apple.commcenter.device_specific_nobackup.plist"));
				string startPath = Environment.CurrentDirectory + "/Required/Backup/" + SN;
				string zipPath = Environment.CurrentDirectory + "/Required/Backup/" + SN + ".zip";
				//string extractPath = @".\extract";
				ZipFile.CreateFromDirectory(startPath, zipPath);
				sshclient.Disconnect();
				scpClient.Disconnect();
				MessageBox.Show("Files dumped! Go to 'Extras' and reset your device to continue!");
			}
			catch(Exception error)
			{
				MessageBox.Show("Hey! We found an error! If you need help, make an issue on GitHub or show it in our Discord server: \n " + error.Message + "Error");
			}
		}
		public void KilliProxy()
		{
			foreach (var process in Process.GetProcessesByName("iproxy"))
			{
				process.Kill();
			}
		}
		public static void StartiProxy()
		{
			try
			{

				new Process
				{
					StartInfo =
						{
							FileName = Environment.CurrentDirectory + "/Required/iproxy.exe",
							Arguments = "22 44",
							UseShellExecute = false,
							RedirectStandardOutput = true,
							WindowStyle = ProcessWindowStyle.Hidden,
							CreateNoWindow = true
						}
				}.Start();
			}
			catch (System.ComponentModel.Win32Exception)
			{
				MessageBox.Show("iProxy not found! Is your reference folder deleted?");
			}
		}

        private void ActivateDevice_Click(object sender, EventArgs e)
        {
			KilliProxy();
			StartiProxy();
			SshClient sshclient = new SshClient(host, user, pass);
			ScpClient scpClient = new ScpClient("127.0.0.1", "root", "alpine");
			sshclient.Connect();
			scpClient.Connect();
			string SN = sshclient.CreateCommand("ioreg -l | grep IOPlatformSerialNumber --color=never").Execute().Replace("\"", "").Replace("\t", "").Replace(" ", "").Replace("|", "").Replace("IOPlatformSerialNumber=", "").Trim().Remove(0, 13);
			string zipPath = Environment.CurrentDirectory + "/ref/Backup/" + SN + ".zip";
			string extractPath = Environment.CurrentDirectory + "/ref/Backup/" + SN;
			sshclient.CreateCommand("mount -o rw,union,update /").Execute();
			sshclient.CreateCommand("rm -rf /var/mobile/Media/" + SN).Execute();
			sshclient.CreateCommand("mkdir /var/mobile/Media/Downloads / " + SN).Execute();
			string text = sshclient.CreateCommand("find /private/var/containers/Data/System/ -iname 'internal'").Execute().Replace("Library/internal", "").Replace("\n", "").Replace("//", "/");
			sshclient.CreateCommand("mkdir /var/mobile/Media/" + SN).Execute();
			scpClient.Upload(new DirectoryInfo(Environment.CurrentDirectory + "/Required/Backup/" + SN), "/var/mobile/Media/Downloads/" + SN);
			sshclient.CreateCommand("mv -f /var/mobile/Media/Downloads/" + SN + " /var/mobile/Media").Execute();
			sshclient.CreateCommand("chown -R mobile:mobile /var/mobile/Media/" + SN).Execute();
			sshclient.CreateCommand("chmod -R 755 /var/mobile/Media/" +SN).Execute();
			sshclient.CreateCommand("chmod 644 /var/mobile/Media/"+ SN + "/activation_record.plist").Execute();
			sshclient.CreateCommand("chmod 644 /var/mobile/Media/" + SN + "/data_ark.plist").Execute();
			sshclient.CreateCommand("chmod 644 /var/mobile/Media/" + SN + "/com.apple.commcenter.device_specific_nobackup.plist").Execute();
			sshclient.CreateCommand("killall backboardd && sleep 12").Execute();
			sshclient.CreateCommand("killall backboardd && sleep 12").Execute();
			sshclient.CreateCommand("mv -f /var/mobile/Media/" + SN +"/FairPlay /var/mobile/Library/FairPlay").Execute();
			sshclient.CreateCommand("chmod 755 /var/mobile/Library/FairPlay").Execute();
			string text2 = sshclient.CreateCommand("find /private/var/containers/Data/System/ -iname 'internal'").Execute().Replace("Library/internal", "").Replace("\n", "").Replace("//", "/");
			sshclient.CreateCommand("chflags nouchg " + text2 + "/Library/internal/data_ark.plist").Execute();
			sshclient.CreateCommand("mv - f /var/Mobile/Media/" + SN + " /var/containers/Data/System/X/Library/internal/data_ark.plist /var/containers/Data/System/" + text2 + "/Library/internal/data_ark.plist").Execute();
			sshclient.CreateCommand("chmod 755 " + text2 + "/Library/internal/data_ark.plist").Execute();
			sshclient.CreateCommand("chflags uchg " + text2 + "/Library/internal/data_ark.plist").Execute();
			sshclient.CreateCommand("mkdir -p /var/containers/Data/System" + text2 + "/Library/activation_records").Execute();
			sshclient.CreateCommand("chflags nouchg /var/containers/Data/System/" + text2 + "/Library/activation_records").Execute();
			sshclient.CreateCommand("mv -f /var/mobile/Media/" + SN + "/var/containers/Data/System/X/Library/activation_records/activation_record.plist /var/containers/Data/System/" + text2 + "/Library/activation_records/").Execute();
			//activation_record
			sshclient.CreateCommand("chmod 755 " + text2 + "/Library/activation_records/activation_record.plist").Execute();
			sshclient.CreateCommand("chflags uchg " + text2 + "/Library/activation_records/activation_record.plist").Execute();
			sshclient.CreateCommand("chflags nouchg /var/wireless/Library/Preferences/com.apple.commcenter.device_specific_nobackup.plist").Execute();
			sshclient.CreateCommand("mv -f /var/mobile/Media/"+ SN + "/com.apple.commcenter.device_specific_nobackup.plist /var/wireless/Library/Preferences/com.apple.commcenter.device_specific_nobackup.plist").Execute();
			sshclient.CreateCommand("chown root:mobile /var/wireless/Library/Preferences/com.apple.commcenter.device_specific_nobackup.plist").Execute();
			sshclient.CreateCommand("chmod 755 /var/wireless/Library/Preferences/com.apple.commcenter.device_specific_nobackup.plist").Execute();
			sshclient.CreateCommand("chown 755 /var/wireless/Library/Preferences/com.apple.commcenter.device_specific_nobackup.plist").Execute();
			sshclient.CreateCommand("chflags uchg /var/wireless/Library/Preferences/com.apple.commcenter.device_specific_nobackup.plist").Execute();
			sshclient.CreateCommand("rm -rf /var/mobile/Media/" + SN).Execute();
			sshclient.CreateCommand("launchctl unload /System/Library/LaunchDaemons/com.apple.mobileactivationd.plist").Execute();
			sshclient.CreateCommand("launchctl load /System/Library/LaunchDaemons/com.apple.mobileactivationd.plist").Execute();
			sshclient.CreateCommand("killall -9 backboardd").Execute();
			scpClient.Disconnect();
			sshclient.Disconnect();
			MessageBox.Show("Device activated! Thanks for using OpenBypass!");
			if (Directory.Exists(Environment.CurrentDirectory + "/Required/Backup/" + SN))
			{
				Directory.Delete(Environment.CurrentDirectory + "/Required/Backup/" + SN, true);
			}
		}

        private void aboutButton_Click(object sender, EventArgs e)
        {
			MessageBox.Show("Credit to VASKE & St0rm team for passcode bypass.");
        }
    }
}