using SquoundApp.Pages;


namespace SquoundApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // *** IMPORTANT ***
            // Do not register any pages as routes if they are defined in AppShell.xaml as <ShellContent>.
            // Doing so would result in an ambiguous route because the pages would be registered twice.

            // Register routes for navigation.
            // This allows you to navigate to specific pages using a route name.
            // For example 'Shell.Current.GoToAsync(nameof(ItemPage));'
            Routing.RegisterRoute(nameof(AboutPage), typeof(AboutPage));
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(ItemSummaryPage), typeof(ItemSummaryPage));
            Routing.RegisterRoute(nameof(SellPage), typeof(SellPage));

            // The following pages are registered as <ShellContent> and therefore should NOT be re-registered here.
            // Routing.RegisterRoute(nameof(HomePage), typeof(HomePage));
            // Routing.RegisterRoute(nameof(CoarseSearchPage), typeof(CoarseSearchPage));
        }
    }
}
