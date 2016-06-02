using System;
using System.Linq;
using System.Security;
using System.Text;

namespace ZipWiFiWin10Configurator
{
    public class ZipConfigureWiFiCommand : ZipCommand
    {
        public int SecurityType { get; set; }
        public string Domain { get; set; }
        public string Password { get; set; }
        public string ProxyHost { get; set; }
        public string ProxyPort { get; set; }
        public string ProxyUsername { get; set; }
        public string ProxyPassword { get; set; }

        public override byte[] GetBytes()
        {
            string tempPayload = SecurityType + "\0" + Domain.Trim() + "\0" + Password.ToString().Trim();
            if(!String.IsNullOrWhiteSpace(ProxyHost) && !String.IsNullOrWhiteSpace(ProxyPort))
            {
                tempPayload = "\0" + tempPayload + ProxyHost.Trim() + "\0" + ProxyPort.Trim();
                if (!String.IsNullOrWhiteSpace(ProxyUsername) && !String.IsNullOrWhiteSpace(ProxyPassword))
                {
                    tempPayload = "\0" + tempPayload + ProxyUsername.Trim() + "\0" + ProxyPassword.Trim();
                }
            }
            byte[] payload = Encoding.ASCII.GetBytes(tempPayload);
            var msg = new byte[] { (byte)'h', (byte)(2 + payload.Length) }.ToArray().Concat(payload);
            byte[] bytes = msg.ToArray();
            return bytes;
        }
    }
}
