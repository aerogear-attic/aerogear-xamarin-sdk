using System;
namespace AeroGear.Mobile.Core.Exception
{
    public class ServiceModuleInstanceNotFoundException : System.Exception
    {
        public ServiceModuleInstanceNotFoundException(String message) : base(message) { }
        public ServiceModuleInstanceNotFoundException(String message, System.Exception exception) : base(message, exception) { }
    }
}
