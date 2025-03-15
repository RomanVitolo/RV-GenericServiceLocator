using UnityEngine;

namespace GenericServiceLocator.Samples.Runtime
{
    /// <summary>
    /// Concrete implementation of ILoggingService.
    /// </summary>
    public class LoggingService : ILoggingService
    {
        public void Log(string message)
        {
            Debug.Log(message: "[LoggingService]: " + message);
        }
    }
}