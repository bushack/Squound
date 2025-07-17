using SquoundApp.Models;
using SquoundApp.Utilities;


namespace SquoundApp.Services
{
    public class AboutService
    {
        private HttpService httpService;

        readonly AboutModel model = new();

        public AboutService(HttpService httpService)
        {
            this.httpService = httpService;
        }

        public async Task<AboutModel> GetHTTP()
        {
            return await httpService.GetJsonAsync<AboutModel>(
                "https://raw.githubusercontent.com/bushack/files/refs/heads/main/about.json");
        }
    }
}
