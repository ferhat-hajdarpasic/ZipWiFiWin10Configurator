using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.WiFi;

namespace ZipWiFiWin10Configurator
{
    class ZipNetworkConnection
    {
        public WiFiAvailableNetwork WiFiNetwork { get; set; }
        public String ConnectionStatus { get; set; }
        public String Color { get; set; }
        public String Ssid { get { return WiFiNetwork.Ssid; } }
    }
}
