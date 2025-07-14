using System.Net.Http.Json;
using System.Text.Json;

using SquoundApp.Models;
using SquoundApp.Utilities;


namespace SquoundApp.Services
{
    public class ProductService
    {
        List<ProductModel> productList = new();

        public void Clear()
        {
            // Check if the product list is already populated
            if (productList?.Count > 0)
            {
                productList.Clear();
            }
        }


        public async Task<List<ProductModel>?> GetProductsApi()
        {
            if (productList?.Count > 0)
            {
                return productList;
            }

            // For the release version of the project we will set the base address for the HttpService
            // This is useful if you are making multiple requests to the same base URL.
            // For example "https://squound.azure.net/api/products/";
            var httpService = ServiceLocator.GetService<HttpService>();

            // URL of debug REST service (Android does not support https://localhost:5001)
            // This URL is used for debugging purposes on Android devices or emulators.
            string LocalHostUrl = DeviceInfo.Platform == DevicePlatform.Android ? "10.0.2.2" : "localhost";
            string Scheme = "https";
            string Port = "7184";
            string RestUrl = $"{Scheme}://{LocalHostUrl}:{Port}/api/productmodels/";

            var response = await httpService.GetJsonAsync<List<ProductModel>>(RestUrl);

            if (response != null)
            {
                productList = response;
            }

            return productList;
        }

        public async Task<List<ProductModel>?> GetProductsHttp()
        {
            if (productList?.Count > 0)
                return productList;

            var httpService = ServiceLocator.GetService<HttpService>();

            var response = await httpService.GetJsonAsync<List<ProductModel>>("https://raw.githubusercontent.com/bushack/files/refs/heads/main/products.json");

            if (response != null)
            {
                productList = response;
            }

            return productList;
        }

        public async Task<List<ProductModel>?> GetProductsEmbedded()
        {
            if (productList?.Count > 0)
                return productList;

            // Else read from embedded JSON file
            // See https://www.youtube.com/watch?v=DuNLR_NJv8U
            using var stream = await FileSystem.OpenAppPackageFileAsync("Resources/Raw/products.json");
            using var reader = new StreamReader(stream);
            var contents = await reader.ReadToEndAsync();
            var products = JsonSerializer.Deserialize<List<ProductModel>>(contents);

            if (products != null)
            {
                productList = products;
            }

            return productList;
        }
    }
}
