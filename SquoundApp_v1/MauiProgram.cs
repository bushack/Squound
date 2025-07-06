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
                    fonts.AddFont("PublicSans-VariableFont_wght.ttf", "BodyFont");
                    fonts.AddFont("PublicSans-VariableFont_wght.ttf", "FooterFont");
                    fonts.AddFont("BarlowSemiCondensed-Regular.ttf", "PriceFont");
                });

            // Register services and view models
            // This is where you register your services and view models with the dependency injection container.
            // This allows you to inject them into your pages and view models.
            // For example, ProductService is registered as a singleton service,
            // which means there will be only one instance of ProductService throughout the app.
            // ProductSearchViewModel is registered as a singleton view model,
            // which means there will be only one instance of ProductSearchViewModel throughout the app.
            // ProductDetailViewModel is registered as a transient view model,
            // which means a new instance will be created each time it is requested.
            builder.Services.AddSingleton<HttpService>();

            builder.Services.AddSingleton<AboutUsService>();
            builder.Services.AddTransient<AboutUsViewModel>();
            builder.Services.AddTransient<AboutUsPage>();

            builder.Services.AddSingleton<FooterViewModel>();
            builder.Services.AddSingleton<FooterView>();

            builder.Services.AddSingleton<ProductService>();

            builder.Services.AddSingleton<ProductSearchViewModel>();
            builder.Services.AddTransient<ProductDetailViewModel>();

            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<ProductSearchPage>();
            builder.Services.AddTransient<ProductDetailPage>();


#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
