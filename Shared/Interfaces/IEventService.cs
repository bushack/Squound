

namespace Shared.Interfaces
{
    public interface IEvent
    {
    }


    public interface IEventService
    {
        void Subscribe<TEvent>(Action<TEvent> handler) where TEvent : IEvent;
        void Unsubscribe<TEvent>(Action<TEvent> handler) where TEvent : IEvent;
        void Publish<TEvent>(TEvent eventItem) where TEvent : IEvent;
    }
}
