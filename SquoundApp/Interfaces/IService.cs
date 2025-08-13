using Shared.Logging;


namespace SquoundApp.Interfaces
{
    public interface IService<T>
    {
        Task<Result<List<T>>> GetDataAsync();
    }
}
