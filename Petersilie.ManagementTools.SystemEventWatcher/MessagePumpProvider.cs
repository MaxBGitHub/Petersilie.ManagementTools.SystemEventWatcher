using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Win32;
using System.Windows.Forms;

namespace Petersilie.ManagementTools.SystemEventWatcher
{
    internal partial class MessagePumpProvider : Form
    {
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

        private void OnSystemEventConsumed(MessagePumpEventArgs e)
        {
            onSystemEventConsumed?.Invoke(this, e);
        }


        private EventTypeFlag _eventFlags;

        internal MessagePumpProvider(EventTypeFlag flag)
        {
            _eventFlags = flag;

            InitializeComponent();            
        }

        private void MessagePump_Load(object sender, EventArgs e)
        {
            ApplyEvents();
        }

        private void MessagePump_FormClosing(object sender, FormClosingEventArgs e)
        {
            RemoveEvents();
        }


        private void AddUserEventHandler()
        {
            SystemEvents.UserPreferenceChanged += SystemEvents_UserPreferenceChanged;
            SystemEvents.UserPreferenceChanging += SystemEvents_UserPreferenceChanging;
        }

        private void RemoveUserEventHandler()
        {
            SystemEvents.UserPreferenceChanged -= SystemEvents_UserPreferenceChanged;
            SystemEvents.UserPreferenceChanging -= SystemEvents_UserPreferenceChanging;
        }
        
        private void SystemEvents_UserPreferenceChanging(object sender, UserPreferenceChangingEventArgs e)
        {
            OnSystemEventConsumed(new MessagePumpEventArgs(e, typeof(UserPreferenceChangingEventArgs), 
                EventTypeFlag.UserPreference, EventAction.Changing));
        }

        private void SystemEvents_UserPreferenceChanged(object sender, UserPreferenceChangedEventArgs e)
        {
            OnSystemEventConsumed(new MessagePumpEventArgs(e, typeof(UserPreferenceChangedEventArgs), 
                EventTypeFlag.UserPreference, EventAction.Changed));
        }

        private void AddTimeEventHandler()
        {
            SystemEvents.TimeChanged += SystemEvents_TimeChanged;
            SystemEvents.TimerElapsed += SystemEvents_TimerElapsed;
        }

        private void RemoveTimeEventHandler()
        {
            SystemEvents.TimeChanged -= SystemEvents_TimeChanged;
            SystemEvents.TimerElapsed -= SystemEvents_TimerElapsed;
        }

        private void SystemEvents_TimerElapsed(object sender, TimerElapsedEventArgs e)
        {
            OnSystemEventConsumed(new MessagePumpEventArgs(e, typeof(TimerElapsedEventArgs), 
                EventTypeFlag.Time, EventAction.Elapsed));
        }

        private void SystemEvents_TimeChanged(object sender, EventArgs e)
        {
            OnSystemEventConsumed(new MessagePumpEventArgs(e, typeof(EventArgs), 
                EventTypeFlag.Time, EventAction.Changed));
        }

        private void AddSessionEventHandler()
        {
            SystemEvents.SessionEnded   += SystemEvents_SessionEnded;
            SystemEvents.SessionEnding  += SystemEvents_SessionEnding;
            SystemEvents.SessionSwitch  += SystemEvents_SessionSwitch;
        }

        private void RemoveSessionEventHandler()
        {
            SystemEvents.SessionEnded   -= SystemEvents_SessionEnded;
            SystemEvents.SessionEnding  -= SystemEvents_SessionEnding;
            SystemEvents.SessionSwitch  -= SystemEvents_SessionSwitch;
        }

        private void SystemEvents_SessionSwitch(object sender, SessionSwitchEventArgs e)
        {
            OnSystemEventConsumed(new MessagePumpEventArgs(e, typeof(SessionSwitchEventArgs), 
                EventTypeFlag.Session, EventAction.Switch));
        }

        private void SystemEvents_SessionEnding(object sender, SessionEndingEventArgs e)
        {
            OnSystemEventConsumed(new MessagePumpEventArgs(e, typeof(SessionEndingEventArgs), 
                EventTypeFlag.Session, EventAction.Ending));
        }

        private void SystemEvents_SessionEnded(object sender, SessionEndedEventArgs e)
        {
            OnSystemEventConsumed(new MessagePumpEventArgs(e, typeof(SessionEndedEventArgs), 
                EventTypeFlag.Session, EventAction.Ended));
        }

        private void AddPowerEventHandler()
        {
            SystemEvents.PowerModeChanged += SystemEvents_PowerModeChanged;
        }

        private void RemovePowerEventHandler()
        {
            SystemEvents.PowerModeChanged -= SystemEvents_PowerModeChanged;
        }

        private void SystemEvents_PowerModeChanged(object sender, PowerModeChangedEventArgs e)
        {
            OnSystemEventConsumed(new MessagePumpEventArgs(e, typeof(PowerModeChangedEventArgs), 
                EventTypeFlag.Power, EventAction.Changed));
        }

        private void AddPaletteEventHandler()
        {
            SystemEvents.PaletteChanged += SystemEvents_PaletteChanged;
        }

        private void RemovePaletteEventHandler()
        {
            SystemEvents.PaletteChanged -= SystemEvents_PaletteChanged;
        }

        private void SystemEvents_PaletteChanged(object sender, EventArgs e)
        {
            OnSystemEventConsumed(new MessagePumpEventArgs(e, typeof(EventArgs), 
                EventTypeFlag.Palette, EventAction.Changed));
        }

        private void AddFontEventHandler()
        {
            SystemEvents.InstalledFontsChanged += SystemEvents_InstalledFontsChanged;
        }

        private void RemoveFontEventHandler()
        {
            SystemEvents.InstalledFontsChanged -= SystemEvents_InstalledFontsChanged;
        }

        private void SystemEvents_InstalledFontsChanged(object sender, EventArgs e)
        {
            OnSystemEventConsumed(new MessagePumpEventArgs(e, typeof(EventArgs), 
                EventTypeFlag.Font, EventAction.Changed));
        }

        private void AddDisplayEventHandler()
        {
            SystemEvents.DisplaySettingsChanged += SystemEvents_DisplaySettingsChanged;
            SystemEvents.DisplaySettingsChanging += SystemEvents_DisplaySettingsChanging;
        }

        private void RemoveDisplayEventHandler()
        {
            SystemEvents.DisplaySettingsChanged -= SystemEvents_DisplaySettingsChanged;
            SystemEvents.DisplaySettingsChanging -= SystemEvents_DisplaySettingsChanging;
        }

        private void SystemEvents_DisplaySettingsChanging(object sender, EventArgs e)
        {
            OnSystemEventConsumed(new MessagePumpEventArgs(e, typeof(EventArgs),
                EventTypeFlag.DisplaySetting, EventAction.Changing));
        }

        private void SystemEvents_DisplaySettingsChanged(object sender, EventArgs e)
        {
            OnSystemEventConsumed(new MessagePumpEventArgs(e, typeof(EventArgs),
                EventTypeFlag.DisplaySetting, EventAction.Changed));
        }


        public void ApplyEvents()
        {
            if ((_eventFlags & EventTypeFlag.DisplaySetting) == EventTypeFlag.DisplaySetting) {
                AddDisplayEventHandler();
            }

            if ((_eventFlags & EventTypeFlag.Font) == EventTypeFlag.Font) {
                AddFontEventHandler();
            }

            if ((_eventFlags & EventTypeFlag.Palette) == EventTypeFlag.Palette) {
                AddPaletteEventHandler();
            }

            if ((_eventFlags & EventTypeFlag.Power) == EventTypeFlag.Power) {
                AddPowerEventHandler();
            }

            if ((_eventFlags & EventTypeFlag.Session) == EventTypeFlag.Session) {
                AddSessionEventHandler();
            }

            if ((_eventFlags & EventTypeFlag.Time) == EventTypeFlag.Time) {
                AddTimeEventHandler();
            }

            if ((_eventFlags & EventTypeFlag.UserPreference) == EventTypeFlag.UserPreference)
            {
                AddUserEventHandler();
            }
        }


        public void RemoveEvents()
        {
            if ((_eventFlags & EventTypeFlag.DisplaySetting) == EventTypeFlag.DisplaySetting) {
                RemoveDisplayEventHandler();
            }

            if ((_eventFlags & EventTypeFlag.Font) == EventTypeFlag.Font) {
                RemoveFontEventHandler();
            }

            if ((_eventFlags & EventTypeFlag.Palette) == EventTypeFlag.Palette) {
                RemovePaletteEventHandler();
            }

            if ((_eventFlags & EventTypeFlag.Power) == EventTypeFlag.Power) {
                RemovePowerEventHandler();
            }

            if ((_eventFlags & EventTypeFlag.Session) == EventTypeFlag.Session) {
                RemoveSessionEventHandler();
            }

            if ((_eventFlags & EventTypeFlag.Time) == EventTypeFlag.Time) {
                RemoveTimeEventHandler();
            }

            if ((_eventFlags & EventTypeFlag.UserPreference) == EventTypeFlag.UserPreference) {
                RemoveUserEventHandler();
            }
        }
    }


    partial class MessagePumpProvider
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (null != components)) {
                components.Dispose();
                this.RemoveEvents();
            }
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
