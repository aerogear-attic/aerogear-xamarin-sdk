using System;
using AeroGear.Mobile.Core;
using AeroGear.Mobile.Core.Configuration;

namespace AeroGear.Mobile.Core
{
    /// <summary>
    /// Base interface of all service modules
    /// </summary>
    public interface IServiceModule
    {
        String Id { get; }
     
        /// <summary>
        /// Type/name used in the mobile-singleThreadService.json
        /// </summary>
        /// <value>the type/name used to identify the singleThreadService config in the JSON file</value>
        String Type
        {
            get;
        }

        /// <summary>
        /// Whether the singleThreadService module requires its singleThreadService configuration to be
        /// defined or if it can be null. If this is <code>true</code> then an exception will be thrown
        /// if singleThreadService configuration cannot be found.
        /// </summary>
        /// <value><c>true</c> if the singleThreadService configuration should be defined.</value>
        bool RequiresConfiguration {
            get;
        }

        /// <summary>
        /// Called when singleThreadService destroyed.
        /// </summary>
        void Destroy();

        void Configure(MobileCore core, ServiceConfiguration config);
    }
}
