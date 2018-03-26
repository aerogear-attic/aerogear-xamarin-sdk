using System;
using AeroGear.Mobile.Core.Logging;
using Android.Util;

namespace AeroGear.Mobile.Core.Platform.Android.Logging
{
    /// <summary>
    /// Android specific implementation of <see cref="ILogger"/> using default Android <see cref="Log"/>.
    /// </summary>
    public class AndroidLogger : ILogger
    {
        public const string DEFAULT_TAG = "AeroGear";

        public void Debug(string tag, string message) => Log.Debug(tag,message);

        public void Debug(string message) => Log.Debug(DEFAULT_TAG,message);

        public void Debug(string tag, string message, System.Exception e) => Log.Debug(tag, message, e);

        public void Debug(string message, System.Exception e) => Log.Debug(DEFAULT_TAG, message, e);

        public void Error(string tag, string message) => Log.Error(tag, message);

        public void Error(string message) => Log.Error(DEFAULT_TAG, message);

        public void Error(string tag, string message, System.Exception e) => Log.Error(tag, message, e);

        public void Error(string message, System.Exception e) => Log.Error(DEFAULT_TAG, message, e);

        public void Info(string tag, string message) => Log.Info(tag, message);

        public void Info(string message) => Log.Info(DEFAULT_TAG, message);

        public void Info(string tag, string message, System.Exception e) => Log.Info(tag, message, e);

        public void Info(string message, System.Exception e) => Log.Info(DEFAULT_TAG, message, e);

        public void Warning(string tag, string message) => Log.Warn(tag, message);

        public void Warning(string message) => Log.Warn(DEFAULT_TAG, message);

        public void Warning(string tag, string message, System.Exception e) => Log.Warn(tag, message, e);

        public void Warning(string message, System.Exception e) => Log.Warn(DEFAULT_TAG, message, e);

    }
}
