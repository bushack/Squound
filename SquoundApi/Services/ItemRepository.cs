using SquoundApi.Interfaces;
using SquoundApi.Models;


namespace SquoundApi.Services
{
    public class ItemRepository : IItemRepository
    {
        private readonly List<ItemModel> itemList = [];

        public ItemRepository()
        {
            //InitializeData();
        }

        public bool DoesItemExist(long id)
        {
            return itemList.Any(item => item.ItemId == id);
        }

        public IEnumerable<ItemModel> All
        {
            get
            {
                return itemList;
            }
        }
        public IEnumerable<ItemModel> Get(long id)
        {
            var item = this.Find(id);

            if (item == null)
            {
                // If no item is found, return an empty enumerable.
                return Enumerable.Empty<ItemModel>();
            }

            // Return a list containing the single found item.
            return new List<ItemModel> { item };
        }

        public ItemModel? Find(long id)
        {
            return itemList.FirstOrDefault(item => item.ItemId == id);
        }

        public void Insert(ItemModel item)
        {
            itemList.Add(item);
        }

        public void Update(ItemModel item)
        {
            var itemToUpdate = this.Find(item.ItemId);

            if (itemToUpdate != null)
            {
                var index = itemList.IndexOf(itemToUpdate);

                itemList.RemoveAt(index);
                itemList.Insert(index, item);
            }
        }

        public void Delete(long id)
        {
            var productToDelete = this.Find(id);

            if (productToDelete != null)
            {
                itemList.Remove(productToDelete);
            }
        }

        //private void InitializeData()
        //{
        //    productList.Add(new ProductModel
        //    {
        //        ProductId = 100000,
        //        Name = "1970s Teak Cabinet",
        //        Manufacturer = "Abbess",
        //        Description = "Teak sliding doors with inset aluminium circular pulls, robust metal legs and adjustable internal shelf. Versatile and charming.",
        //        Quantity = 1,
        //        Price = 1020.30m,
        //        CategoryId = 0,
        //        //Image0 = "https://raw.githubusercontent.com/bushack/images/main/cabinet_abbess_1_.jpg",
        //        //Image1 = "https://raw.githubusercontent.com/bushack/images/main/cabinet_abbess_2_.jpg",
        //        //Image2 = "https://raw.githubusercontent.com/bushack/images/main/cabinet_abbess_3_.jpg",
        //        //Image3 = "https://raw.githubusercontent.com/bushack/images/main/cabinet_abbess_4_.jpg",
        //        //Image4 = "https://raw.githubusercontent.com/bushack/images/main/cabinet_abbess_5_.jpg",
        //        //Image5 = "https://raw.githubusercontent.com/bushack/images/main/cabinet_abbess_6_.jpg"
        //    });

        //    productList.Add(new ProductModel
        //    {
        //        ProductId = 100001,
        //        Name = "Mid-Century Tallboy",
        //        Manufacturer = "Wrighton Furniture",
        //        Description = "Elegant styling, circular pulls, sculptural leg detailing and beautiful timbers make this an especially unusual & desirable piece.",
        //        Quantity = 1,
        //        Price = 6789.99m,
        //        CategoryId = 0,
        //        //Image0 = "https://raw.githubusercontent.com/bushack/images/main/mid_century_tallboy_wrighton.jpg",
        //        //Image1 = "https://raw.githubusercontent.com/bushack/images/main/cabinet_abbess_2_.jpg",
        //        //Image2 = "https://raw.githubusercontent.com/bushack/images/main/cabinet_abbess_3_.jpg",
        //        //Image3 = "https://raw.githubusercontent.com/bushack/images/main/cabinet_abbess_4_.jpg",
        //        //Image4 = "https://raw.githubusercontent.com/bushack/images/main/cabinet_abbess_5_.jpg",
        //        //Image5 = "https://raw.githubusercontent.com/bushack/images/main/cabinet_abbess_6_.jpg"
        //    });

        //    productList.Add(new ProductModel
        //    {
        //        ProductId = 100002,
        //        Name = "1960s Tallboy",
        //        Manufacturer = "Austinsuite",
        //        Description = "Designed by Frank Guille for top end British maker Austinsuite. Elegant lines, sculpted full length pulls, tapered legs and differing drawer sizes combine with high quality timbers and beautiful craftmanship making this one of the most desirable British mid century tallboys you can find.",
        //        Quantity = 1,
        //        Price = 2345.67m,
        //        CategoryId = 0,
        //        //Image0 = "https://raw.githubusercontent.com/bushack/images/main/tallboy_austinsuite.jpg",
        //        //Image1 = "https://raw.githubusercontent.com/bushack/images/main/cabinet_abbess_2_.jpg",
        //        //Image2 = "https://raw.githubusercontent.com/bushack/images/main/cabinet_abbess_3_.jpg",
        //        //Image3 = "https://raw.githubusercontent.com/bushack/images/main/cabinet_abbess_4_.jpg",
        //        //Image4 = "https://raw.githubusercontent.com/bushack/images/main/cabinet_abbess_5_.jpg",
        //        //Image5 = "https://raw.githubusercontent.com/bushack/images/main/cabinet_abbess_6_.jpg"
        //    });

        //    productList.Add(new ProductModel
        //    {
        //        ProductId = 100006,
        //        Name = "Mid-Century Tallboy",
        //        Manufacturer = "Homeworthy Furniture",
        //        Description = "Quality craftmanship, curvaceous pulls, overhanging top and afromosia detailing; classic mid century styling & super desirable.",
        //        Quantity = 1,
        //        Price = 3456.78m,
        //        CategoryId = 0,
        //        //Image0 = "https://raw.githubusercontent.com/bushack/images/main/tallboy_homeworthy.jpg",
        //        //Image1 = "https://raw.githubusercontent.com/bushack/images/main/cabinet_abbess_2_.jpg",
        //        //Image2 = "https://raw.githubusercontent.com/bushack/images/main/cabinet_abbess_3_.jpg",
        //        //Image3 = "https://raw.githubusercontent.com/bushack/images/main/cabinet_abbess_4_.jpg",
        //        //Image4 = "https://raw.githubusercontent.com/bushack/images/main/cabinet_abbess_5_.jpg",
        //        //Image5 = "https://raw.githubusercontent.com/bushack/images/main/cabinet_abbess_6_.jpg"
        //    });

        //    productList.Add(new ProductModel
        //    {
        //        ProductId = 100007,
        //        Name = "Mid-Century Tallboy",
        //        Manufacturer = "Avalon Furniture",
        //        Description = "Beautifully grained, rich honey coloured timbers, sculpted solid afromosia pulls and shapely flaring solid beech legs; a fabulous, classic design from Avalon.",
        //        Quantity = 1,
        //        Price = 4567.89m,
        //        CategoryId = 0,
        //        //Image0 = "https://raw.githubusercontent.com/bushack/images/main/tallboy_avalon.jpg",
        //        //Image1 = "https://raw.githubusercontent.com/bushack/images/main/cabinet_abbess_2_.jpg",
        //        //Image2 = "https://raw.githubusercontent.com/bushack/images/main/cabinet_abbess_3_.jpg",
        //        //Image3 = "https://raw.githubusercontent.com/bushack/images/main/cabinet_abbess_4_.jpg",
        //        //Image4 = "https://raw.githubusercontent.com/bushack/images/main/cabinet_abbess_5_.jpg",
        //        //Image5 = "https://raw.githubusercontent.com/bushack/images/main/cabinet_abbess_6_.jpg"
        //    });
        //}
    }
}
