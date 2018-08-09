using System;
using System.IO;
using UIKit;
using Foundation;

namespace AeroGear.Mobile.Security.Checks
{
    /// <summary>
    /// Check if the device is Jailbroken.
    /// </summary>
    public class JailbrokenDeviceCheck : AbstractDeviceCheck
    {
        protected override string Name => "Jailbreak Check";

        public JailbrokenDeviceCheck()
        {
        }

        public override DeviceCheckResult Check()
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
                    return new DeviceCheckResult(this, true);
                }

                try
                {
                    File.WriteAllText("/private/JailbreakTest.txt", "Jailbreak Test", System.Text.Encoding.UTF8);
                    return new DeviceCheckResult(this, true);
                }
                catch
                {
                    return new DeviceCheckResult(this, false);
                }
            }
            else
            {
                return new DeviceCheckResult(this, false);
            }
        }
    }
}
