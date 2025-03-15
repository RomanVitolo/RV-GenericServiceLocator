using GenericServiceLocator.Runtime;

namespace GenericServiceLocator.Samples.ServiceLocatorDemo_02.Runtime
{
    /// <summary>
    /// Installer for the AdvancedEventBus.
    /// Registers a single, global instance of AdvancedEventBus into the ServiceLocator.
    /// </summary>
    public static class AdvancedEventBusInstaller
    {
        public static void Install()
        {
            if (!ServiceLocator.TryGet<IEventBus>(service: out _))
            {
                ServiceLocator.Register<IEventBus>(service: new AdvancedEventBus());
            }
        }
    }
}