using AeroGear.Mobile.Core.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AeroGear.Mobile.Core.Utils
{
    ///<summary>
    ///This class maintains mappings for <see cref="MobileCore"/> of platform specific implementaityons of <see cref="IServiceModule"/>
    ///</summary>
    sealed class ServiceRegistry
    {
        private static Object registryLock = new Object();

        private readonly ILogger logger;
        private readonly IPlatformInjector injector;
        private List<Type> dependencyTypes;
        
        internal ServiceRegistry(ILogger logger, IPlatformInjector injector) {
            this.logger = logger;
            this.injector = injector;
        }

        /// <summary>
        /// This method scans all assemblies for classes annotated with <see cref="ServiceAttribute"/>.
        /// </summary>
        /// <param name="injector">The <see cref="IPlatformInjector"/> to use to get access to platform resources.</param>
        internal async Task RegisterAssemblies()
        {
            await Task.Run(() => {
                lock (registryLock)
                {
                    if (dependencyTypes != null) {
                        return;//Short circuit as we've already calculated the dependency tree;
                    }
                    dependencyTypes = new List<Type>();
                    Assembly[] assemblies = injector.GetAssemblies();

                    Type targetAttrType = typeof(ServiceAttribute);

                    // Don't use LINQ for performance reasons
                    // Naive implementation can easily take over a second to run
                    foreach (Assembly assembly in assemblies)
                    {
                        Attribute[] attributes;
                        try
                        {
                            attributes = assembly.GetCustomAttributes(targetAttrType).ToArray();
                        }
                        catch (System.IO.FileNotFoundException exception)
                        {
                            // Sometimes the previewer doesn't actually have everything required for these loads to work
                            logger.Error($"Could not load assembly: {assembly.FullName} for Attibute {targetAttrType.FullName} | Some renderers may not be loaded", exception);
                            continue;
                        }

                        if (attributes.Length == 0)
                            continue;

                        foreach (ServiceAttribute attribute in attributes)
                        {
                            if (!dependencyTypes.Contains(attribute.Implementor))
                            {
                                dependencyTypes.Add(attribute.Implementor);
                            }
                        }
                    }
                }
            }).ConfigureAwait(false);
            
        }

        /// <summary>
        /// This method will find the implmentation of the service class.
        /// 
        /// If <see cref="RegisterAssemblies"/> has not been called or is still running this method will block until it has finished.
        /// 
        /// </summary>
        /// <param name="serviceClass">The class to find a platform implementation for</param>
        /// <returns>A platform implmention or null</returns>
        internal Type FindImplementation(Type serviceClass)
        {
            if (dependencyTypes == null) {
                this.RegisterAssemblies().Wait();
            }

            return dependencyTypes.FirstOrDefault(t => serviceClass.GetTypeInfo().IsAssignableFrom(t.GetTypeInfo()));
        }
    }
}
