using System;
using System.Threading;
using Windows.UI.Xaml;
using System.Collections.Concurrent;

namespace ZipWiFiWin10Configurator
{
    public class TcpConnTimer
    {
        private DispatcherTimer m_timer = new DispatcherTimer();
        private static ConcurrentQueue<ZipCommand> CommandQueue = new ConcurrentQueue<ZipCommand>();

        public TimeSpan Interval
        {
            get { return m_timer.Interval; }
            set {m_timer.Interval = value;}
        }

        private void OnTimer(object sender, object e)
        {
            SendNextCommand();
        }


        public TcpConnTimer(int interval)
        {
            AutoResetEvent autoEvent = new AutoResetEvent(true);
            m_timer.Interval = new TimeSpan(0, 0, 0, 0, interval);
            m_timer.Tick += OnTimer;
            m_timer.Start();
        }

        protected virtual void SendNextCommand()
        {
            var tempCommand = GetNextCommand();
            if (tempCommand != null)
            {
                SendCommand(tempCommand);
            }
        }

        protected virtual ZipCommand GetNextCommand()
        {
                return ZipMessageBus.DequeueNextCommand();
        }

        protected virtual void SendCommand(ZipCommand command)
        {
            SocketClient socketClient = new SocketClient();

            try
            {
                String status = socketClient.Connect(new Settings().Hostname, new Settings().Port);
                if (status == "Success")
                {
                    byte[] payload = command.GetBytes();
                    Boolean success = socketClient.Send(payload);
                    if (success)
                    {
                        byte[] bytes = socketClient.Receive();
                        ZipResponse response = (new DataReceiver()).Handle(bytes);
                        OnCommandResponseReceived(response);
                    }
                } else
                {
                    ToastHelper.PopToast("Failed to connect", new Settings().Hostname + " on port " + new Settings().Port, "Replace", "Toast1");
                    OnCommandResponseReceived(new ZipConfigurWiFiResponse { IsValid = false});
                }
            } finally
            {
                socketClient.Close();
            }
        }

        public void OnCommandResponseReceived(ZipResponse response)
        {
            ZipMessageBus.SINGLETON.QueueResponse(response);
        }

        public void QueueResponse(ZipResponse response)
        {
            NotifyResponseReceived(response);
        }

        public event ZipResponseReceivedHandler ResponseReceived;
        private void NotifyResponseReceived(ZipResponse response)
        {
            if (this.ResponseReceived != null)
            {
                ZipResponseReceivedEventArgs eventargs = new ZipResponseReceivedEventArgs() { Response = response };
                this.ResponseReceived(this, eventargs);
            }
        }
    }
}
