using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Json;
using System.Text.Json;
using SquoundApp_v1.Models;

namespace SquoundApp_v1.Services
{
    public class ProductService
    {
        HttpClient httpClient;

        List<ProductModel> productList = new();

        public ProductService()
        {
            httpClient = new HttpClient();

            // Set the base address for the HttpClient
            // This is useful if you are making multiple requests to the same base URL.
            // You can replace the URL with your actual API endpoint.
            // For example, if you are fetching products from a specific API, set the base address accordingly.
            // Note: Ensure that the URL is valid and accessible.
            //httpClient.BaseAddress = new Uri("https://api.example.com/");
        }

        public async Task<List<ProductModel>> Clear()
        {
            // Check if the product list is already populated
            if (productList?.Count > 0)
                productList.Clear();

            return productList;
        }

        public async Task<List<ProductModel>> GetProductsHTTP()
        {
            if (productList?.Count > 0)
                return productList;

            // Else
            var response = await httpClient.GetAsync("https://raw.githubusercontent.com/bushack/files/refs/heads/main/products.json");

            if (response.IsSuccessStatusCode)
            {
                productList = await response.Content.ReadFromJsonAsync<List<ProductModel>>();
            }

            return productList;
        }

        public async Task<List<ProductModel>> GetProductsEmbedded()
        {
            if (productList?.Count > 0)
                return productList;

            // Else read from embedded JSON file
            // See https://www.youtube.com/watch?v=DuNLR_NJv8U
            using var stream = await FileSystem.OpenAppPackageFileAsync("Resources/Raw/products.json");
            using var reader = new StreamReader(stream);
            var contents = await reader.ReadToEndAsync();
            productList = JsonSerializer.Deserialize<List<ProductModel>>(contents);

            return productList;
        }
    }
}
