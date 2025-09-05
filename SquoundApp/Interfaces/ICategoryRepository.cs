using Shared.DataTransfer;


namespace SquoundApp.Interfaces
{
    public interface ICategoryRepository
    {
        public Task<IReadOnlyList<CategoryDto>> GetCategoriesAsync();
    }
}
