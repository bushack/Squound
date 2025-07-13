using SquoundApp.Models;


namespace SquoundApp.Interfaces
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
