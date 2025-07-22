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
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddTransient<ProductListingPage>();
            builder.Services.AddSingleton<ProductSearchPage>();
            builder.Services.AddTransient<SellPage>();

            // Services
            builder.Services.AddSingleton<AboutService>();
            builder.Services.AddSingleton<HttpService>();
            builder.Services.AddSingleton<ProductService>();
            builder.Services.AddSingleton<SellService>();

            // Views
            builder.Services.AddSingleton<BasicHeaderView>();
            builder.Services.AddSingleton<HeaderView>();
            builder.Services.AddSingleton<FooterView>();
            builder.Services.AddSingleton<ProductCategoriesView>();
            builder.Services.AddSingleton<ProductFilterView>();

            // View Models
            builder.Services.AddTransient<AboutViewModel>();
            builder.Services.AddSingleton<CategoriesViewModel>();
            builder.Services.AddSingleton<FooterViewModel>();
            builder.Services.AddSingleton<ProductFilterViewModel>();
            builder.Services.AddTransient<ProductListingViewModel>();
            builder.Services.AddSingleton<ProductSearchViewModel>();
            builder.Services.AddTransient<SellViewModel>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
