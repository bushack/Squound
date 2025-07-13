using SquoundApi.Interfaces;
using SquoundApi.Models;


namespace SquoundApi.Services
{
    public class ProductRepository : IProductRepository
    {
        private List<ProductModel> productList = new();

        public ProductRepository()
        {
            InitializeData();
        }

        public bool DoesProductExist(long id)
        {
            return productList.Any(product => product.Id == id);
        }

        public IEnumerable<ProductModel> All
        {
            get
            {
                return productList;
            }
        }

        public ProductModel Find(long id)
        {
            return productList.FirstOrDefault(product => product.Id == id);
        }

        public void Insert(ProductModel product)
        {
            productList.Add(product);
        }

        public void Update(ProductModel product)
        {
            var productToUpdate = this.Find(product.Id);
            var index = productList.IndexOf(productToUpdate);

            productList.RemoveAt(index);
            productList.Insert(index, product);
        }

        public void Delete(long id)
        {
            productList.Remove(this.Find(id));
        }

        private void InitializeData()
        {
            productList.Add(new ProductModel
            {
                Id = 1,
                Name = "Product A",
                Manufacturer = "Squound",
                Description = "A revolutionary new product!",
                Price = 100
            });

            productList.Add(new ProductModel
            {
                Id = 2,
                Name = "Squound Pro",
                Manufacturer = "Squound",
                Description = "The professional version of Squound.",
                Price = 200
            });

            productList.Add(new ProductModel
            {
                Id = 28071983,
                Name = "Squound Staff : Chris",
                Manufacturer = "Monica & John Collier",
                Description = "Second born child of the aforementioned couple. Chris is a co-founder of Squound LLC",
                Price = 100000000
            });

            productList.Add(new ProductModel
            {
                Id = 100000,
                Name = "1960s Sofa",
                Manufacturer = "G-Plan",
                Description = "Luxurious sofa designed and manufactureed by the world-famous G-Plan. You won't regret buuying this!",
                Price = 2500
            });
        }
    }
}
