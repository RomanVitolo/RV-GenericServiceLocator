using System;
using GenericServiceLocator.Runtime;
using GenericServiceLocator.Samples.Runtime.AudioModule;
using GenericServiceLocator.Samples.Runtime.PhysicsModule;
using UnityEngine;

namespace GenericServiceLocator.Samples.Runtime
{
    public class ServiceLocatorDemo : MonoBehaviour
    {
        private void Start()
        {
            var logger = ServiceLocator.Get<ILoggingService>();
            logger.Log(message: "Logging service retrieved successfully.");

            var physics = ServiceLocator.Get<IPhysicsService>();
            physics.SimulatePhysics();

            ServiceLocator.Reset();
            Debug.Log(message: "All services have been reset.");

            if (!ServiceLocator.TryGet<ILoggingService>(service: out logger))
                Debug.Log(message: "Logging service is no longer available after reset.");
        }
    }
}