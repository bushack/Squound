using Microsoft.Extensions.Logging;

using SquoundApp.Models;

using Shared.Logging;


namespace SquoundApp.Services
{
    public class AboutService(HttpService httpService, ILogger<AboutService> logger)
    {
        private readonly ILogger<AboutService> _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        private readonly HttpService _HttpService = httpService ?? throw new ArgumentNullException(nameof(httpService));

        private readonly string _Url = "https://raw.githubusercontent.com/bushack/files/refs/heads/main/about.json";


        public async Task<Result<AboutModel>> GetDataAsync()
        {
            _Logger.LogInformation("Retrieving data from {_Url}", _Url);

            try
            {
                var response = await _HttpService.GetJsonAsync<AboutModel>(_Url)
                    ?? throw new Exception($"Attempt to retrieve JSON content from {_Url} failed.");

                var data = response.Data
                    ?? throw new Exception($"JSON content retrieved from {_Url} is null.");

                return Result<AboutModel>.Ok(data);
            }

            catch (Exception ex)
            {
                _Logger.LogWarning(ex, "Undefined error from server.");

                return Result<AboutModel>.Fail("Undefined error from server.");
            }
        }
    }
}