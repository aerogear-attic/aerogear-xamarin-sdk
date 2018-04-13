using System;
namespace AeroGear.Mobile.Core.Exception
{
    /// <summary>
    /// This is an exception that is thrown when Core or module wasn't properly initialized.
    /// </summary>
    public class NotInitializedException: System.Exception
    {
        public NotInitializedException(String message) : base(message) { }
        public NotInitializedException(String message, System.Exception exception) : base(message, exception) { }
    }
}
