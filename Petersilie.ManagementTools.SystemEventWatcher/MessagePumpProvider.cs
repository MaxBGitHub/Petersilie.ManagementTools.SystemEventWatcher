using Microsoft.Win32;
using System;
using System.Windows.Forms;

namespace Petersilie.ManagementTools.SystemEventWatcher
{
    /* !======================================!
    ** !==      IMPORTANT * PLEASE READ     ==!
    ** !======================================!
    ** In order to actually listen for any system event you need a
    ** window which hooks to the windows message pump (WndProc).
    ** All system events are static events which is why you must
    ** make sure to detach/remove all event handlers when you end
    ** the application or dispose it. If you do not detach the handlers
    ** you will get memory leaks! */
    internal partial class MessagePumpProvider : Form
    {
        internal event EventHandler<UserPreferenceChangingEventArgs> OnUserPreferenceChanging;
        internal event EventHandler<UserPreferenceChangedEventArgs> OnUserPreferencedChanged;
        internal event EventHandler<EventArgs> OnTimeChanged;
        internal event EventHandler<TimerElapsedEventArgs> OnTimerElapsed;
        internal event EventHandler<SessionEndedEventArgs> OnSessionEnded;
        internal event EventHandler<SessionEndingEventArgs> OnSessionEnding;
        internal event EventHandler<SessionSwitchEventArgs> OnSessionSwitching;
        internal event EventHandler<PowerModeChangedEventArgs> OnPowerModeChanged;
        internal event EventHandler<EventArgs> OnPalleteChanged;
        internal event EventHandler<EventArgs> OnInstalledFontsChanged;
        internal event EventHandler<EventArgs> OnDisplaySettingsChanging;
        internal event EventHandler<EventArgs> OnDisplaySettingsChanged;
        internal event EventHandler<EventArgs> OnEventsThreadShutdown;


        internal void SystemEvents_UserPreferenceChanging(object sender, UserPreferenceChangingEventArgs e) { OnUserPreferenceChanging?.Invoke(sender, e); }
        internal void SystemEvents_UserPreferenceChanged(object sender, UserPreferenceChangedEventArgs e) { OnUserPreferencedChanged?.Invoke(sender, e); }
        internal void SystemEvents_TimerElapsed(object sender, TimerElapsedEventArgs e) { OnTimerElapsed?.Invoke(sender, e); }
        internal void SystemEvents_TimeChanged(object sender, EventArgs e) { OnTimeChanged?.Invoke(sender, e); }
        internal void SystemEvents_SessionSwitch(object sender, SessionSwitchEventArgs e) { OnSessionSwitching?.Invoke(sender, e); }
        internal void SystemEvents_SessionEnding(object sender, SessionEndingEventArgs e) { OnSessionEnding?.Invoke(sender, e); }
        internal void SystemEvents_SessionEnded(object sender, SessionEndedEventArgs e) { OnSessionEnded?.Invoke(sender, e); }
        internal void SystemEvents_PowerModeChanged(object sender, PowerModeChangedEventArgs e) { OnPowerModeChanged?.Invoke(sender, e); }
        internal void SystemEvents_PaletteChanged(object sender, EventArgs e) { OnPalleteChanged?.Invoke(sender, e); }
        internal void SystemEvents_InstalledFontsChanged(object sender, EventArgs e) { OnInstalledFontsChanged?.Invoke(sender, e); }        
        internal void SystemEvents_DisplaySettingsChanging(object sender, EventArgs e) { OnDisplaySettingsChanging?.Invoke(sender, e); }
        internal void SystemEvents_DisplaySettingsChanged(object sender, EventArgs e) { OnDisplaySettingsChanged?.Invoke(sender, e); }
        internal void SystemEvents_EventsThreadShutdown(object sender, EventArgs e) { OnEventsThreadShutdown?.Invoke(sender, e); }


        public void ApplyEvents()
        {
            SystemEvents.UserPreferenceChanged += SystemEvents_UserPreferenceChanged;
            SystemEvents.UserPreferenceChanging += SystemEvents_UserPreferenceChanging;
            SystemEvents.TimeChanged += SystemEvents_TimeChanged;
            SystemEvents.TimerElapsed += SystemEvents_TimerElapsed;
            SystemEvents.SessionEnded += SystemEvents_SessionEnded;
            SystemEvents.SessionEnding += SystemEvents_SessionEnding;
            SystemEvents.SessionSwitch += SystemEvents_SessionSwitch;
            SystemEvents.PowerModeChanged += SystemEvents_PowerModeChanged;
            SystemEvents.PaletteChanged += SystemEvents_PaletteChanged;
            SystemEvents.InstalledFontsChanged += SystemEvents_InstalledFontsChanged;
            SystemEvents.DisplaySettingsChanged += SystemEvents_DisplaySettingsChanged;
            SystemEvents.DisplaySettingsChanging += SystemEvents_DisplaySettingsChanging;
            SystemEvents.EventsThreadShutdown += SystemEvents_EventsThreadShutdown;
        }


        public void RemoveEvents()
        {
            SystemEvents.UserPreferenceChanged -= SystemEvents_UserPreferenceChanged;
            SystemEvents.UserPreferenceChanging -= SystemEvents_UserPreferenceChanging;
            SystemEvents.TimeChanged -= SystemEvents_TimeChanged;
            SystemEvents.TimerElapsed -= SystemEvents_TimerElapsed;
            SystemEvents.SessionEnded -= SystemEvents_SessionEnded;
            SystemEvents.SessionEnding -= SystemEvents_SessionEnding;
            SystemEvents.SessionSwitch -= SystemEvents_SessionSwitch;
            SystemEvents.PowerModeChanged -= SystemEvents_PowerModeChanged;
            SystemEvents.PaletteChanged -= SystemEvents_PaletteChanged;
            SystemEvents.InstalledFontsChanged -= SystemEvents_InstalledFontsChanged;
            SystemEvents.DisplaySettingsChanged -= SystemEvents_DisplaySettingsChanged;
            SystemEvents.DisplaySettingsChanging -= SystemEvents_DisplaySettingsChanging;
            SystemEvents.EventsThreadShutdown -= SystemEvents_EventsThreadShutdown;
        }


        internal MessagePumpProvider()
        {
            InitializeComponent();
        }

        /* Assigned in the custom InitializeComponent() method
        ** in the designer class of the MessagePumpProvider window. */
        private void MessagePump_Load(object sender, EventArgs e)
        {
            ApplyEvents();
        }

        /* Assigned in the custom InitializeComponent() method
        ** in the designer class of the MessagePumpProvider window. */
        private void MessagePump_FormClosing(object sender, FormClosingEventArgs e)
        {
            RemoveEvents();
        }        
    }


    // Designer of MessagePumpProvider.
    partial class MessagePumpProvider
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (null != components)) {
                components.Dispose();                
            }
            this.RemoveEvents();
            base.Dispose(disposing);
        }


        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(0, 0);
            this.FormBorderStyle = FormBorderStyle.None;
            this.Name = this.GetType().Name;
            this.Text = this.GetType().Name;
            this.WindowState = FormWindowState.Minimized;
            this.Load += new EventHandler(this.MessagePump_Load);
            this.FormClosing += new FormClosingEventHandler(this.MessagePump_FormClosing);
            this.ResumeLayout(false);
            this.ShowInTaskbar = false;
            this.ShowIcon = false;
        }
    }
}
