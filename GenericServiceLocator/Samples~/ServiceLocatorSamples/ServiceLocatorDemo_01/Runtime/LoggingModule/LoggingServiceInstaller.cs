using GenericServiceLocator.Runtime;

namespace GenericServiceLocator.Samples.Runtime
{
    /// <summary>
    /// Installer responsible for registering the LoggingService.
    /// </summary>
    public static class LoggingServiceInstaller
    {
        public static void Install()
        {
            if (!ServiceLocator.TryGet<ILoggingService>(service: out _))
            {
                ServiceLocator.Register<ILoggingService>(service: new LoggingService());
            }
        }
    }
}