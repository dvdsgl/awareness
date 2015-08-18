using System;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using Microsoft.Win32;

using Cadenza;
using Cadenza.Collections;

using Awareness.Agnostic;

namespace Awareness
{
	class WindowsPlatform : AbstractPlatform
	{
		#region Win32 Interop
		[DllImport ("User32.dll")]
		private static extern bool GetLastInputInfo (ref LASTINPUTINFO plii);

		internal struct LASTINPUTINFO
		{
			public uint cbSize;
			public uint dwTime;
		}
		#endregion

		static TimeSpan GetTimeSinceInput ()
		{
			var info = new LASTINPUTINFO ();
			info.cbSize = (uint) Marshal.SizeOf (info);

			if (!GetLastInputInfo (ref info))
				throw new Exception ("Could not GetTimeSinceInput");

			return TimeSpan.FromMilliseconds (info.dwTime);
		}

		Form MainForm;
		IClock clock = new RealClock ();

		public override event EventHandler ApplicationLaunched, ApplicationWillQuit, SystemResumed;

		public override IClock Clock
		{
			get { return clock; }
		}

		public WindowsPlatform (Form form)
			: base (new WindowsPreferences ())
		{
			MainForm = form;
			MainForm.Load += delegate {
				ApplicationLaunched.Raise (this);
			};
			Application.ApplicationExit += delegate {
				ApplicationWillQuit.Raise (this);
			};

			SystemEvents.PowerModeChanged += PowerModeChanged;
		}

		void PowerModeChanged (object sender, PowerModeChangedEventArgs e)
		{
			switch (e.Mode) {
				case PowerModes.Resume:
					SystemResumed.Raise (this);
					break;
			}
		}

		public override DateTime LastActivity
		{
			get
			{
				var timeSinceInput = TimeSpan.FromMilliseconds (Environment.TickCount) - GetTimeSinceInput ();
				return clock.Now - timeSinceInput;
			}
		}

		protected override String ResourcesDirectory
		{
			get
			{
#if DEBUG
				var top = GetType ().Assembly.Location;
				5.Times ().ForEach (i => top = Path.GetDirectoryName (top));
				return Path.Combine (top, "Resources");
#else
                return Registry.CurrentUser.CreateSubKey("Software\\Futureproof\\Awareness").GetValue("Install_Dir") as string;
#endif
			}
		}

		public override void RunOnMainThread (Action act)
		{
			try {
				MainForm.Invoke (act);
			} catch {
				// We are getting strange exceptions here, when RunOnMainThread is
				// called after the main form has closed. In this case, we just try
				// to continue quitting by aborting the current thread.
				Thread.CurrentThread.Abort ();
			}
		}

		public override IThreadEvent GetThreadEvent ()
		{
			return new RealThreadEvent ();
		}

		public override void ThreadSleep (TimeSpan duration)
		{
			Thread.Sleep (duration);
		}

		public override Thread CreateWorkerThread (Action act)
		{
			return new Thread (() => act ()) { IsBackground = true };
		}

		public override AbstractBowlSound CreateBowlSound (string soundPath)
		{
			return new BowlSound (soundPath);
		}

		public override void OpenUrl (string url)
		{
			Process.Start (url);
		}
	}
}
