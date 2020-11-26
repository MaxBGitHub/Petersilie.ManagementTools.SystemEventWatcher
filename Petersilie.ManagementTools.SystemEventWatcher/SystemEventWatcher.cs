using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32;

namespace Petersilie.ManagementTools.SystemEventWatcher
{
    class main_cs
    {
        static void EventConsumed(object sender, MessagePumpEventArgs e)
        {
            Console.WriteLine(e.Source + " - " + e.Action);
        }

        static void Main(string[] args)
        {
            var sysWatcher = new SystemEventWatcher(EventTypeFlag.All);
            sysWatcher.Start();
            while (true) { }
        }
    }


    public class SystemEventWatcher : IDisposable
    {
        private EventTypeFlag       _eventFlags;
        private MessagePumpProvider _messagePumpProvider;
        private Thread              _messagePumpThread;
        private AsyncWriter         _logWriter;


        private event EventHandler<MessagePumpEventArgs> onSystemEventConsumed;
        public event EventHandler<MessagePumpEventArgs> SystemEventConsumed
        {
            add {
                onSystemEventConsumed += value;
            }
            remove {
                onSystemEventConsumed -= value;
            }
        }


        private void WriteEntry(MessagePumpEventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ms"));
            sb.Append($"\t{Environment.MachineName}");
            sb.Append($"\t{e.Source} {e.Action}\n");
            _logWriter.Write(sb.ToString());
        }


        private void PumpRecieved(object sender, MessagePumpEventArgs e)
        {
            WriteEntry(e);
            onSystemEventConsumed?.Invoke(this, e);
        }


        public void Stop()
        {
            if (null != _messagePumpThread) {
                _messagePumpThread.Join(500);
                if (_messagePumpThread.IsAlive) {
                    _messagePumpThread.Abort();
                }
                _messagePumpThread = null;
            }

            if (null != _messagePumpProvider) {
                _messagePumpProvider.Close();
                _messagePumpProvider.Dispose();
                _messagePumpProvider = null;
            }            
        }

        public void Start()
        {
            _messagePumpThread = new Thread(new ThreadStart(InitMessagePump));
            _messagePumpThread.Start();
        }

        private void InitMessagePump()
        {
            _messagePumpProvider = new MessagePumpProvider(_eventFlags);
            _messagePumpProvider.SystemEventConsumed += this.PumpRecieved;
            Application.Run(_messagePumpProvider);
        }


        private Guid guid = Guid.NewGuid();

        public SystemEventWatcher(EventTypeFlag flags)
        {
            _eventFlags = flags;
            _logWriter = new AsyncWriter("c:\\users\\max.bader\\desktop\\logtest.log");
        }


        ~SystemEventWatcher() { Dispose(false); }
        public void Dispose() { Dispose(true); }
        private void Dispose(bool disposing)
        {            
            if (disposing) {
                GC.SuppressFinalize(this);
            }
            Stop();
        }
    }
}
