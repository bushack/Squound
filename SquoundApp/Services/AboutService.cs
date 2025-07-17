using SquoundApp.Models;
using SquoundApp.Utilities;


namespace SquoundApp.Services
{
    public class AboutService
    {
        readonly AboutModel model = new();

        public async Task<AboutModel> GetHTTP()
        {
            var httpService = ServiceLocator.GetService<HttpService>();

            return await httpService.GetJsonAsync<AboutModel>(
                "https://raw.githubusercontent.com/bushack/files/refs/heads/main/about.json");
        }
    }
}
