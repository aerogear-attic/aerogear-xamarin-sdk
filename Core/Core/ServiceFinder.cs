using System;
using Unity;

namespace AeroGear.Mobile.Core
{
    public static class ServiceFinder
    {
        private static readonly UnityContainer Container = new UnityContainer();

        /// <summary>
        ///     Resolve the correct implementation for the type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>an instance of the correct implementation class</returns>
        public static T Resolve<T>()
        {
            return Container.Resolve<T>();
        }

        public static void RegisterType<TFrom, TTo>() where TTo : TFrom
        {
            Container.RegisterType<TFrom, TTo>();
        }

        public static void RegisterInstance<TInstance>(TInstance instance)
        {
            Container.RegisterInstance<TInstance>(instance);
        }

        public static bool IsRegistered<TInstance>()
        {
            return Container.IsRegistered<TInstance>();
        }

    }
}
