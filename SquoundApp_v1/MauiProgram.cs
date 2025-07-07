using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using SquoundApp_v1.Pages;
using SquoundApp_v1.Services;
using SquoundApp_v1.Utilities;
using SquoundApp_v1.ViewModels;
using SquoundApp_v1.Views;

namespace SquoundApp_v1
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
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
            builder.Services.AddSingleton<HttpService>();

            builder.Services.AddSingleton<AboutService>();
            builder.Services.AddTransient<AboutViewModel>();
            builder.Services.AddTransient<AboutPage>();

            builder.Services.AddSingleton<SellService>();
            builder.Services.AddTransient<SellViewModel>();
            builder.Services.AddTransient<SellPage>();

            builder.Services.AddSingleton<FooterViewModel>();
            builder.Services.AddSingleton<FooterView>();

            builder.Services.AddSingleton<ProductService>();

            builder.Services.AddSingleton<ProductSearchViewModel>();
            builder.Services.AddTransient<ProductListingViewModel>();

            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<ProductSearchPage>();
            builder.Services.AddTransient<ProductListingPage>();


#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
