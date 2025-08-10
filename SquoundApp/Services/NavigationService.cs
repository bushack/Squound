using System.Diagnostics;


namespace SquoundApp.Services
{
    public class NavigationService
    {
        private readonly SemaphoreSlim navigationLock = new(1, 1);
        private readonly List<string> navigationHistory = [];

        public IReadOnlyList<string> NavigationHistory => navigationHistory.AsReadOnly();


        //
        public NavigationService()
        {
            UpdateNavigationHistory();
        }


        //
        private void UpdateNavigationHistory()
        {
            var navigationStack = Shell.Current?.Navigation?.NavigationStack;
            navigationHistory.Clear();

            if (navigationStack is null)
                return;

            foreach (var page in navigationStack)
            {
                navigationHistory.Add(page?.GetType().Name ?? "Null");
            }
        }


        //
        private void LogNavigationHistory()
        {
            Debug.WriteLine("Current Navigation Stack:");

            int i = 0;
            foreach (var route in navigationHistory)
            {
                Debug.WriteLine($"[{i}]: {route}");
                i++;
            }
        }


        //
        public async Task GoToAsync(string route, bool animate = true, IDictionary<string, object>? parameters = null)
        {
            await navigationLock.WaitAsync();

            try
            {
                Debug.WriteLine($"Navigating to {route}");

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
                Debug.WriteLine($"Navigation error: {ex}");
            }

            finally
            {
                navigationLock.Release();
            }
        }


        /// <summary>
        /// Navigates to the previous page on the navigation stack.
        /// If no previous page exists, navigation will proceed to the HomePage.
        /// </summary>
        public async Task GoBackOrHomeAsync()
        {
            await navigationLock.WaitAsync();

            try
            {
                Debug.WriteLine($"Navigating back");

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
                Debug.WriteLine($"Navigation error: {ex}");
            }

            finally
            {
                navigationLock.Release();
            }
        }
    }
}
