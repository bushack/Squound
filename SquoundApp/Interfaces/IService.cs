

namespace SquoundApp.Interfaces
{
    public interface IService<T>
    {
        Task<List<T>> GetDataAsync();
    }
}
