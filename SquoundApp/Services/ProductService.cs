using System.Text.Json;

using SquoundApp.Utilities;

using Shared.DataTransfer;


namespace SquoundApp.Services
{
    public class ProductService
    {
        List<ProductDto> productList = new();

        public void Clear()
        {
            // Check if the product list is already populated
            if (productList.Count > 0)
            {
                productList.Clear();
            }
        }


        public async Task<List<ProductDto>?> GetProductsApi()
        {
            if (productList.Count > 0)
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
            string RestUrl = $"{Scheme}://{LocalHostUrl}:{Port}/api/products/";

            var response = await httpService.GetJsonAsync<List<ProductDto>>(RestUrl);

            if (response != null)
            {
                productList = response;
            }

            return productList;
        }

        public async Task<List<ProductDto>?> GetProductsHttp()
        {
            if (productList?.Count > 0)
                return productList;

            var httpService = ServiceLocator.GetService<HttpService>();

            var response = await httpService.GetJsonAsync<List<ProductDto>>("https://raw.githubusercontent.com/bushack/files/refs/heads/main/products.json");

            if (response != null)
            {
                productList = response;
            }

            return productList;
        }

        public async Task<List<ProductDto>?> GetProductsEmbedded()
        {
            if (productList?.Count > 0)
                return productList;

            // Else read from embedded JSON file
            // See https://www.youtube.com/watch?v=DuNLR_NJv8U
            using var stream = await FileSystem.OpenAppPackageFileAsync("Resources/Raw/products.json");
            using var reader = new StreamReader(stream);
            var contents = await reader.ReadToEndAsync();
            var products = JsonSerializer.Deserialize<List<ProductDto>>(contents);

            if (products != null)
            {
                productList = products;
            }

            return productList;
        }
    }
}
