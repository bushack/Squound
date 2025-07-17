namespace SquoundApp.Utilities
{
    public static class ServiceLocator
    {
        /// <summary>
        /// Provides access to the application's services.
        /// </summary>
        /// <typeparam name="T">The service type required.</typeparam>
        /// <returns>The service object requested.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static T GetService<T>() where T : class
        {
            return (Application.Current as App)?
                .Handler?.MauiContext?.Services
                .GetService<T>()
                ?? throw new InvalidOperationException($"Service {typeof(T)} not found.");
        }
    }
}