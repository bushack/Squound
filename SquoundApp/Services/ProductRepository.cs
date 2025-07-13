using SquoundApp.Interfaces;
using SquoundApp.Models;


namespace SquoundApp.Services
{
    public class ProductRepository : IProductRepository
    {
        private List<Product> productList = new();

        public ProductRepository()
        {
            InitializeData();
        }

        public bool DoesProductExist(int id)
        {
            return productList.Any(product => product.Id == id);
        }

        public IEnumerable<Product> GetAllProducts
        {
            get
            {
                return productList;
            }
        }

        public Product Find(int id)
        {
            return productList.FirstOrDefault(product => product.Id == id);
        }

        public void Insert(Product product)
        {
            productList.Add(product);
        }

        public void Update(Product product)
        {
            var productToUpdate = this.Find(product.Id);
            var index = productList.IndexOf(productToUpdate);

            productList.RemoveAt(index);
            productList.Insert(index, product);
        }

        public void Delete(int id)
        {
            productList.Remove(this.Find(id));
        }

        private void InitializeData()
        {
            productList.Add(new Product
            {
                Id = 1,
                Name = "Product A",
                Manufacturer = "Squound",
                Description = "A revolutionary new product!",
                Price = 100
            });

            productList.Add(new Product
            {
                Id = 2,
                Name = "Squound Pro",
                Manufacturer = "Squound",
                Description = "The professional version of Squound.",
                Price = 200
            });
        }
    }
}
