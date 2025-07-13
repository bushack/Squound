using SquoundApp_v1.Models;


namespace SquoundApp_v1.Interfaces
{
    public interface IProductRepository
    {
        bool DoesProductExist(int id);
        IEnumerable<Product> GetAllProducts { get; }
        Product Find(int id);
        void Insert(Product product);
        void Update(Product product);
        void Delete(int id);
    }
}
