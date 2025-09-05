using Microsoft.Extensions.Logging;

using System.Net.Http.Json;

using SquoundApp.Exceptions;
using SquoundApp.Interfaces;

using Shared.Logging;


namespace SquoundApp.Services
{
    public class HttpService : IHttpService
    {
        private readonly ILogger<HttpService> _Logger;
        protected readonly HttpClient _HttpClient;

        public HttpService(ILogger<HttpService> logger, HttpClient? client = null, string? baseUrl = null)
        {
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _HttpClient = client ?? new HttpClient();

            // Set the base address for the httpClient
            // This is useful if you are making multiple requests to the same base URL.
            // Ensure that the URL is valid and accessible.
            if (baseUrl is not null)
            {
                _HttpClient.BaseAddress = new Uri(baseUrl);
            }
        }

        public async Task<Result<T?>> GetJsonAsync<T>(string url)
        {
            try
            {
                _Logger.LogInformation("Attempting to retrieve JSON content from {url}", url);

                // Asynchronously request data from url.
                var response = await _HttpClient.GetAsync(url);

                // Check success.
                if (response.IsSuccessStatusCode is false)
                    throw new HttpServiceException($"Server returned status code {response.StatusCode}");

                // Check content.
                var content = await response.Content.ReadFromJsonAsync<T>()
                    ?? throw new HttpServiceException("Failed to deserialize JSON content.");

                _Logger.LogInformation("Successfully retrieved and deserialized JSON from {url}.", url);

                return Result<T?>.Ok(content);
            }

            catch (HttpServiceException ex)
            {
                _Logger.LogWarning(ex, "Service error while retrieving JSON content from {url}.", url);

                return Result<T?>.Fail("An invalid response was received while retrieving data from the server.");
            }

            catch (Exception ex)
            {
                _Logger.LogWarning(ex, "Error retrieving JSON from {url}.", url);

                return Result<T?>.Fail("An error occurred while retrieving data from the server.");
            }
        }
    }
}