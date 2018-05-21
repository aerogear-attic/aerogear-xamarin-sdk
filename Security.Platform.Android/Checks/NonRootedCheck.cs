using System;
using System.IO;
using Android.Content;
using Android.OS;
using Java.Lang;

namespace AeroGear.Mobile.Security.Checks
{
    /// <summary>
    /// A check for whether the device the application is running on is rooted.
    /// </summary>
    public class NonRootedCheck : AbstractSecurityCheck
    {
        protected override string Name { get { return "Rooted Check"; } }

        private readonly Context context;

        public NonRootedCheck(Context ctx)
        {
            this.context = ctx;
        }

        public override SecurityCheckResult Check()
        {
            bool rooted = CheckRootMethod1() || CheckRootMethod2() || CheckRootMethod3();
            return new SecurityCheckResult(this, !rooted);
        }

        private bool CheckRootMethod1()
        {
            string buildTags = Build.Tags;
            return buildTags != null && buildTags.Contains("test-keys");
        }

        private bool CheckRootMethod2()
        {
            string[] paths = { "/system/app/Superuser.apk", "/sbin/su",
                "/system/bin/su", "/system/xbin/su", "/data/local/xbin/su",
                "/data/local/bin/su", "/system/sd/xbin/su",
                "/system/bin/failsafe/su", "/data/local/su", "/su/bin/su"};
            foreach (string path in paths)
            {
                if (File.Exists(path)) return true;
            }
            return false;
        }

        private bool CheckRootMethod3()
        {
            Java.Lang.Process process = null;
            try
            {
                process = Runtime.GetRuntime().Exec(new string[] { "/system/xbin/which", "su" });
                Java.IO.BufferedReader input = new Java.IO.BufferedReader(
                    new Java.IO.InputStreamReader(process.InputStream));
                if (input.ReadLine() != null) return true;
                return false;
            }
            catch (Throwable)
            {
                return false;
            }
            finally
            {
                if (process != null) process.Destroy();
            }
        }
    }
}
