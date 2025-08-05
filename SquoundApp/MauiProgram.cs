using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;

using System.Globalization;
using System.Threading;

using SquoundApp.Pages;
using SquoundApp.Services;
using SquoundApp.Utilities;
using SquoundApp.ViewModels;
using SquoundApp.Views;

namespace SquoundApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            // Set the default culture to en-GB (UK).
            // This ensures currencies will always be displayed as
            // UK Pound Sterling (unless overridded elsewhere).
            var culture = new CultureInfo("en-GB");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;

            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("BarlowSemiCondensed-Regular.ttf", "HeadlineFont");
                    fonts.AddFont("PublicSans-SemiBold.ttf", "RegularFont");
                });

            // Register services and view models
            // This is where you register your services and view models with the dependency injection container.
            // This allows you to inject them into your pages and view models.
            // For example, ProductService is registered as a singleton service,
            // which means there will be only one instance of ProductService throughout the app.
            // ProductSearchViewModel is registered as a singleton view model,
            // which means there will be only one instance of ProductSearchViewModel throughout the app.
            // ProductListingViewModel is registered as a transient view model,
            // which means a new instance will be created each time it is requested.

            // Pages
            builder.Services.AddTransient<AboutPage>();
            builder.Services.AddSingleton<HomePage>();
            builder.Services.AddTransient<ProductListingPage>();
            builder.Services.AddSingleton<CoarseSearchPage>();
            builder.Services.AddSingleton<RefinedSearchPage>();
            builder.Services.AddTransient<SellPage>();

            // Services
            builder.Services.AddSingleton<AboutService>();
            builder.Services.AddSingleton<CategoryService>();
            builder.Services.AddSingleton<HttpService>();
            builder.Services.AddSingleton<ProductService>();
            builder.Services.AddSingleton<SearchService>();
            builder.Services.AddSingleton<SellService>();

            // Views
            builder.Services.AddSingleton<AdvancedHeaderView>();
            builder.Services.AddSingleton<BasicHeaderView>();
            builder.Services.AddSingleton<FooterView>();
            builder.Services.AddSingleton<SearchHeadingView>();
            builder.Services.AddSingleton<SearchCategoriesView>();
            builder.Services.AddSingleton<SortAndFilterView>();

            // View Models
            builder.Services.AddTransient<AboutViewModel>();
            builder.Services.AddSingleton<FooterViewModel>();
            builder.Services.AddSingleton<CoarseSearchViewModel>();
            builder.Services.AddSingleton<RefinedSearchViewModel>();
            builder.Services.AddTransient<ProductListingViewModel>();
            builder.Services.AddTransient<SellViewModel>();
            builder.Services.AddSingleton<SortAndFilterViewModel>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
