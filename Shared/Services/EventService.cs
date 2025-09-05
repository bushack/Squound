using System.Collections.Concurrent;

using Microsoft.Extensions.Logging;

using Shared.Interfaces;


namespace Shared.Services
{
    public class EventService : IEventService
    {
        private readonly ILogger<EventService> _logger;
        private readonly ConcurrentDictionary<Type, List<Delegate>> _subscribers = new();


        public EventService(ILogger<EventService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }


        public void Subscribe<TEvent>(Action<TEvent> handler) where TEvent : IEvent
        {
            // Get or create the list of handlers for the event type.
            var handlers = _subscribers.GetOrAdd(typeof(TEvent), _ => []);
            
            lock (handlers)
            {
                handlers.Add(handler);

                _logger.LogDebug("Subscribed to event {EventType} with handler {Handler}", typeof(TEvent).Name, handler.Method.Name);
            }
        }


        public void Unsubscribe<TEvent>(Action<TEvent> handler) where TEvent : IEvent
        {
            // Get the list of handlers for the event type.
            if (_subscribers.TryGetValue(typeof(TEvent), out var handlers))
            {
                lock (handlers)
                {
                    handlers.Remove(handler);

                    _logger.LogDebug("Unsubscribed from event {EventType} with handler {Handler}", typeof(TEvent).Name, handler.Method.Name);
                }
            }
        }


        public void Publish<TEvent>(TEvent eventItem) where TEvent : IEvent
        {
            // Get the list of handlers for the event type.
            if (_subscribers.TryGetValue(typeof(TEvent), out var handlers))
            {
                // Create a copy of the handlers to guard against modified during iteration.
                List<Delegate> handlersCopy;
                lock (handlers)
                {
                    handlersCopy = [.. handlers];
                }

                // Invoke each handler.
                foreach (var handler in handlersCopy)
                {
                    if (handler is Action<TEvent> action)
                    {
                        action(eventItem);

                        _logger.LogDebug("Published event {EventType} to handler {Handler}", typeof(TEvent).Name, action.Method.Name);
                    }
                }
            }
        }
    }
}
