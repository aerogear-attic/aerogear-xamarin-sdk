using AeroGear.Mobile.Core.Logging;
using System;

namespace AeroGear.Mobile.Core.Logging
{
    public class IOSLogger : ILogger
    {
        public const string DEFAULT_TAG = "AeroGear";

        private void Log(string level, string tag, string message, System.Exception e)
        {
            System.Console.WriteLine($"{level}/{tag}/{message}");
            if (e != null)
            {
                System.Console.WriteLine($"{level}/{e.ToString()}");  
            } 
        }

        public void Debug(string tag, string message) => Log("D", tag, message, null);
        public void Debug(string message) => Log("D", DEFAULT_TAG, message, null);
        public void Debug(string tag, string message, System.Exception e) => Log("D", tag, message, e);
        public void Debug(string message, System.Exception e) => Log("D", DEFAULT_TAG, message, null);

        public void Info(string tag, string message) => Log("I", tag, message, null);
        public void Info(string message) => Log("I", DEFAULT_TAG, message, null);
        public void Info(string tag, string message, System.Exception e) => Log("I", tag, message, e);
        public void Info(string message, System.Exception e) => Log("I", DEFAULT_TAG, message, null);

        public void Error(string tag, string message) => Log("E", tag, message, null);
        public void Error(string message) => Log("E", DEFAULT_TAG, message, null);
        public void Error(string tag, string message, System.Exception e) => Log("E", tag, message, e);
        public void Error(string message, System.Exception e) => Log("E", DEFAULT_TAG, message, null);

        public void Warning(string tag, string message) => Log("W", tag, message, null);
        public void Warning(string message) => Log("W", DEFAULT_TAG, message, null);
        public void Warning(string tag, string message, System.Exception e) => Log("W", tag, message, e);
        public void Warning(string message, System.Exception e) => Log("W", DEFAULT_TAG, message, null);


    }
}
