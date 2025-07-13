using SquoundApi.Models;


namespace SquoundApi.Interfaces
{
    public interface IProductRepository
    {
        bool DoesProductExist(long id);
        IEnumerable<ProductModel> All { get; }
        ProductModel Find(long id);
        void Insert(ProductModel product);
        void Update(ProductModel product);
        void Delete(long id);
    }
}
