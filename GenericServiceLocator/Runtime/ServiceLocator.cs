using System;
using System.Collections.Generic;
using UnityEngine;

namespace GenericServiceLocator.Runtime
{
    public abstract class ServiceLocator
    {
        private static readonly Dictionary<Type, object> _services = new Dictionary<Type, object>();
        private static readonly object _lock = new object();

        /// <summary>
        /// Registers a service instance for the given type TService.
        /// Throws an exception if the service is already registered.
        /// </summary>
        /// <typeparam name="T">The type of service to register.</typeparam>
        /// <param name="service">The service instance.</param>
        public static void Register<T>(T service)
        {
            if (service == null)
                throw new ArgumentNullException(paramName: nameof(service));

            var type = typeof(T);
            lock (_lock)
            {
                if (!_services.TryAdd(type, service))
                    throw new InvalidOperationException(message: $"Service of type {type} is already registered.");
            }
        }

        /// <summary>
        /// Retrieves the registered service of type TService.
        /// Throws an exception if the service is not found.
        /// </summary>
        /// <typeparam name="T">The type of service to retrieve.</typeparam>
        /// <returns>The service instance.</returns>
        public static T Get<T>()
        {
            var type = typeof(T);
            lock (_lock)
            {
                if (_services.TryGetValue(key: type, value: out object service))
                    return (T)service;
            }
            throw new InvalidOperationException(message: $"Service of type {type} is not registered.");
        }

        /// <summary>
        /// Attempts to retrieve the registered service of type TService.
        /// </summary>
        /// <typeparam name="T">The type of service to retrieve.</typeparam>
        /// <param name="service">The service instance if found, otherwise default.</param>
        /// <returns>True if the service is found, otherwise false.</returns>
        public static bool TryGet<T>(out T service)
        {
            var type = typeof(T);
            lock (_lock)
            {
                if (_services.TryGetValue(key: type, value: out object obj))
                {
                    service = (T)obj;
                    return true;
                }
            }
            service = default;
            return false;
        }

        /// <summary>
        /// Unregisters the service of type TService.
        /// </summary>
        /// <typeparam name="T">The type of service to unregister.</typeparam>
        public static void Unregister<T>()
        {
            var type = typeof(T);
            lock (_lock)
            {
                _services.Remove(key: type);
                Debug.Log($"Remove Service {type}");
            }
        }

        /// <summary>
        /// Clears all registered services.
        /// This is useful for resetting the locator, for example in unit tests.
        /// </summary>
        public static void Reset()
        {
            lock (_lock)
            {
                _services.Clear();
            }
        }
    }
}