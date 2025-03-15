using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace GenericServiceLocator.Samples.ServiceLocatorDemo_02.Runtime
{
    /// <summary>
    /// Advanced, thread-safe implementation of the IEventBus interface.
    /// </summary>
    public class AdvancedEventBus : IEventBus
    {
        private class SubscriptionEntry<TEvent>
        {
            public Action<TEvent> Handler { get; }
            public bool Once { get; }

            public SubscriptionEntry(Action<TEvent> handler, bool once)
            {
                Handler = handler;
                Once = once;
            }
        }

        private readonly Dictionary<Type, List<object>> _subscriptions = new Dictionary<Type, List<object>>();
        private readonly object _lock = new object();

        /// <summary>
        /// Subscribe a handler to an event of type TEvent.
        /// </summary>
        public void Subscribe<TEvent>(Action<TEvent> handler)
        {
            if (handler == null)
                throw new ArgumentNullException(paramName: nameof(handler));

            var entry = new SubscriptionEntry<TEvent>(handler: handler, once: false);
            var eventType = typeof(TEvent);

            lock (_lock)
            {
                if (!_subscriptions.TryGetValue(key: eventType, value: out var list))
                {
                    list = new List<object>();
                    _subscriptions[key: eventType] = list;
                }
                list.Add(item: entry);
            }
        }

        /// <summary>
        /// Subscribe a handler that will be invoked only once.
        /// After the first event, it is automatically unsubscribed.
        /// </summary>
        public void SubscribeOnce<TEvent>(Action<TEvent> handler)
        {
            if (handler == null)
                throw new ArgumentNullException(paramName: nameof(handler));

            var entry = new SubscriptionEntry<TEvent>(handler: handler, once: true);
            var eventType = typeof(TEvent);

            lock (_lock)
            {
                if (!_subscriptions.TryGetValue(key: eventType, value: out var list))
                {
                    list = new List<object>();
                    _subscriptions[key: eventType] = list;
                }
                list.Add(item: entry);
            }
        }

        /// <summary>
        /// Unsubscribe a handler from an event of type TEvent.
        /// </summary>
        public void Unsubscribe<TEvent>(Action<TEvent> handler)
        {
            if (handler == null)
                throw new ArgumentNullException(paramName: nameof(handler));

            var eventType = typeof(TEvent);
            lock (_lock)
            {
                if (_subscriptions.TryGetValue(key: eventType, value: out var list))
                {
                    list.RemoveAll(match: x => x is SubscriptionEntry<TEvent> entry && entry.Handler == handler);
                }
            }
        }

        /// <summary>
        /// Publish an event synchronously to all subscribed handlers.
        /// </summary>
        public void Publish<TEvent>(TEvent evt)
        {
            var eventType = typeof(TEvent);
            List<SubscriptionEntry<TEvent>> handlers = new List<SubscriptionEntry<TEvent>>();

            lock (_lock)
            {
                if (_subscriptions.TryGetValue(key: eventType, value: out var list))
                {
                    foreach (var obj in list)
                    {
                        if (obj is SubscriptionEntry<TEvent> entry)
                        {
                            handlers.Add(item: entry);
                        }
                    }
                }
            }

            foreach (var entry in handlers)
            {
                try
                {
                    entry.Handler(obj: evt);
                }
                catch (Exception ex)
                {
                    Debug.LogError(message: $"Error in event handler for {eventType}: {ex}");
                }
            }

            lock (_lock)
            {
                if (_subscriptions.TryGetValue(key: eventType, value: out var list))
                {
                    list.RemoveAll(match: x => x is SubscriptionEntry<TEvent> entry && entry.Once);
                }
            }
        }

        /// <summary>
        /// Publish an event asynchronously. Handlers are invoked in separate tasks.
        /// Waits until all handlers complete.
        /// </summary>
        public async Task PublishAsync<TEvent>(TEvent evt)
        {
            var eventType = typeof(TEvent);
            List<SubscriptionEntry<TEvent>> handlers = new List<SubscriptionEntry<TEvent>>();

            lock (_lock)
            {
                if (_subscriptions.TryGetValue(key: eventType, value: out var list))
                {
                    foreach (var obj in list)
                    {
                        if (obj is SubscriptionEntry<TEvent> entry)
                        {
                            handlers.Add(item: entry);
                        }
                    }
                }
            }

            var tasks = new List<Task>();
            foreach (var entry in handlers)
            {
                tasks.Add(item: Task.Run(action: () =>
                {
                    try
                    {
                        entry.Handler(obj: evt);
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError(message: $"Error in async event handler for {eventType}: {ex}");
                    }
                }));
            }

            await Task.WhenAll(tasks: tasks);

            lock (_lock)
            {
                if (_subscriptions.TryGetValue(key: eventType, value: out var list))
                {
                    list.RemoveAll(match: x => x is SubscriptionEntry<TEvent> entry && entry.Once);
                }
            }
        }
    }
}