using System;

namespace Awareness
{
	public partial class ActivityMonitor
	{	
		public static TimeSpan SystemIdleTime {
			get {
				return TimeSpan.FromSeconds (GetSystemIdleTime ());
			}
		}
	}
}

