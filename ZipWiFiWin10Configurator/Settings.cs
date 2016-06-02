using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace ZipWiFiWin10Configurator
{
    class Settings
    {
        private void storeString(String name, String value)
        {
            ApplicationData.Current.LocalSettings.Values[name] = value;
        }
        private String retrieveString(String name)
        {
            return (String)ApplicationData.Current.LocalSettings.Values[name];
        }
        private string retrieveString(string name, string defaultValue)
        {
            if (!ApplicationData.Current.LocalSettings.Values.ContainsKey(name))
            {
                storeString(name, defaultValue);
            }
            return retrieveString(name);
        }

        private void store(String name, Object value)
        {
            ApplicationData.Current.LocalSettings.Values[name] = value;
        }

        private Object retrieve(String name, String defaultValue)
        {
            if (!ApplicationData.Current.LocalSettings.Values.ContainsKey(name))
            {
                store(name, defaultValue);
            }
            return ApplicationData.Current.LocalSettings.Values[name];
        }
        private int retrieveInt(String name, int defaultValue)
        {
            if (!ApplicationData.Current.LocalSettings.Values.ContainsKey(name))
            {
                store(name, defaultValue);
            }
            return (int)ApplicationData.Current.LocalSettings.Values[name];
        }

        private int retrieveInt(String name)
        {
            return (int)ApplicationData.Current.LocalSettings.Values[name];
        }

        public String Hostname
        {
            get { return (String)retrieveString("hostname", "192.168.1.1"); }
            set { storeString("hostname", value); }
        }
        public int Port {
            get {return retrieveInt("port", 9001);}
            set{store("port", value);}
        }
        public  String WiFiDomainPrefix {
            get { return retrieveString("WiFiDomainPrefix", "ZIP-"); }
            set { storeString("WiFiDomainPrefix", value);}
        }
        public String WiFiDomainPassword
        {
            get { return retrieveString("WiFiDomainPassword", "12345678"); }
            set { storeString("WiFiDomainPassword", value); }
        }

        public String ConfigDomain {
            get { return retrieveString("ConfigDomain", "ZIP-"); }
            set { storeString("ConfigDomain", value); }
        }
        public  String ConfigPassword {
            get { return retrieveString("ConfigPassword", null); }
            set { storeString("ConfigPassword", value); }
        }
        public String ProxyHost {
            get { return retrieveString("ProxyHost"); }
            set { storeString("ProxyHost", value); }
        }
        public String ProxyPort {
            get { return retrieveString("ProxyPort"); }
            set { storeString("ProxyPort", value); }
        }
        public  String ProxyUsername {
            get { return retrieveString("ProxyUsername", null); }
            set { storeString("ProxyUsername", value); }
        }
        public  String ProxyPassword {
            get { return retrieveString("ProxyPassword", null); }
            set { storeString("ProxyPassword", value); }
        }
    }
}
