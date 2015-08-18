using System;
using System.IO;
using System.Threading;

using MonoMac.AppKit;
using MonoMac.Foundation;

using Cadenza;

using Awareness.Agnostic;

namespace Awareness
{
	public class MacPlatform : AbstractPlatform
	{
		
		public override event EventHandler ApplicationLaunched, ApplicationWillQuit, SystemResumed;
		
		IClock clock = new RealClock ();
				
		public MacPlatform () : base (new MacPreferences ())
		{
			var defaults = NSDictionary.FromFile (ResourceNamed ("UserDefaults.plist"));
			NSUserDefaults.StandardUserDefaults.RegisterDefaults (defaults);
			
			OnWorkspaceNotification ("NSWorkspaceDidWakeNotification", HandleNSWorkspaceDidWakeNotification);
			NSApplication.SharedApplication.DidFinishLaunching += HandleNSApplicationSharedApplicationDidFinishLaunching;
			NSApplication.SharedApplication.WillTerminate += HandleNSApplicationSharedApplicationWillTerminate;
		}

		void HandleNSApplicationSharedApplicationWillTerminate (object sender, EventArgs e)
		{
			ApplicationWillQuit.Raise (this);
		}
		
		void HandleNSWorkspaceDidWakeNotification ()
		{
			SystemResumed.Raise (this);
		}
		
		void HandleNSApplicationSharedApplicationDidFinishLaunching (object sender, EventArgs e)
		{
			ApplicationLaunched.Raise (this);
		}
		
		public MacPreferences MacPreferences {
			get { return (MacPreferences) Preferences; }
		}
		
		void OnWorkspaceNotification (string notification, Action act)
		{
			var center = NSWorkspace.SharedWorkspace.NotificationCenter;
			center.AddObserver ((NSString)notification, note => act ());
		}
		
		#region implemented abstract members of AbstractPlatform
		
		public override void ThreadSleep (TimeSpan duration)
		{
			Thread.Sleep (duration);
		}
		
		public override void RunOnMainThread (Action act)
		{
			NSThread.Current.InvokeOnMainThread (() => act ());
		}
		
		public override IThreadEvent GetThreadEvent ()
		{
			return new RealThreadEvent ();
		}
		
		public override IClock Clock {
			get { return clock; }
		}
		
		public override DateTime LastActivity {
			get {
				return Clock.Now - ActivityMonitor.SystemIdleTime;
			}
		}
		
		protected override string ResourcesDirectory {
			get {
				return Path.Combine (NSBundle.MainBundle.ResourcePath, "Resources");
			}
		}
		
		public override Thread CreateWorkerThread (Action act)
		{
			return new Thread (() => {
				using (var pool = new NSAutoreleasePool ())
					act ();
			});
		}
		
		public override AbstractBowlSound CreateBowlSound (string soundPath)
		{
			return new BowlSound (soundPath);
		}
		
		public override void OpenUrl (string url)
		{
			NSWorkspace.SharedWorkspace.OpenUrl (new NSUrl (url));
		}
		
		#endregion
	}
}

