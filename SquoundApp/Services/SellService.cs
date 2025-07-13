using SquoundApp.Models;
using SquoundApp.Utilities;

namespace SquoundApp.Services
{
    public class SellService
    {
        SellModel model = new();

        public async Task<SellModel> GetHTTP()
        {
            var httpService = ServiceLocator.GetService<HttpService>();

            return await httpService.GetJsonAsync<SellModel>(
                "https://raw.githubusercontent.com/bushack/files/refs/heads/main/sell.json");
        }
    }
}
