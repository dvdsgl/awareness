using System;
using System.Drawing;
using System.Windows.Forms;

using Cadenza;

using Awareness.Agnostic;

namespace Awareness
{
	public partial class AwarenessForm : Form
	{
		readonly AbstractPlatform Platform;
		readonly AwarenessController Controller;

		readonly AwarenessNotifyIconController NotifyIconController;

		public AwarenessForm ()
		{
			InitializeComponent ();

			Platform = new WindowsPlatform (this);

			BackgroundImage = Image.FromFile (Platform.ResourceNamed ("splash.jpg"));
			Height = BackgroundImage.Height + 35;
			Width = BackgroundImage.Width + 12;

			Controller = new AwarenessController (Platform);
			Controller.BreakTimer.BreakChecked += BreakChecked;

			NotifyIconController = new AwarenessNotifyIconController (Platform, Controller);
			NotifyIconController.DoubleClick += NotifyIcon_DoubleClicked;

			// Want to get form *not* to show, but best we can do is minimize.
			if (!Platform.Preferences.IsFirstRun) {
				WindowState = FormWindowState.Minimized;
			}
		}

		void NotifyIcon_DoubleClicked (object sender, EventArgs e)
		{
			WindowState = FormWindowState.Normal;
			Show ();
		}

		void BreakChecked (object sender, EventArgs e)
		{
			Text = Controller.BreakTimer.ActivityTime.ToHoursAndMinutes () + " | Awareness";
		}

		void AwarenessForm_FormClosing (object sender, FormClosingEventArgs e)
		{
			if (e.CloseReason == CloseReason.UserClosing) {
				e.Cancel = true;
				Hide ();
			}
		}
	}
}
