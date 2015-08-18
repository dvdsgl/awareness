using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using Cadenza;

using Awareness.Agnostic;

namespace Awareness
{
	class AwarenessNotifyIconController
	{
		public event EventHandler DoubleClick;

		readonly NotifyIcon Icon;
		readonly AbstractPlatform Platform;
		readonly AwarenessController Controller;

		MenuItem TimeDisplay;

		public AwarenessNotifyIconController (AbstractPlatform platform, AwarenessController controller)
		{
			Platform = platform;
			Platform.ApplicationWillQuit += ApplicationWillQuit;

			Controller = controller;
			Controller.BreakTimer.BreakChecked += BreakChecked;
			Controller.BreakTimer.BreakSuggested += BreakSuggested;

			Icon = new NotifyIcon {
				Visible = true,
				ContextMenu = BuildContextMenu (),
				Icon = new Icon (Platform.ResourceNamed ("bowl.ico"))
			};

			Icon.DoubleClick += (sender, args) => DoubleClick.Raise (this);
			Icon.BalloonTipClicked += BalloonTipClicked;
		}

		void BalloonTipClicked (object sender, EventArgs e)
		{
			Platform.OpenUrl (Constants.BreakIdeasUrl);
		}

		void BreakSuggested (object sender, EventArgs e)
		{
			var title = string.Format ("It's been {0} since your last break.", Controller.BreakTimer.ActivityTime.ToHoursAndMinutesLong ());
			var message =  string.Format ("How about a {0}-minute break? Click here for break ideas!", (int)Platform.Preferences.MinBreak.TotalMinutes);
			Icon.ShowBalloonTip ((int) 10.Seconds ().TotalMilliseconds, title, message, ToolTipIcon.Info);
		}

		void BreakChecked (object sender, EventArgs e)
		{
			TimeDisplay.Text = Controller.BreakTimer.ActivityTime.ToHoursAndMinutesLong () + " since last break";
			Icon.Text = Controller.BreakTimer.ActivityTime.ToHoursAndMinutes () + " | Awareness";
		}

		void ApplicationWillQuit (object sender, EventArgs e)
		{
			Icon.Visible = false;
		}

		ContextMenu BuildContextMenu ()
		{
			var menu = new ContextMenu (BuildMenuItems ().ToArray ());
			return menu;
		}

		IEnumerable<MenuItem> BuildMenuItems ()
		{
			var main = new MenuItem ("Awareness") { DefaultItem = true };
			main.Click += delegate { DoubleClick.Raise (this); };
			yield return main;

			yield return new MenuItem ("-");

			yield return TimeDisplay = new MenuItem () { Enabled = false };

			yield return MenuItemWithClickAction ("Break Ideas...", () => {
				Platform.OpenUrl (Constants.BreakIdeasUrl);
			});

			yield return new MenuItem ("-");

			yield return MenuItemWithClickAction ("Awareness Website", () => {
				Platform.OpenUrl (Constants.AwarenessWebsiteUrl);
			});

			yield return MenuItemWithClickAction ("Facebook Page", () => {
				Platform.OpenUrl (Constants.AwarenessFacebookPageUrl);
			});

			yield return MenuItemWithClickAction ("Support", () => {
				Platform.OpenUrl (Constants.AwarenessSupportUrl);
			});

			yield return new MenuItem ("-");

			yield return MenuItemWithClickAction ("Exit", () => {
				Application.Exit ();
			});
		}

		MenuItem MenuItemWithClickAction (string text, Action act)
		{
			var item = new MenuItem (text);
			item.Click += (sender, args) => act ();
			return item;
		}
	}
}
