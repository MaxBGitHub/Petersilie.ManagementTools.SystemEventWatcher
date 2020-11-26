using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Petersilie.ManagementTools.SystemEventWatcher
{
    public class MessagePumpEventArgs : EventArgs
    {
        public EventTypeFlag Source { get; }
        public EventAction Action { get; }
        public object SystemEventArgs { get; }
        public Type SystemEventType { get; }

        public MessagePumpEventArgs(object         sysEventArgs, 
                            Type            sysEventType, 
                            EventTypeFlag   source, 
                            EventAction     action)
        {
            SystemEventArgs = sysEventArgs;
            SystemEventType = sysEventType;
            Source = source;
            Action = action;
        }
    }

    interface ITest<T>
    {

    }
}
