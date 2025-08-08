using SquoundApi.Models;


namespace SquoundApi.Interfaces
{
    public interface IItemRepository
    {
        bool DoesItemExist(long id);
        IEnumerable<ItemModel> All { get; }
        IEnumerable<ItemModel> Get(long id);
        ItemModel? Find(long id);
        void Insert(ItemModel item);
        void Update(ItemModel item);
        void Delete(long id);
    }
}
