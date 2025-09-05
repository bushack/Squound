using Shared.Logging;


namespace SquoundApp.Interfaces
{
    public interface IApiService<T>
    {
        Task<Result<List<T>>> GetDataAsync();
    }
}
