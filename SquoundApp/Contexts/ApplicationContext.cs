using SquoundApp.Interfaces;

using Shared.Interfaces;


namespace SquoundApp.Contexts
{
    public class ApplicationContext(IEventService events, INavigationService navigation) : IApplicationContext
    {
        public IEventService Events { get; } = events ?? throw new ArgumentNullException(nameof(events));

        public INavigationService Navigation { get; } = navigation ?? throw new ArgumentNullException(nameof(navigation));
    }
}
