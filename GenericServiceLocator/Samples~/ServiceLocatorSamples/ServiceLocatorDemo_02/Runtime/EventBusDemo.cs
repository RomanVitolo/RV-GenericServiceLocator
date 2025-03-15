using System;
using GenericServiceLocator.Runtime;
using GenericServiceLocator.Samples.Runtime;
using GenericServiceLocator.Samples.Runtime.AudioModule;
using GenericServiceLocator.Samples.Runtime.PhysicsModule;
using UnityEngine;

namespace GenericServiceLocator.Samples.ServiceLocatorDemo_02.Runtime
{
    public class EventBusDemo : MonoBehaviour
    {
        private class PlayerScoredEvent
        {
            public int Points;
            public string PlayerName;
        }

        private void OnPlayerScored(PlayerScoredEvent scoredEvent)
        {
            var logger = ServiceLocator.Get<ILoggingService>();
            logger.Log(message: $"[Persistent Handler] {scoredEvent.PlayerName} scored {scoredEvent.Points} points.");
        }

        private void OnPlayerScoredOnce(PlayerScoredEvent scoredEvent)
        {
            var logger = ServiceLocator.Get<ILoggingService>();
            logger.Log(message: $"[Once Handler] Congratulations {scoredEvent.PlayerName} for scoring {scoredEvent.Points} points (only once)!");
        }

        private async void Start()
        {
            try
            {
                var logger = ServiceLocator.Get<ILoggingService>();
                logger.Log(message: "FullServiceLocatorEventBusDemo starting.");

                var eventBus = ServiceLocator.Get<IEventBus>();

                eventBus.Subscribe<PlayerScoredEvent>(handler: OnPlayerScored);
                eventBus.SubscribeOnce<PlayerScoredEvent>(handler: OnPlayerScoredOnce);
                logger.Log(message: "Subscribed persistent and once event handlers.");

                var scoreEvent = new PlayerScoredEvent { Points = 10, PlayerName = "Alice" };
                logger.Log(message: "Publishing synchronous PlayerScoredEvent...");
                await eventBus.PublishAsync(evt: scoreEvent);

                var scoreEventAsync = new PlayerScoredEvent { Points = 20, PlayerName = "Bob" };
                logger.Log(message: "Publishing asynchronous PlayerScoredEvent...");
                await eventBus.PublishAsync(evt: scoreEventAsync);

                var physicsService = ServiceLocator.Get<IPhysicsService>();
                physicsService.SimulatePhysics();

                eventBus.Unsubscribe<PlayerScoredEvent>(handler: OnPlayerScored);
                logger.Log(message: "Unsubscribed persistent event handler.");

                var scoreEventAfterUnsub = new PlayerScoredEvent { Points = 15, PlayerName = "Charlie" };
                logger.Log(message: "Publishing PlayerScoredEvent after unsubscription (no handlers expected)...");
                await eventBus.PublishAsync(evt: scoreEventAfterUnsub);
               
                ServiceLocator.Reset();
                logger.Log(message: "All services have been reset.");

                if (!ServiceLocator.TryGet(service: out logger))
                {
                    Debug.Log(message: "LoggingService is no longer available after reset.");
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning(e.Message);
            }
        }
    }
}