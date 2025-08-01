

namespace Shared.DataTransfer
{
    // CategoryDto is used to transfer category data between the client and server.
    public class CategoryDto
    {
        public string Name { get; set; } = string.Empty;

        public List<SubcategoyDto> Subcategories { get; set; } = [];
    }


    // SubcategoyDto is used to transfer subcategory data within a category.
    public class SubcategoyDto
    {
        public string Name { get; set; } = string.Empty;
    }
}
