using GenericServiceLocator.Samples.Runtime;
using GenericServiceLocator.Samples.Runtime.AudioModule;
using GenericServiceLocator.Samples.Runtime.PhysicsModule;
using UnityEngine;

namespace GenericServiceLocator.Samples.ServiceLocatorDemo_02.Runtime
{
    public class ServiceLocatorGlobalInstaller : MonoBehaviour
    {
        private void Awake()
        {
            LoggingServiceInstaller.Install();
            PhysicsServiceInstaller.Install();
            AdvancedEventBusInstaller.Install();
            
            Debug.Log(message: "Advanced EventBus installed via ServiceLocator.");
        }
    }
}