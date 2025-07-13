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
                Id = 100000,
                Name = "1970s Teak Cabinet",
                Manufacturer = "Abbess",
                Description = "Teak sliding doors with inset aluminium circular pulls, robust metal legs and adjustable internal shelf. Versatile and charming.",
                Price = 1234,
                Image0 = "https://raw.githubusercontent.com/bushack/images/main/cabinet_abbess_1_.jpg",
                Image1 = "https://raw.githubusercontent.com/bushack/images/main/cabinet_abbess_2_.jpg",
                Image2 = "https://raw.githubusercontent.com/bushack/images/main/cabinet_abbess_3_.jpg",
                Image3 = "https://raw.githubusercontent.com/bushack/images/main/cabinet_abbess_4_.jpg",
                Image4 = "https://raw.githubusercontent.com/bushack/images/main/cabinet_abbess_5_.jpg",
                Image5 = "https://raw.githubusercontent.com/bushack/images/main/cabinet_abbess_6_.jpg"
            });

            productList.Add(new ProductModel
            {
                Id = 100001,
                Name = "Mid-Century Tallboy",
                Manufacturer = "Wrighton Furniture",
                Description = "Elegant styling, circular pulls, sculptural leg detailing and beautiful timbers make this an especially unusual & desirable piece.",
                Price = 100000000,
                Image0 = "https://raw.githubusercontent.com/bushack/images/main/mid_century_tallboy_wrighton.jpg",
                Image1 = "https://raw.githubusercontent.com/bushack/images/main/cabinet_abbess_2_.jpg",
                Image2 = "https://raw.githubusercontent.com/bushack/images/main/cabinet_abbess_3_.jpg",
                Image3 = "https://raw.githubusercontent.com/bushack/images/main/cabinet_abbess_4_.jpg",
                Image4 = "https://raw.githubusercontent.com/bushack/images/main/cabinet_abbess_5_.jpg",
                Image5 = "https://raw.githubusercontent.com/bushack/images/main/cabinet_abbess_6_.jpg"
            });

            productList.Add(new ProductModel
            {
                Id = 100002,
                Name = "1960s Tallboy",
                Manufacturer = "Austinsuite",
                Description = "Designed by Frank Guille for top end British maker Austinsuite. Elegant lines, sculpted full length pulls, tapered legs and differing drawer sizes combine with high quality timbers and beautiful craftmanship making this one of the most desirable British mid century tallboys you can find.",
                Price = 1234,
                Image0 = "https://raw.githubusercontent.com/bushack/images/main/tallboy_austinsuite.jpg",
                Image1 = "https://raw.githubusercontent.com/bushack/images/main/cabinet_abbess_2_.jpg",
                Image2 = "https://raw.githubusercontent.com/bushack/images/main/cabinet_abbess_3_.jpg",
                Image3 = "https://raw.githubusercontent.com/bushack/images/main/cabinet_abbess_4_.jpg",
                Image4 = "https://raw.githubusercontent.com/bushack/images/main/cabinet_abbess_5_.jpg",
                Image5 = "https://raw.githubusercontent.com/bushack/images/main/cabinet_abbess_6_.jpg"
            });

            productList.Add(new ProductModel
            {
                Id = 100006,
                Name = "Mid-Century Tallboy",
                Manufacturer = "Homeworthy Furniture",
                Description = "Quality craftmanship, curvaceous pulls, overhanging top and afromosia detailing; classic mid century styling & super desirable.",
                Price = 1234,
                Image0 = "https://raw.githubusercontent.com/bushack/images/main/tallboy_homeworthy.jpg",
                Image1 = "https://raw.githubusercontent.com/bushack/images/main/cabinet_abbess_2_.jpg",
                Image2 = "https://raw.githubusercontent.com/bushack/images/main/cabinet_abbess_3_.jpg",
                Image3 = "https://raw.githubusercontent.com/bushack/images/main/cabinet_abbess_4_.jpg",
                Image4 = "https://raw.githubusercontent.com/bushack/images/main/cabinet_abbess_5_.jpg",
                Image5 = "https://raw.githubusercontent.com/bushack/images/main/cabinet_abbess_6_.jpg"
            });

            productList.Add(new ProductModel
            {
                Id = 100007,
                Name = "Mid-Century Tallboy",
                Manufacturer = "Avalon Furniture",
                Description = "Beautifully grained, rich honey coloured timbers, sculpted solid afromosia pulls and shapely flaring solid beech legs; a fabulous, classic design from Avalon.",
                Price = 1234,
                Image0 = "https://raw.githubusercontent.com/bushack/images/main/tallboy_avalon.jpg",
                Image1 = "https://raw.githubusercontent.com/bushack/images/main/cabinet_abbess_2_.jpg",
                Image2 = "https://raw.githubusercontent.com/bushack/images/main/cabinet_abbess_3_.jpg",
                Image3 = "https://raw.githubusercontent.com/bushack/images/main/cabinet_abbess_4_.jpg",
                Image4 = "https://raw.githubusercontent.com/bushack/images/main/cabinet_abbess_5_.jpg",
                Image5 = "https://raw.githubusercontent.com/bushack/images/main/cabinet_abbess_6_.jpg"
            });
        }
    }
}
