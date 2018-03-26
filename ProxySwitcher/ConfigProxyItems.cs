using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ProxySwitcher
{
	
	public class ConfigProxyItems
	{
		[XmlArray("ProxyItems"), XmlArrayItem(typeof(ProxyItem), ElementName = "ProxyItem")]
		public List<ProxyItem> ProxyItems { get; set; }
	}
}
