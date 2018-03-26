using System;
using AeroGear.Mobile.Core.Logging;

namespace Core.Platform.Logging
{
    public class IOSLogger : ILogger
    {
        public const string DEFAULT_TAG = "AeroGear";

        private void log(string level, string tag, string message, Exception e)
        {
            System.Console.WriteLine($"{level}/{tag}/{message}");
            if (e != null)
            {
                System.Console.WriteLine($"{level}/{e.ToString()}");  
            } 
        }

        public void Debug(string tag, string message) => log("D", tag, message, null);
        public void Debug(string message) => log("D", DEFAULT_TAG, message, null);
        public void Debug(string tag, string message, Exception e) => log("D", tag, message, e);
        public void Debug(string message, Exception e) => log("D", DEFAULT_TAG, message, null);

        public void Info(string tag, string message) => log("I", tag, message, null);
        public void Info(string message) => log("I", DEFAULT_TAG, message, null);
        public void Info(string tag, string message, Exception e) => log("I", tag, message, e);
        public void Info(string message, Exception e) => log("I", DEFAULT_TAG, message, null);

        public void Error(string tag, string message) => log("E", tag, message, null);
        public void Error(string message) => log("E", DEFAULT_TAG, message, null);
        public void Error(string tag, string message, Exception e) => log("E", tag, message, e);
        public void Error(string message, Exception e) => log("E", DEFAULT_TAG, message, null);

        public void Warning(string tag, string message) => log("W", tag, message, null);
        public void Warning(string message) => log("W", DEFAULT_TAG, message, null);
        public void Warning(string tag, string message, Exception e) => log("W", tag, message, e);
        public void Warning(string message, Exception e) => log("W", DEFAULT_TAG, message, null);


    }
}
