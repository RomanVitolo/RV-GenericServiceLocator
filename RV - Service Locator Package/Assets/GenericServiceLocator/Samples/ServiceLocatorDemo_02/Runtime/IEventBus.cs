using System;
using System.Threading.Tasks;

namespace GenericServiceLocator.Samples.ServiceLocatorDemo_02.Runtime
{
    /// <summary>
    /// Interface for an advanced EventBus.
    /// Supports subscribing, unsubscribing, publishing (sync and async),
    /// and "once" subscriptions.
    /// </summary>
    public interface IEventBus
    {
        void Subscribe<TEvent>(Action<TEvent> handler);
        void SubscribeOnce<TEvent>(Action<TEvent> handler);
        void Unsubscribe<TEvent>(Action<TEvent> handler);
        void Publish<TEvent>(TEvent evt);
        Task PublishAsync<TEvent>(TEvent evt);
    }
}