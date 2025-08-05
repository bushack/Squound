using SquoundApp.Models;
using SquoundApp.Utilities;


namespace SquoundApp.Services
{
    public class SellService(HttpService httpService)
    {
        private readonly HttpService httpService = httpService;

        public async Task<SellModel> GetHTTP()
        {
            return await httpService.GetJsonAsync<SellModel>(
                "https://raw.githubusercontent.com/bushack/files/refs/heads/main/sell.json");
        }
    }
}
