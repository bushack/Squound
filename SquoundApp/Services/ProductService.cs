using System.Text.Json;

using SquoundApp.Utilities;

using Shared.DataTransfer;


namespace SquoundApp.Services
{
    public class ProductService
    {
        private HttpService httpService;

        List<ProductDto> productList = new();

        public ProductService(HttpService httpService)
        {
            this.httpService = httpService;
        }

        public void Clear()
        {
            // Check if the product list is already populated
            if (productList.Count > 0)
            {
                productList.Clear();
            }
        }

        /// <summary>
        /// Asynchronously retrieves a list of products from a REST API.
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public async Task<List<ProductDto>?> GetProductsRestApi(string? category = null)
        {
            if (productList.Count > 0)
            {
                return productList;
            }

            // For the release version of the project we will set the base address for the HttpService
            // This is useful if you are making multiple requests to the same base URL.
            // For example "https://squound.azure.net/api/products/";

            // URL of debug REST service (Android does not support https://localhost:5001)
            // This URL is used for debugging purposes on Android devices or emulators.
            string LocalHostUrl = DeviceInfo.Platform == DevicePlatform.Android ? "192.168.1.114" : "localhost";
            string Scheme = DeviceInfo.Platform == DevicePlatform.Android ? "http" : "https";
            string Port = DeviceInfo.Platform == DevicePlatform.Android ? "5050" : "7184";
            //string RestUrl = $"{Scheme}://{LocalHostUrl}:{Port}/api/products/search?category=None&manufacturer=Austinsuite&sortby=PriceDesc&pagenumber=1&pagesize=10";
            //string RestUrl = $"{Scheme}://{LocalHostUrl}:{Port}/api/products/search?category=Lighting&sortby=PriceAsc&pagenumber=1&pagesize=10";
            //string RestUrl = $"{Scheme}://{LocalHostUrl}:{Port}/api/products/12";
            //string RestUrl = $"{Scheme}://{LocalHostUrl}:{Port}/api/products/all";

            string RestUrl = "";
            if (category != null)
            {
                RestUrl = $"{Scheme}://{LocalHostUrl}:{Port}/api/products/search?category={category}&sortby=PriceAsc&pagenumber=1&pagesize=10";
            }

            else
            {
                RestUrl = $"{Scheme}://{LocalHostUrl}:{Port}/api/products/all";
            }

            var response = await httpService.GetJsonAsync<List<ProductDto>>(RestUrl);

            if (response != null)
            {
                productList = response;
            }

            return productList;
        }

        /// <summary>
        /// Asynchronously retrieves a list of products from a remote JSON file.
        /// </summary>
        /// <param name="url">URL of the file to read.
        /// Example: "https://raw.githubusercontent.com/bushack/files/refs/heads/main/products.json"</param>
        /// <returns></returns>
        public async Task<List<ProductDto>?> GetProductsRemoteJson(string url)
        {
            if (productList?.Count > 0)
                return productList;

            var response = await httpService.GetJsonAsync<List<ProductDto>>(url);

            if (response != null)
            {
                productList = response;
            }

            return productList;
        }

        /// <summary>
        /// Asynchronously retrieves a list of products from an embedded JSON file.
        /// </summary>
        /// <param name="filename">Relative filepath of the file to read.
        /// Example: "Resources/Raw/products.json"</param>
        /// <returns></returns>
        public async Task<List<ProductDto>?> GetProductsEmbeddedJson(string filename)
        {
            if (productList?.Count > 0)
                return productList;

            // Else read from embedded JSON file
            // See https://www.youtube.com/watch?v=DuNLR_NJv8U
            using var stream = await FileSystem.OpenAppPackageFileAsync(filename);
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
