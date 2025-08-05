using SquoundApp.Models;
using SquoundApp.Utilities;


namespace SquoundApp.Services
{
    public class AboutService(HttpService httpService)
    {
        private readonly HttpService httpService = httpService;


        public async Task<AboutModel> GetHTTP()
        {
            return await httpService.GetJsonAsync<AboutModel>(
                "https://raw.githubusercontent.com/bushack/files/refs/heads/main/about.json");
        }
    }
}
