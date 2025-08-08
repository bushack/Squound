

namespace Shared.DataTransfer
{
    public class SearchResponseDto<T>
    {
        public int TotalItems { get; set; } = 0;

        public int PageSize { get; set; } = 0;

        public int CurrentPage { get; set; } = 0;

        public int TotalPages => TotalItems == 0 || PageSize == 0 ? 0 : (int)Math.Ceiling((decimal)TotalItems / PageSize);

        public List<T> Items { get; set; } = [];
    }
}
