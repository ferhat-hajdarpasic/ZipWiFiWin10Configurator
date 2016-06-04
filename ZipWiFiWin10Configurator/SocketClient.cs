using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ZipWiFiWin10Configurator
{
    class SocketClient
    {
        Socket _socket = null;

        // Signaling object used to notify when an asynchronous operation is completed
        static ManualResetEvent _clientDone = new ManualResetEvent(false);

        // Define a timeout in milliseconds for each asynchronous call. If a response is not received within this 
        // timeout period, the call is aborted.
        const int TIMEOUT_MILLISECONDS = 5000;

        // The maximum size of the data buffer to use with the asynchronous socket methods
        const int MAX_BUFFER_SIZE = 2048;

        public string Connect(string hostName, int portNumber)
        {
            string result = string.Empty;
            DnsEndPoint hostEntry = new DnsEndPoint(hostName, portNumber);
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            SocketAsyncEventArgs socketEventArg = new SocketAsyncEventArgs();
            socketEventArg.RemoteEndPoint = hostEntry;
            socketEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(delegate (object s, SocketAsyncEventArgs e)
            {
                result = e.SocketError.ToString();
                _clientDone.Set();
            });
            _clientDone.Reset();
            _socket.ConnectAsync(socketEventArg);
            _clientDone.WaitOne(TIMEOUT_MILLISECONDS);
            return result;
        }
        public Boolean Send(byte[] payload)
        {
            if (_socket != null)
            {
                SocketAsyncEventArgs socketEventArg = new SocketAsyncEventArgs();
                try
                {
                    socketEventArg.RemoteEndPoint = _socket.RemoteEndPoint;
                    socketEventArg.UserToken = null;
                    socketEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(delegate (object s, SocketAsyncEventArgs e)
                    {
                        String response = e.SocketError.ToString();
                        _clientDone.Set();
                    });
                    socketEventArg.SetBuffer(payload, 0, payload.Length);
                    _clientDone.Reset();
                    _socket.SendAsync(socketEventArg);
                    _clientDone.WaitOne(TIMEOUT_MILLISECONDS);
                    return true;
                } catch(SocketException e)
                {
                    ToastHelper.PopToast("Connection failed", e.Message , "Replace", "Toast1");
                }
            }
            else
            {
                ToastHelper.PopToast("Connection failed", "Socket is not initialized", "Replace", "Toast1");
            }
            return false;
        }
        public byte[] Receive()
        {
            string response = "Operation Timeout";
            if (_socket != null)
            {
                SocketAsyncEventArgs socketEventArg = new SocketAsyncEventArgs();
                socketEventArg.RemoteEndPoint = _socket.RemoteEndPoint;
                socketEventArg.SetBuffer(new Byte[MAX_BUFFER_SIZE], 0, MAX_BUFFER_SIZE);
                socketEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(delegate (object s, SocketAsyncEventArgs e)
                {
                    if (e.SocketError == SocketError.Success)
                    {
                        response = Encoding.UTF8.GetString(e.Buffer, e.Offset, e.BytesTransferred);
                        response = response.Trim('\0');
                    }
                    else
                    {
                        response = e.SocketError.ToString();
                    }

                    _clientDone.Set();
                });

                _clientDone.Reset();
                _socket.ReceiveAsync(socketEventArg);
                _clientDone.WaitOne(TIMEOUT_MILLISECONDS);
            }
            else
            {
                response = "Socket is not initialized";
            }
            return Encoding.UTF8.GetBytes(response);
        }
        public void Close()
        {
            if (_socket != null)
            {
                _socket.Dispose();
            }
        }
    }
}
