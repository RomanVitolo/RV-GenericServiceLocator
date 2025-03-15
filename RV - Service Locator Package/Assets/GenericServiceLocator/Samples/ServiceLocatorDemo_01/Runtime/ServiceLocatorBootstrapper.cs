using GenericServiceLocator.Samples.Runtime.AudioModule;
using GenericServiceLocator.Samples.Runtime.PhysicsModule;
using UnityEngine;

namespace GenericServiceLocator.Samples.Runtime
{
    /// <summary>
    /// Composition root that installs all module services using their dedicated installers.
    /// </summary>
    public class ServiceLocatorBootstrapper : MonoBehaviour
    {
        private void Awake()
        {
            LoggingServiceInstaller.Install();
            PhysicsServiceInstaller.Install();
            Debug.Log(message: "All services have been installed via their respective installers.");
        }
    }
}