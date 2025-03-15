namespace GenericServiceLocator.Samples.Runtime.AudioModule
{
    /// <summary>
    /// Service interface for audio functionality.
    /// </summary>
    public interface IAudioService
    {
        void PlaySound(string soundName);
    }
}