using System;

namespace Awareness.Agnostic
{
    public static class Log
    {
        
		const Level CurrentLevel = Level.Verbose;
        
        public enum Level {
            Silent,
            Error,
            Debug,
            Info,
            Verbose
        }
        
        public static void Error (string format, params object[] args)
        {
            WriteLog (Level.Error, format, args);
        }
        
        public static void Error (object o)
        {
            Error (o.ToString ());
        }
        
        public static void Error (Exception e)
        {
            Error ("{0}: {1}\n{2}", e.GetType ().Name, e.Message, e.StackTrace);
        }
        
        public static void Debug (string format, params object[] args)
        {
            WriteLog (Level.Debug, format, args);
        }
        
        public static void Debug (object o)
        {
            Debug (o.ToString ());
        }
        
        public static void Info (string format, params object[] args)
        {
            WriteLog (Level.Info, format, args);
        }
        
        public static void Info (object o)
        {
            Info (o.ToString ());
        }
        
        static void WriteLog (Level level, string format, params object[] args)
        {
            if (CurrentLevel < level) return;
            
            var levelName = Enum.GetName (typeof (Level), level);
            var message = string.Format (format, args);
            Console.WriteLine ("[{0} {1}] {2}", levelName, DateTime.Now.ToLongTimeString (), message);
        }
    }
}

