using Shared.Interfaces;


namespace SquoundApp.Interfaces
{
    public interface IApplicationContext
    {
        IEventService Events { get; }

        INavigationService Navigation { get; }
    }
}