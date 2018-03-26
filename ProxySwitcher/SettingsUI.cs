using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace ProxySwitcher
{
	public partial class SettingsUI : Form
	{
		private Form1 form1;

		public SettingsUI(Form1 form1)
		{
			InitializeComponent();

			this.form1 = form1;
		}

		private void SettingsUI_Load(object sender, System.EventArgs e)
		{
			try
			{
				XmlSerializer xs = new XmlSerializer(typeof(ConfigProxyItems));
				using (StreamReader rd = new StreamReader("proxy-conf.xml"))
				{
					ConfigProxyItems c = xs.Deserialize(rd) as ConfigProxyItems;

					foreach (ProxyItem item in c.ProxyItems)
					{
						if (item.Name != null)
						{
							dataGridView1.Rows.Add(item.Name, item.ConfigUrl);
						}
					}
				}
			}
			catch (FileNotFoundException ignored)
			{

			}
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			Dispose();
		}

		private void button2_Click(object sender, System.EventArgs e)
		{
			ConfigProxyItems c = new ConfigProxyItems();
			c.ProxyItems = new System.Collections.Generic.List<ProxyItem>();
			foreach (DataGridViewRow item in dataGridView1.Rows)
			{
				ProxyItem proxyItem = new ProxyItem();
				proxyItem.Name = item.Cells[0].Value as string;
				proxyItem.ConfigUrl = item.Cells[1].Value as string;

				c.ProxyItems.Add(proxyItem);
			}

			XmlSerializer xs = new XmlSerializer(typeof(ConfigProxyItems));
			using (StreamWriter rd = new StreamWriter("proxy-conf.xml"))
			{
				xs.Serialize(rd, c);
			}

			Dispose();
			form1.UpdateItems();
		}
	}
}
