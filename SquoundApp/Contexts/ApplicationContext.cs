//using SquoundApp.Interfaces;

//using Shared.Interfaces;


//namespace SquoundApp.Contexts
//{
//    public class ApplicationContext(ICategoryService categories, IEventService events,
//        IItemService items, ISearchService search) : IApplicationContext
//    {
//        public ICategoryService Categories { get; } = categories ?? throw new ArgumentNullException(nameof(categories));

//        public IEventService Events { get; } = events ?? throw new ArgumentNullException(nameof(events));

//        public IItemService Items { get; } = items ?? throw new ArgumentNullException(nameof(items));

//        public ISearchService Search { get; } = search ?? throw new ArgumentNullException(nameof(search));
//    }
//}
