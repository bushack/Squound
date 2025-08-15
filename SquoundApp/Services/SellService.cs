using Microsoft.Extensions.Logging;

using SquoundApp.Models;

using Shared.Logging;


namespace SquoundApp.Services
{
    public class SellService(HttpService httpService, ILogger<SellService> logger)
    {
        private readonly ILogger<SellService> _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        private readonly HttpService _HttpService = httpService ?? throw new ArgumentNullException(nameof(httpService));

        private readonly string _Url = "https://raw.githubusercontent.com/bushack/files/refs/heads/main/sell.json";

        public async Task<Result<SellModel>> GetDataAsync()
        {
            _Logger.LogInformation("Retrieving data from {_Url}", _Url);

            try
            {
                var response = await _HttpService.GetJsonAsync<SellModel>(_Url)
                    ?? throw new Exception($"Attempt to retrieve JSON content from {_Url} failed.");

                var data = response.Data
                    ?? throw new Exception($"JSON content retrieved from {_Url} is null.");

                return Result<SellModel>.Ok(data);
            }

            catch (Exception ex)
            {
                _Logger.LogWarning(ex, "Undefined error from server.");

                return Result<SellModel>.Fail("Undefined error from server.");
            }
        }
    }
}
