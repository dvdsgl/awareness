using System;
using System.Text;

namespace Awareness.Agnostic
{
	public static class TimeSpanExtensions
	{
		public static TimeSpan Min (this TimeSpan self, TimeSpan other)
		{
			return self < other ? self : other;
		}
		
		public static TimeSpan Max (this TimeSpan self, TimeSpan other)
		{
			return self > other ? self : other;
		}
		
        public static string ToShortTimeString (this TimeSpan self)
        {
            string hours   = ((int) self.TotalHours).ToString ().PadLeft (2, '0'),
                   minutes = self.Minutes.ToString ().PadLeft (2, '0'),
                   seconds = self.Seconds.ToString ().PadLeft (2, '0');
            
            // TASK: Figure out how to use string.Format to pad with zeros.
            return string.Format ("{0}:{1}:{2}", hours, minutes, seconds);
        }
        
		/// <summary>
		/// Returns a clock representation (e.g. "32m" or "1h 10m").
		/// </summary>
		/// <param name="self">
		/// A <see cref="TimeSpan"/>
		/// </param>
		/// <returns>
		/// A <see cref="System.String"/>
		/// </returns>
		public static string ToHoursAndMinutes (this TimeSpan self)
		{
			var builder = new StringBuilder ();
            
            if (0 < self.Hours)
                builder.AppendFormat ("{0}h", self.Hours);
            if (0 < self.Hours && 0 < self.Minutes)
                builder.Append (" ");
            if (0 == self.Hours || self.Minutes != 0)
                builder.AppendFormat ("{0}m", self.Minutes);
            
            return builder.ToString ();
		}
		
		public static string ToHoursAndMinutesLong (this TimeSpan self)
		{
            var builder = new StringBuilder ();
            
            if (0 < self.Hours)
                builder.AppendFormat (Pluralize (self.Hours, "{0} hour", "{0} hours"), self.Hours);
            if (0 < self.Hours && 0 < self.Minutes)
                builder.Append (" and ");
            if (0 == self.Hours || self.Minutes != 0)
                builder.AppendFormat (Pluralize (self.Minutes, "{0} minute", "{0} minutes"), self.Minutes);
            
            return builder.ToString ();
		}
        
        // TASK: consider adding to int (e.g. n.Many ("song", "songs"))
        static string Pluralize (int n, string singular, string plural)
        {
            return n == 1 ? singular : plural;
        }
	}
}

