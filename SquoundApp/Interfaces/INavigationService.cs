

namespace SquoundApp.Interfaces
{
    public interface INavigationService
    {
        /// <summary>
        /// Exposes a read-only list of strings representing the navigation history.
        /// </summary>
        public IReadOnlyList<string> NavigationHistory { get; }


        /// <summary>
        /// Compares the provided route with the current page's route to determine if they match.
        /// </summary>
        public bool IsCurrentPage(string route);


        /// <summary>
        /// Navigates to a specified route within the application.
        /// </summary>
        /// <param name="route">The route to navigate to, which should correspond to a registered route in the Shell.</param>
        /// <param name="animate">Whether to animate the navigation transition. Default is true.</param>
        /// <param name="parameters">An optional dictionary of parameters to pass to the target page.</param>
        public Task GoToAsync(string route, bool animate = true, IDictionary<string, object>? parameters = null);


        /// <summary>
        /// Navigates back to the previous page in the navigation stack if possible;
        /// If a backward navigation is not possible, navigates to the home page instead.
        /// </summary>
        public Task GoBackOrHomeAsync();
    }
}
