using SquoundApp.Pages;


namespace SquoundApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // Register routes for navigation
            // This allows you to navigate to specific pages using a route name.
            // For example, you can navigate to ItemPage using the route name "ItemPage".
            // The route name should match the name of the page class.
            // nameof(ItemPage) == "ItemPage"
            Routing.RegisterRoute(nameof(AboutPage), typeof(AboutPage));
            Routing.RegisterRoute(nameof(HomePage), typeof(HomePage));
            Routing.RegisterRoute(nameof(ItemPage), typeof(ItemPage));
            Routing.RegisterRoute(nameof(SellPage), typeof(SellPage));
            Routing.RegisterRoute(nameof(CoarseSearchPage), typeof(CoarseSearchPage));
            Routing.RegisterRoute(nameof(RefinedSearchPage), typeof(RefinedSearchPage));
        }
    }
}
