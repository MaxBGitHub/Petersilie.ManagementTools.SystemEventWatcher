using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;

namespace Petersilie.ManagementTools.SystemEventWatcher
{
    internal class AsyncWriter
    {
        public string FileName { get; }

        private static ReaderWriterLock _writeLock = new ReaderWriterLock();

        private async Task WriteInternal(string content)
        {
            FileStream asyncStream = null;
            try
            {                
                asyncStream = new FileStream(FileName, 
                                            FileMode.Append, 
                                            FileAccess.Write, 
                                            FileShare.Read, 
                                            0x1000, 
                                            true);

                byte[] buffer = Encoding.Unicode.GetBytes(content);
                await asyncStream.WriteAsync(buffer, 0, buffer.Length);
            }
            finally {                
                if (null != asyncStream) {
                    asyncStream.Dispose();
                }                
            }
        }


        private void ThreadWrite(string content)
        {
            try {
                _writeLock.AcquireWriterLock(2000);
            } catch {
                return;
            }

            try {
                WriteInternal(content).Wait();                
            } finally {
                _writeLock.ReleaseWriterLock();
            }
        }


        public void Write(string content)
        {
            Thread writeThread = new Thread(
                () => ThreadWrite(content));

            writeThread.SetApartmentState(ApartmentState.MTA);
            writeThread.IsBackground = true;
            writeThread.Start();
        }

        public AsyncWriter(string fileName)
        {
            FileName = fileName;
        }
    }
}
