using Shared.DataTransfer;


namespace SquoundApp.Selectors
{
    public class CategoryTemplateSelector : DataTemplateSelector
    {
        public required DataTemplate CategoryTemplate { get; set; }
        public required DataTemplate SubcategoryTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            return item switch
            {
                CategoryDto => CategoryTemplate,
                SubcategoryDto => SubcategoryTemplate,
                null => throw new ArgumentNullException(nameof(item), "Item was null"),
                _ => throw new NotSupportedException($"Unsupported item type: {item?.GetType().Name}")
            };
        }
    }
}
