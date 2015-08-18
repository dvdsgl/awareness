using System;
using System.IO;
using System.Diagnostics;

using MonoMac.Foundation;
using MonoMac.AppKit;
using MonoMac.ObjCRuntime;

namespace Awareness
{
    public static class NSApplicationExtensions
    {
        
        /// <summary>
        /// Get the name of this application.
        /// </summary>
        /// <param name="self">
        /// A <see cref="NSApplication"/>
        /// </param>
        /// <returns>
        /// A <see cref="System.String"/>
        /// </returns>
        public static string GetName (this NSApplication self)
        {
            var bundle = NSBundle.MainBundle;
            var name   = bundle.InfoDictionary [(NSString) "CFBundleName"];
            return name.ToString ();
        }
        
        /// <summary>
        /// Make this application the active application.
        /// </summary>
        /// <param name="self">
        /// A <see cref="NSApplication"/>
        /// </param>
        public static void OrderFront (this NSApplication self)
        {
            NSWorkspace.SharedWorkspace.LaunchApplication (self.GetName ());
        }
        
        public static bool IsShownInDock (this NSApplication self)
        {
            var path = Path.Combine (NSBundle.MainBundle.BundlePath, "Contents/Info.plist");
            var plist = NSMutableDictionary.FromFile (path);
            var uie = plist[(NSString) "LSUIElement"] as NSNumber;
            return uie == null ? true : !uie.BoolValue;
        }
        
        public static void SetIsShownInDock (this NSApplication self, bool visible = true, bool relaunch = false)
        {
            if (visible == self.IsShownInDock ()) return;
            
            var path = Path.Combine (NSBundle.MainBundle.BundlePath, "Contents/Info.plist");
            var plist = NSMutableDictionary.FromFile (path);
            plist[(NSString) "LSUIElement"] = NSNumber.FromBoolean (!visible);
            
            if (plist.WriteToFile (path, true)) {
                Process.Start ("/usr/bin/touch", string.Format ("-fmac \"{0}\"", path));
                if (relaunch) self.Relaunch ();
            }
        }
        
        public static void Relaunch (this NSApplication self)
        {
            var pid = NSProcessInfo.ProcessInfo.ProcessIdentifier;
            var script = string.Format (@"
                while [ `ps -p {0} | wc -l` -gt 1 ]; do sleep 0.1; done; open '{1}'
            ", pid, NSBundle.MainBundle.BundlePath);
            NSTask.LaunchFromPath ("/bin/sh", new [] { "-c", script });
            self.Terminate (self);
        }
    }
}