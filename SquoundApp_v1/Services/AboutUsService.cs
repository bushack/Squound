using SquoundApp_v1.Models;
using SquoundApp_v1.Utilities;

namespace SquoundApp_v1.Services
{
    public class AboutUsService
    {
        AboutUsModel model = new();

        public async Task<AboutUsModel> GetHTTP()
        {
            var httpService = ServiceLocator.GetService<HttpService>();

            return await httpService.GetJsonAsync<AboutUsModel>(
                "https://raw.githubusercontent.com/bushack/files/refs/heads/main/about.json");
        }
    }
}
