using Microsoft.Extensions.Logging;

using SquoundApp.Interfaces;
using SquoundApp.Pages;


namespace SquoundApp.Services
{
    public class NavigationService : INavigationService
    {
        private readonly ILogger<NavigationService> _Logger;
        private readonly SemaphoreSlim _NavigationLock = new(1, 1);
        private readonly List<string> _NavigationHistory = [];

        public IReadOnlyList<string> NavigationHistory => _NavigationHistory.AsReadOnly();


        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="logger"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public NavigationService(ILogger<NavigationService> logger)
        {
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));

            UpdateNavigationHistory();
        }


        /// <summary>
        /// Updates the internal navigation history list to reflect the current state of the Shell's navigation stack.
        /// </summary>
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


        /// <summary>
        /// Logs the current navigation history to the debug output.
        /// </summary>
        /// <remarks>Each entry in the navigation history is logged with its index and route.  This method
        /// is intended for debugging purposes and provides insight into the current state of the navigation
        /// stack.</remarks>
        private void LogNavigationHistory()
        {
            _Logger.LogDebug("Current Navigation Stack:");

            int i = 0;
            foreach (var route in _NavigationHistory)
            {
                _Logger.LogDebug("[{i++}]: {route}", i, route);
            }
        }


        /// <summary>
        /// Compares the provided route with the current page's route to determine if they match.
        /// </summary>
        public bool IsCurrentPage(string route)
        {
            var current = Shell.Current?.CurrentPage;

            if (current is null)
            {
                return false;
            }

            return current.Title.Equals(route, StringComparison.OrdinalIgnoreCase);
        }


        /// <summary>
        /// Navigates to a specified route within the application.
        /// </summary>
        /// <param name="route">The route to navigate to, which should correspond to a registered route in the Shell.</param>
        /// <param name="animate">Whether to animate the navigation transition. Default is true.</param>
        /// <param name="parameters">An optional dictionary of parameters to pass to the target page.</param>
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
        /// Navigates back to the previous page in the navigation stack if possible;
        /// If a backward navigation is not possible, navigates to the home page instead.
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
                    await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
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