using System;

namespace ZipWiFiWin10Configurator
{
    public class ZipConfigurWiFiResponse : ZipResponse
    {
        public override bool Parse(byte[] bytes, int length)
        {
            return true;
        }

        public override Type GetRequestType()
        {
            return typeof(ZipConfigureWiFiCommand);
        }
    }
}
