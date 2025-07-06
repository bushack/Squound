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
    public class AboutUsService
    {
        HttpClient httpClient;

        AboutUsModel model = new();

        public AboutUsService()
        {
            httpClient = new HttpClient();

            // Set the base address for the HttpClient
            // This is useful if you are making multiple requests to the same base URL.
            // You can replace the URL with your actual API endpoint.
            // For example, if you are fetching products from a specific API, set the base address accordingly.
            // Note: Ensure that the URL is valid and accessible.
            //httpClient.BaseAddress = new Uri("https://api.example.com/");
        }

        public async Task<AboutUsModel> GetHTTP()
        {
            var response = await httpClient.GetAsync("https://raw.githubusercontent.com/bushack/files/refs/heads/main/about.json");

            if (response.IsSuccessStatusCode)
            {
                model = await response.Content.ReadFromJsonAsync<AboutUsModel>();
            }

            return model;
        }
    }
}
