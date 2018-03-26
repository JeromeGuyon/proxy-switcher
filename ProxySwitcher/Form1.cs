using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace ProxySwitcher
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();

			//register at logon
			RegistryKey registry = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run", true);
			registry.SetValue("ProxySwitcher", "\"" + System.Reflection.Assembly.GetEntryAssembly().Location + "\"");
		}

		private void exitToolStripMenuItem1_Click(object sender, EventArgs e)
		{
			Dispose();
		}


		private void noProxyToolStripMenuItem_Click(object sender, EventArgs e)
		{
			RegistryKey registry = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings", true);
			registry.SetValue("ProxyEnable", 0);

			if(registry.GetValue("AutoConfigURL") != null) registry.DeleteValue("AutoConfigURL");

			notifyUpdate();
		}

		private void SetProxyUrl(string url)
		{
			RegistryKey registry = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings", true);
			registry.SetValue("ProxyEnable", 0);
			registry.SetValue("AutoConfigURL", url);

			notifyUpdate();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			Visible = false;
			Opacity = 0;

			notifyIcon1.BalloonTipText = "ProxySwitcher is docked here !!";
			notifyIcon1.ShowBalloonTip(10000);

			// load from conf
			ReloadItems();
		}

		internal void UpdateItems()
		{
			ReloadItems();
		}

		private void ReloadItems()
		{
			contextMenuStrip1.Items.Clear();
			contextMenuStrip1.Items.Add(settingsToolStripMenuItem);
			contextMenuStrip1.Items.Add(toolStripSeparator2);

			try
			{
				XmlSerializer xs = new XmlSerializer(typeof(ConfigProxyItems));
				using (StreamReader rd = new StreamReader("proxy-conf.xml"))
				{
					ConfigProxyItems c = xs.Deserialize(rd) as ConfigProxyItems;

					foreach (ProxyItem item in c.ProxyItems)
					{
						if(item.Name != null)
						{
							ToolStripMenuItem tsm = new ToolStripMenuItem(item.Name);
							tsm.Click += (sender, e) => SetProxyUrl(item.ConfigUrl);

							contextMenuStrip1.Items.Add(tsm);
						}
							 
					}
				}
			}
			catch (FileNotFoundException ignored)
			{
				
			}
			contextMenuStrip1.Items.Add(noProxyToolStripMenuItem);
			contextMenuStrip1.Items.Add(toolStripSeparator1);
			contextMenuStrip1.Items.Add(exitToolStripMenuItem1);

		}

		[DllImport("wininet.dll")]
		public static extern bool InternetSetOption(IntPtr hInternet, int dwOption, IntPtr lpBuffer, int dwBufferLength);
		public const int INTERNET_OPTION_SETTINGS_CHANGED = 39;
		public const int INTERNET_OPTION_REFRESH = 37;

		private void notifyUpdate()
		{
			// These lines implement the Interface in the beginning of program 
			// They cause the OS to refresh the settings, causing IP to realy update
			InternetSetOption(IntPtr.Zero, INTERNET_OPTION_SETTINGS_CHANGED, IntPtr.Zero, 0);
			InternetSetOption(IntPtr.Zero, INTERNET_OPTION_REFRESH, IntPtr.Zero, 0);
		}

		private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if(settingsUI == null || settingsUI.IsDisposed) settingsUI = new SettingsUI(this);
			settingsUI.Show();
		}

		private SettingsUI settingsUI = null;

		private void contextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
		{

		}
	}
}
