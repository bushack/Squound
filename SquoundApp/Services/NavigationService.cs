using Microsoft.Extensions.Logging;


namespace SquoundApp.Services
{
    public class NavigationService
    {
        private readonly ILogger<NavigationService> _Logger;
        private readonly SemaphoreSlim _NavigationLock = new(1, 1);
        private readonly List<string> _NavigationHistory = [];

        public IReadOnlyList<string> NavigationHistory => _NavigationHistory.AsReadOnly();


        //
        public NavigationService(ILogger<NavigationService> logger)
        {
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));

            UpdateNavigationHistory();
        }


        //
        private void UpdateNavigationHistory()
        {
            _NavigationHistory.Clear();
            var navigationStack = Shell.Current?.Navigation?.NavigationStack;

            if (navigationStack is null)
                return;

            foreach (var page in navigationStack)
            {
                _NavigationHistory.Add(page?.GetType().Name ?? "Null");
            }
        }


        //
        private void LogNavigationHistory()
        {
            _Logger.LogDebug("Current Navigation Stack:");

            int i = 0;
            foreach (var route in _NavigationHistory)
            {
                _Logger.LogDebug("[{i++}]: {route}", i, route);
            }
        }


        //
        public async Task GoToAsync(string route, bool animate = true, IDictionary<string, object>? parameters = null)
        {
            await _NavigationLock.WaitAsync();

            try
            {
                _Logger.LogDebug("Navigating to {route}", route);

                if (parameters is not null && parameters.Count > 0)
                {
                    await Shell.Current.GoToAsync(route, animate, parameters);
                }

                else
                {
                    await Shell.Current.GoToAsync(route, animate);
                }
                
                UpdateNavigationHistory();
                LogNavigationHistory();
            }

            catch (Exception ex)
            {
                _Logger.LogWarning(ex, "Error while attempting to navigate to {route}", route);
            }

            finally
            {
                _NavigationLock.Release();
            }
        }


        /// <summary>
        /// Navigates to the previous page on the navigation stack.
        /// If no previous page exists, navigation will proceed to the HomePage.
        /// </summary>
        public async Task GoBackOrHomeAsync()
        {
            await _NavigationLock.WaitAsync();

            try
            {
                _Logger.LogDebug("Navigating back:");

                var current = Shell.Current;
                if (current is null)
                    return;

                var navigationStack = current.Navigation?.NavigationStack;
                if (navigationStack is null)
                    return;

                if (navigationStack.Count > 1)
                {
                    await Shell.Current.GoToAsync("..");
                }

                else
                {
                    await Shell.Current.GoToAsync("//HomePage");
                }

                UpdateNavigationHistory();
                LogNavigationHistory();
            }

            catch (Exception ex)
            {
                _Logger.LogWarning(ex, "Error while attempting to navigate back");
            }

            finally
            {
                _NavigationLock.Release();
            }
        }
    }
}
