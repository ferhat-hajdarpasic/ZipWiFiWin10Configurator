using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZipWiFiWin10Configurator
{
    public abstract class ZipCommand
    {
        public abstract byte[] GetBytes();
    }
}
