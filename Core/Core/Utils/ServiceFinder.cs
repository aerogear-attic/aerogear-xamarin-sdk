using System;
using System.Collections.Generic;

namespace AeroGear.Mobile.Core.Utils
{
    public static class ServiceFinder
    {
        private static Dictionary<Type, Type> typeMapping = new Dictionary<Type, Type>();
        private static Dictionary<Type, Object> instanceMapping = new Dictionary<Type, Object>();

        /// <summary>
        ///     Resolve the correct implementation for the type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>an instance of the correct implementation class</returns>
        public static T Resolve<T>()
        {
            Type type = typeof(T);
            if (instanceMapping.ContainsKey(type))
            {
                return (T)instanceMapping[type];
            }

            if (typeMapping.ContainsKey(type))
            {
                return (T)Activator.CreateInstance(typeMapping[type]);
            }
            throw new System.Exception(String.Format("No mapping could been found for type {0}", type.Name));
        }

        public static void RegisterType<TFrom, TTo>() where TTo : TFrom
        {
            typeMapping[typeof(TFrom)] = typeof(TTo);
        }

        public static void RegisterInstance<TInstance>(TInstance instance)
        {
            instanceMapping[typeof(TInstance)] = instance;
        }

        public static bool IsRegistered<TInstance>()
        {
            Type type = typeof(TInstance);
            return typeMapping.ContainsKey(type) || instanceMapping.ContainsKey(type);
        }
    }
}