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
            // For example, you can navigate to ProductListingPage using the route name "ProductListingPage".
            // The route name should match the name of the page class.
            // nameof(ProductListingPage) == "ProductListingPage"
            Routing.RegisterRoute(nameof(AboutPage), typeof(AboutPage));
            Routing.RegisterRoute(nameof(CoarseSearchPage), typeof(CoarseSearchPage));
            Routing.RegisterRoute(nameof(HomePage), typeof(HomePage));
            Routing.RegisterRoute(nameof(ProductListingPage), typeof(ProductListingPage));
            Routing.RegisterRoute(nameof(RefinedSearchPage), typeof(RefinedSearchPage));
            Routing.RegisterRoute(nameof(SellPage), typeof(SellPage));
        }
    }
}
