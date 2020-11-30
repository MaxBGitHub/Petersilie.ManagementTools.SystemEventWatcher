using Microsoft.Win32;
using System;
using System.Threading;
using System.Windows.Forms;

namespace Petersilie.ManagementTools.SystemEventWatcher
{
    /// <summary>
    /// Listens for system events.
    /// </summary>
    public class SystemEventWatcher : IDisposable
    {
        private MessagePumpProvider _msgPump; // Hidden window for WndProc.
        private Thread _msgPumpThread; // Thread running hidden window.


        /// <summary>
        /// Occurs when a user preference is changing.
        /// </summary>
        public event EventHandler<UserPreferenceChangingEventArgs> UserPreferenceChanging
        {
            add {
                _msgPump.OnUserPreferenceChanging += value;
            }
            remove {
                _msgPump.OnUserPreferenceChanging -= value;
            }
        }

        /// <summary>
        /// Occurs when a user preference is changed.
        /// </summary>
        public event EventHandler<UserPreferenceChangedEventArgs> UserPreferenceChanged
        {
            add {
                _msgPump.OnUserPreferencedChanged += value;
            }
            remove {
                _msgPump.OnUserPreferencedChanged -= value;
            }
        }

        /// <summary>
        /// Occurs when the user changes the time on the system clock.
        /// </summary>
        public event EventHandler<EventArgs> TimeChanged
        {
            add {
                _msgPump.OnTimeChanged += value;
            }
            remove {
                _msgPump.OnTimeChanged -= value;
            }
        }

        /// <summary>
        /// Occurs when a windows timer interval has expired.
        /// </summary>
        public event EventHandler<TimerElapsedEventArgs> TimerElapsed
        {
            add {
                _msgPump.OnTimerElapsed += value;
            }
            remove {
                _msgPump.OnTimerElapsed -= value;
            }
        }

        /// <summary>
        /// Occurs when the user is logging off or shutting down the system.
        /// </summary>
        public event EventHandler<SessionEndedEventArgs> SessionEnded
        {
            add {
                _msgPump.OnSessionEnded += value;
            }
            remove {
                _msgPump.OnSessionEnded -= value;
            }
        }

        /// <summary>
        /// Occurs when the user is trying to logg of or shut down the system.
        /// </summary>
        public event EventHandler<SessionEndingEventArgs> SessionEnding
        {
            add {
                _msgPump.OnSessionEnding += value;                
            }
            remove {
                _msgPump.OnSessionEnding -= value;
            }
        }

        /// <summary>
        /// Occurs when the user suspends or resumes the system.
        /// </summary>
        public event EventHandler<PowerModeChangedEventArgs> PowerModeChanged
        {
            add {
                _msgPump.OnPowerModeChanged += value;
            }
            remove {
                _msgPump.OnPowerModeChanged -= value;
            }
        }

        /// <summary>
        /// Occurs when the user switches to an application 
        /// that uses a different palette.
        /// </summary>
        public event EventHandler<EventArgs> PaletteChanged
        {
            add {
                _msgPump.OnPalleteChanged += value;
            }
            remove {
                _msgPump.OnPalleteChanged -= value;
            }
        }

        /// <summary>
        /// Occurs when the user adds fonts to or 
        /// removes fonts from the system.
        /// </summary>
        public event EventHandler<EventArgs> InstalledFontsChanged
        {
            add {
                _msgPump.OnInstalledFontsChanged += value;
            }
            remove {
                _msgPump.OnInstalledFontsChanged -= value;
            }
        }

        /// <summary>
        /// Occurs before the thread that listens for system
        /// events is terminated.
        /// <para>
        /// Use this event to free the <see cref="SystemEventWatcher"/>
        /// and all its resources by using its Dispose() method.
        /// </para>
        /// </summary>
        public event EventHandler<EventArgs> EventsThreadShutdown
        {
            add {
                _msgPump.OnEventsThreadShutdown += value;
            }
            remove {
                _msgPump.OnEventsThreadShutdown -= value;
            }
        }

        /// <summary>
        /// Occurs when the display settings are changing.
        /// </summary>
        public event EventHandler<EventArgs> DisplaySettingsChanging
        {
            add {
                _msgPump.OnDisplaySettingsChanging += value;
            }
            remove {
                _msgPump.OnDisplaySettingsChanging -= value;
            }
        }

        /// <summary>
        /// Occurs when the user changes the display settings.
        /// </summary>
        public event EventHandler<EventArgs> DisplaySettingsChanged
        {
            add {
                _msgPump.OnDisplaySettingsChanged += value;
            }
            remove {
                _msgPump.OnDisplaySettingsChanged -= value;
            }
        }

        /// <summary>
        /// Stop the <see cref="SystemEventWatcher"/> and free 
        /// all resources used by it.
        /// </summary>
        public void Stop()
        {
            if (null != _msgPumpThread) {
                _msgPumpThread.Join(500);
                if (_msgPumpThread.IsAlive) {
                    _msgPumpThread.Abort();
                }
                _msgPumpThread = null;
            }

            if (null != _msgPump) {
                _msgPump.Close();
                _msgPump.Dispose();
                _msgPump = null;
            }            
        }

        /// <summary>
        /// Start listening for system events.
        /// </summary>
        public void Start()
        {
            _msgPumpThread = new Thread(new ThreadStart(InitMessagePump));
            _msgPumpThread.Start();
        }

        // Run the invisible MessagePumpProvider window.
        private void InitMessagePump()
        {
            Application.Run(_msgPump);
        }

        // Try to cleanup on application exit.
        private void Application_Exit(object sender, EventArgs e)
        {
            try {
                Stop();
            } catch { }
        }


        public SystemEventWatcher()
        {
            _msgPump = new MessagePumpProvider();
            Application.ApplicationExit += Application_Exit;
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
