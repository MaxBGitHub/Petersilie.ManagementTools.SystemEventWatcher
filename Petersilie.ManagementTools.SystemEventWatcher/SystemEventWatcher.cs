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
            string appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string file = "sysevent.watcher";
            string filename = System.IO.Path.Combine(appdata, file);
            _logWriter = new AsyncWriter(filename);
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
