using System.Runtime.InteropServices;
using System.Timers;
using Alarus;
using Alarus.RealTimeFrameProviding;
using EventProcessorExample.Events;
using EventProcessorExample.Settings;

namespace EventProcessorExample
{
	[PluginGUIName("External events generating module")]
	[PluginHasSettings]
	[Guid("5C19153C-B100-431B-8164-CE8FD7E813FE")]
	public class TestEventProcessor : EventProcessor
	{
		private int _counter;
		private int _delay;
		private Timer _eventTimer;

		public override void Initialize(PluginSettings settings)
		{
			// Read common settings
			var isSettingsInitialized = settings.generalSettings != null;

			if (!isSettingsInitialized)
				return;

			var testEventProcessorSettings = (TestEventProcessorSettings)settings.generalSettings;
			_delay = testEventProcessorSettings.Delay;

			if (_delay <= 0)
				return;

			_eventTimer = new Timer(_delay);
			_eventTimer.Elapsed += OnTimedEvent;
			_eventTimer.AutoReset = true;
			_eventTimer.Enabled = true;
		}

		public override PluginSettings SetSettings(PluginSettings settings)
		{
			TestEventProcessorSettings testEventProcessorSettings;
			if (settings.generalSettings == null)
			{
				testEventProcessorSettings = new TestEventProcessorSettings();
				settings.generalSettings = testEventProcessorSettings;
			}
			else
				testEventProcessorSettings = settings.generalSettings as TestEventProcessorSettings;

			var settingsWindow = new SettingsWindow();

			settingsWindow.Delay.Text = testEventProcessorSettings.Delay.ToString();

			settingsWindow.Closed += (snd, args) =>
			{
				int delay;
				int.TryParse(settingsWindow.Delay.Text, out delay);
				testEventProcessorSettings.Delay = delay;
				settings.channelSpecificSettings = new object();
			};

			settingsWindow.ShowDialog();

			return settings;
		}

		public override void Dispose()
		{
			if (_eventTimer == null)
				return;

			_eventTimer.Elapsed -= OnTimedEvent;
			_eventTimer.Enabled = false;
			_eventTimer.Dispose();
		}

		private void OnTimedEvent(object sender, ElapsedEventArgs e)
		{
			var counterEvent = new CounterEvent(_counter);
			GenerateEvent(externalEvent: counterEvent, generateAlarm: false);
			_counter++;
		}
	}
}