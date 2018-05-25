using System;
using System.IO;
using UIKit;
using Foundation;

namespace AeroGear.Mobile.Security.Checks
{
    /// <summary>
    /// Check if the device is Jailbroken.
    /// </summary>
    public class NonJailbrokenCheck : AbstractSecurityCheck
    {
        protected override string Name => "Jailbreak Check";

        public NonJailbrokenCheck()
        {
        }

        public override SecurityCheckResult Check()
        {
            if (ObjCRuntime.Runtime.Arch == ObjCRuntime.Arch.DEVICE)
            {
                if (File.Exists("/Applications/Cydia.app")
                    || File.Exists("/Library/MobileSubstrate/MobileSubstrate.dylib")
                    || File.Exists("/bin/bash")
                    || File.Exists("/usr/sbin/sshd")
                    || File.Exists("/etc/apt")
                    || File.Exists("/private/var/lib/apt/")
                    || UIApplication.SharedApplication.CanOpenUrl(new NSUrl("cydia://package/com.example.package")))
                {
                    return new SecurityCheckResult(this, true);
                }

                try
                {
                    File.WriteAllText("/private/JailbreakTest.txt", "Jailbreak Test", System.Text.Encoding.UTF8);
                    return new SecurityCheckResult(this, true);
                }
                catch
                {
                    return new SecurityCheckResult(this, false);
                }
            }
            else
            {
                return new SecurityCheckResult(this, false);
            }
        }
    }
}
