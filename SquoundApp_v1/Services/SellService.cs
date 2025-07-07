using SquoundApp_v1.Models;
using SquoundApp_v1.Utilities;

namespace SquoundApp_v1.Services
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
