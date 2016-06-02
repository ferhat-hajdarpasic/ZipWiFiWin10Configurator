using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.WiFi;

namespace ZipWiFiWin10Configurator
{
    class ZipNetworkConnection : INotifyPropertyChanged
    {
        private WiFiAvailableNetwork _WiFiNetwork;
        private String _ConnectionStatus;
        private String _Color;
        public WiFiAvailableNetwork WiFiNetwork { get { return _WiFiNetwork; } set {
                _WiFiNetwork = value;
                NotifyPropertyChanged("WiFiNetwork");
            }
        }
        public String ConnectionStatus { get { return _ConnectionStatus; } set {
                _ConnectionStatus = value;
                NotifyPropertyChanged("ConnectionStatus");
            }
        }
        public String Color { get { return _Color; } set {
                _Color = value;
                NotifyPropertyChanged("Color");
            }
        }
        public String Ssid { get { return _WiFiNetwork.Ssid; } }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
