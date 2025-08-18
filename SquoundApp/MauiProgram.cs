using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;

using System.Globalization;

using SquoundApp.Pages;
using SquoundApp.Services;
using SquoundApp.ViewModels;
using SquoundApp.Views;
using SquoundApp.States;

namespace SquoundApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            // Set the default culture to en-GB (UK).
            // This ensures currencies will always be displayed as
            // UK Pound Sterling (unless overridden elsewhere).
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


            // Logging
            builder.Logging.ClearProviders();
#if DEBUG
            // Output log to Console (stdout) and Debug window during development.
            builder.Logging.SetMinimumLevel(LogLevel.Debug);        // Logs Debug, Information, Warning, Error and Critical messages
            builder.Logging.AddConsole();                           // Console stdout
            builder.Logging.AddDebug();                             // For IDE debugging only
#else
            // Output log to Console (stdout) only during production (release).
            builder.Logging.SetMinimumLevel(LogLevel.Warning);      // Logs Warning, Error and Critical messages only
            builder.Logging.AddConsole();                           // Keep console logs for production(release) diagnostics
            // TODO : Optional logging to, eg. file, remote, etc.
#endif

            // Register services and view models
            // This is where you register your services and view models with the dependency injection container.
            // This allows you to inject them into your pages and view models.
            // For example, ItemService is registered as a singleton service,
            // which means there will be only one instance of ItemService throughout the app.
            // RefinedSearchViewModel is registered as a singleton view model,
            // which means there will be only one instance of RefinedSearchViewModel throughout the app.
            // ItemViewModel is registered as a transient view model,
            // which means a new instance will be created each time it is requested.

            // Pages
            builder.Services.AddTransient<AboutPage>();
            builder.Services.AddSingleton<HomePage>();
            builder.Services.AddTransient<ItemPage>();
            builder.Services.AddSingleton<CoarseSearchPage>();
            builder.Services.AddSingleton<RefinedSearchPage>();
            builder.Services.AddTransient<SellPage>();

            // Services
            builder.Services.AddSingleton<AboutService>();
            builder.Services.AddSingleton<CategoryService>();
            builder.Services.AddSingleton<HttpService>();
            builder.Services.AddSingleton<ItemService>();
            builder.Services.AddSingleton<NavigationService>();
            builder.Services.AddSingleton<SearchService>();
            builder.Services.AddSingleton<SellService>();

            // Views
            builder.Services.AddSingleton<AdvancedHeaderView>();
            builder.Services.AddSingleton<BasicHeaderView>();
            builder.Services.AddSingleton<FooterView>();
            builder.Services.AddSingleton<SearchHeadingView>();
            builder.Services.AddSingleton<QuickSearchView>();
            builder.Services.AddSingleton<SortAndFilterView>();

            // View Models
            builder.Services.AddTransient<AboutViewModel>();
            builder.Services.AddSingleton<CoarseSearchViewModel>();
            builder.Services.AddSingleton<FooterViewModel>();
            builder.Services.AddTransient<ItemViewModel>();
            builder.Services.AddSingleton<RefinedSearchViewModel>();
            builder.Services.AddSingleton<QuickSearchViewModel>();
            builder.Services.AddTransient<SellViewModel>();
            builder.Services.AddSingleton<SortAndFilterViewModel>();
            builder.Services.AddSingleton<StartupViewModel>();

            return builder.Build();
        }
    }
}
