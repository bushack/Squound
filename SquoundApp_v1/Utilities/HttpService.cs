using System.Net.Http.Json;

namespace SquoundApp_v1.Utilities
{
    class HttpService
    {
        protected readonly HttpClient httpClient;

        public HttpService(HttpClient? client = null, string? baseUrl = null)
        {
            httpClient = client ?? new HttpClient();

            // Set the base address for the httpClient
            // This is useful if you are making multiple requests to the same base URL.
            // Ensure that the URL is valid and accessible.
            if (baseUrl is not null)
                httpClient.BaseAddress = new Uri(baseUrl);
        }

        public async Task<T?> GetJsonAsync<T>(string url)
        {
            try
            {
                var response = await httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                    return default;

                return await response.Content.ReadFromJsonAsync<T>();
            }

            catch (Exception ex)
            {
                // Handle exceptions, e.g., log them or show an alert to the user.
                Console.WriteLine($"{nameof(HttpService)} Error : {ex.Message}");

                await Shell.Current.DisplayAlert(
                    $"{nameof(HttpService)} Error",
                    $"An error occurred while attempting to fetch data.",
                    "OK");

                return default;
            }
        }
    }
}
