using SquoundApp.Models;


namespace SquoundApp.Services
{
    public class SellService(HttpService httpService)
    {
        private readonly HttpService httpService = httpService;

        public async Task<SellModel> GetDataAsync()
        {
            var response = await httpService.GetJsonAsync<SellModel>(
                "https://raw.githubusercontent.com/bushack/files/refs/heads/main/sell.json");

            // Either return the retrieved data or a default model.
            // TODO : Must populate the default model with appropriate information.
            return response.Data ?? new SellModel();
        }
    }
}
