using System;
namespace AeroGear.Mobile.Core.Exception
{
    /// <summary>
    /// This is an exception that is thrown when bootstrapping a module fails.
    /// </summary>
    public class InitializationException: System.Exception
    {
        public InitializationException(String message) : base(message) {}
        public InitializationException(String message,System.Exception exception) : base(message,exception) { }
    }
}
