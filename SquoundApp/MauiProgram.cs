using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using CommunityToolkit.Maui;

using System.Globalization;

using SquoundApp.Contexts;
using SquoundApp.Interfaces;
using SquoundApp.Pages;
using SquoundApp.Repositories;
using SquoundApp.Services;
using SquoundApp.ViewModels;
using SquoundApp.Views;

using Shared.Interfaces;
using Shared.Services;


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

            // Services & Contexts.
            builder.Services.AddSingleton<IHttpService, HttpService>();
            builder.Services.AddSingleton<IEventService, EventService>();
            builder.Services.AddSingleton<ICategoryService, CategoryService>();
            builder.Services.AddSingleton<ISearchContext, SearchContext>();
            builder.Services.AddSingleton<IItemService, ItemService>();
            builder.Services.AddSingleton<NavigationService>();
            builder.Services.AddSingleton<AboutService>();
            builder.Services.AddSingleton<SellService>();

            // Repositories.
            builder.Services.AddSingleton<ICategoryRepository, CategoryRepository>();

            // View Models.
            builder.Services.AddTransient<AboutViewModel>();
            builder.Services.AddTransient<CoarseSearchViewModel>();
            builder.Services.AddTransient<FooterViewModel>();
            builder.Services.AddTransient<ItemViewModel>();
            builder.Services.AddTransient<RefinedSearchViewModel>();
            builder.Services.AddTransient<QuickSearchViewModel>();
            builder.Services.AddTransient<SellViewModel>();
            builder.Services.AddTransient<SortAndFilterViewModel>();
            builder.Services.AddTransient<StartupViewModel>();

            // Views.
            builder.Services.AddTransient<AdvancedHeaderView>();
            builder.Services.AddTransient<BasicHeaderView>();
            builder.Services.AddTransient<FooterView>();
            builder.Services.AddTransient<SearchHeadingView>();
            builder.Services.AddTransient<QuickSearchView>();
            builder.Services.AddTransient<SortAndFilterView>();

            // Pages.
            builder.Services.AddTransient<AboutPage>();
            builder.Services.AddTransient<HomePage>();
            builder.Services.AddTransient<ItemPage>();
            builder.Services.AddTransient<CoarseSearchPage>();
            builder.Services.AddTransient<RefinedSearchPage>();
            builder.Services.AddTransient<SellPage>();

            return builder.Build();
        }
    }
}