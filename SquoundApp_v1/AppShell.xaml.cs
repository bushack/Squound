using SquoundApp_v1.Pages;


namespace SquoundApp_v1
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // Register routes for navigation
            // This allows you to navigate to specific pages using a route name.
            // For example, you can navigate to ProductDetailPage using the route name "ProductDetailPage".
            // The route name should match the name of the page class.
            // nameof(ProductDetailPage) == "ProductDetailPage"
            Routing.RegisterRoute(nameof(ProductSearchPage), typeof(ProductSearchPage));
            Routing.RegisterRoute(nameof(ProductDetailPage), typeof(ProductDetailPage));
        }
    }
}
