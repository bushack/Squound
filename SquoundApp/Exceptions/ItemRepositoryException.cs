

namespace SquoundApp.Exceptions
{
    public class ItemRepositoryException : Exception
    {
        public ItemRepositoryException(string message) : base(message) { }

        public ItemRepositoryException(string message, Exception inner) : base(message, inner) { }
    }
}
