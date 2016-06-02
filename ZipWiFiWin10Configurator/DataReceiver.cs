using System.Linq;

namespace ZipWiFiWin10Configurator
{
    public class DataReceiver
    {
        protected ZipResponse HandleZipResponse(byte[] data)
        {
            ZipResponse response = null;
            if ((data[0] == 0x55) && (data[1] == 0xaa))
            {
            }
            else
            {
                uint packetLength = data[2];
                response = HandleSinglePacketResponse(data, packetLength);
            }
            return response;
        }

        public ZipResponse Handle(byte[] data)
        {
            ZipResponse response = null;

            if (data[0] >= 'a')
            {
                response = HandleNonZipResponse(data);
            }
            else
            {
                if (data[1] >= 2)
                {
                    response = HandleZipResponse(data);
                }
            }
            return response;
        }
        protected ZipResponse HandleNonZipResponse(byte[] data)
        {
            ZipResponse response = CreateNonZipResponse(data);
            if (response != null)
            {
                response.Parse(data.Skip(1).ToArray(), data.Length);
            }
            return response;
        }
        protected virtual ZipResponse HandleSinglePacketResponse(byte[] protocol, uint packetLength)
        {
            ZipResponse response = null;
            if (protocol.Length >= packetLength)
            {
                response = CreateZipResponse(protocol);
                if (response != null)
                {
                    int length = protocol[1];
                    int protocolHeaderLength = response.ProtocolHeaderLength();
                    byte[] protocolData = protocol.Skip(protocolHeaderLength).ToArray();
                    response.Parse(protocolData, length - protocolHeaderLength);
                }
            }
            return response;
        }

        private static ZipResponse CreateZipResponse(byte[] protocol)
        {
            ZipResponse response = null;
            char cmd = (char)protocol[0];
            int length = protocol[1];

            switch (cmd)
            {
                case 'A':
                    break;
                case 'B':
                    break;
                case 'C':
                    break;
                case 'D':
                    break;
                case 'E':
                    break;
                case 'F':
                    break;
                case 'I':
                    break;
                case 'L':
                    break;
                case 'M':
                    break;
                case 'N':
                    break;
                case 'S':
                    break;
                case 'T':
                    break;
                case 'O':
                    break;
                case 'U':
                    break;
                case 'V':
                    break;
                case 'b':
                    break;
                default:
                    break;
            }
            return response;
        }

        private static ZipResponse CreateNonZipResponse(byte[] data)
        {
            ZipResponse response = null;
            char cmd = (char)data[0];
            switch (cmd)
            {
                case 'e':
                    break;
                case 'f':
                    break;
                case 'h':
                    response = new ZipConfigurWiFiResponse();
                    break;
                case 's':
                    break;
                case 'u':
                    break;
                case 'w':
                    break;
                case 'o':
                    break;
                case 'r':
                    break;
                default:
                    break;
            }
            return response;
        }
    }
}
