using Shared.Logging;


namespace SquoundApp.Interfaces
{
    public interface IHttpService
    {
        public Task<Result<T?>> GetJsonAsync<T>(string url);
    }
}
