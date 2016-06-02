using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ZipWiFiWin10Configurator
{
    public delegate void ZipResponseReceivedHandler(object sender, ZipResponseReceivedEventArgs e);
    public class ZipResponseReceivedEventArgs : EventArgs
    {
        public ZipResponse Response { get; set; }
        public DateTime TimeResponseReceived { get; set; }
    }
    public class ZipMessageBus
    {
        private static ConcurrentQueue<ZipCommand> CommandQueue = new ConcurrentQueue<ZipCommand>();
        private Timer m_timer;

        public static ZipMessageBus SINGLETON = new ZipMessageBus();
        private ZipMessageBus()
        {
        }

        public event ZipResponseReceivedHandler ResponseReceived;

        public void QueueResponse(ZipResponse response)
        {
            NotifyResponseReceived(response);
        }

        private void NotifyResponseReceived(ZipResponse response)
        {
            if (this.ResponseReceived != null)
            {
                ZipResponseReceivedEventArgs eventargs = new ZipResponseReceivedEventArgs() { Response = response };
                this.ResponseReceived(this, eventargs);
            }
        }

        public static bool IsCommandQueued(Type commandType)
        {
            return CommandQueue.Any(s => s.GetType() == commandType);
        }

        public void QueueCommand(ZipCommand zipCommand)
        {
            CommandQueue.Enqueue(zipCommand);
        }

        public static ZipCommand PeekNextCommand()
        {
            ZipCommand tempCommand;
            CommandQueue.TryPeek(out tempCommand);
            return tempCommand;
        }

        public static ZipCommand DequeueNextCommand()
        {
            ZipCommand tempCommand;
            CommandQueue.TryDequeue(out tempCommand);
            return tempCommand;
        }
    }
}

