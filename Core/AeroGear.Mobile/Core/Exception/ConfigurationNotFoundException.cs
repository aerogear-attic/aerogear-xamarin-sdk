using System;
namespace Core.AeroGear.Mobile.Core.Exception
{
    /// <summary>
    /// This is an exception that is thrown when core can't find configuration JSON file.
    /// </summary>
    public class ConfigurationNotFoundException: System.Exception
    {
        public ConfigurationNotFoundException(String message) : base(message) {}
        public ConfigurationNotFoundException(String message,System.Exception exception) : base(message,exception) { }
    }
}
