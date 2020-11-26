using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Petersilie.ManagementTools.SystemEventWatcher
{
    [Flags] public enum EventTypeFlag
    {
        DisplaySetting = 1 << 1,
        Font    = 1 << 2,
        Palette = 1 << 3,
        Power   = 1 << 4,
        Session = 1 << 5,
        Time    = 1 << 6,
        UserPreference = 1 << 7,
        All     = DisplaySetting | 
                    Font | 
                    Palette | 
                    Power | 
                    Session | 
                    Time | 
                    UserPreference,
    }

    public enum EventAction {
        None,
        Changing,
        Changed,
        Elapsed,
        Switch,
        Ending,
        Ended,
        Unknown,
    }
}
