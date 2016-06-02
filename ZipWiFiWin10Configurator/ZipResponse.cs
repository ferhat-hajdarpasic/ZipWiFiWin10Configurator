using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZipWiFiWin10Configurator
{
    public abstract class ZipResponse
    {
        public abstract bool Parse(byte[] bytes, int length);
        public abstract Type GetRequestType();

        protected ZipResponse()
        {
            IsValid = true;
        }
        public bool IsValid { get; set; }

        public virtual int ProtocolHeaderLength()
        {
            return 2; //For most commands
        }
    }
}
