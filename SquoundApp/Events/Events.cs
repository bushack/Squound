using SquoundApp.Interfaces;

using Shared.DataTransfer;
using Shared.Interfaces;


namespace SquoundApp.Events
{
    public record CategoriesLoadedEvent(IReadOnlyList<CategoryDto> Categories) : IEvent;

    //public record CategorySelectedEvent(CategoryDto? SelectedCategory) : IEvent;

    //public record SubcategorySelectedEvent(SubcategoryDto? SelectedSubcategory) : IEvent;

    public record SearchContextChangedEvent(ISearchContext Context) : IEvent;
}
