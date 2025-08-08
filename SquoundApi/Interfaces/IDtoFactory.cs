using SquoundApi.Models;

using Shared.DataTransfer;


namespace SquoundApi.Interfaces
{
    public interface IDtoFactory
    {
        public ItemDto CreateItemDto(ItemModel item);
    }
}
