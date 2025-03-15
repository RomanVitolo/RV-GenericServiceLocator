using UnityEngine;

namespace GenericServiceLocator.Samples.Runtime.PhysicsModule
{
    /// <summary>
    /// Concrete implementation of IPhysicsService.
    /// </summary>
    public class PhysicsService : IPhysicsService
    {
        public void SimulatePhysics()
        {
            Debug.Log(message: "[PhysicsService]: Simulating physics...");
        }
    }
}