using GenericServiceLocator.Runtime;

namespace GenericServiceLocator.Samples.Runtime.PhysicsModule
{
    /// <summary>
    /// Installer responsible for registering the PhysicsService.
    /// </summary>
    public static class PhysicsServiceInstaller
    {
        public static void Install()
        {
            if (!ServiceLocator.TryGet<IPhysicsService>(service: out _))
            {
                ServiceLocator.Register<IPhysicsService>(service: new PhysicsService());
            }
        }
    }
}