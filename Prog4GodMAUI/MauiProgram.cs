using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Prog4GodMAUI.Views;

using Prog4GodMAUI.Services;
using Prog4GodMAUI.ViewModels;

namespace Prog4GodMAUI
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
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton<AppShell>();

            //services
            builder.Services.AddSingleton<BaseService>();
            builder.Services.AddSingleton<ProductService>();
            builder.Services.AddSingleton<CategoryService>();
            builder.Services.AddSingleton<CartService>();
            builder.Services.AddSingleton<UserService>();
            builder.Services.AddSingleton<AuthService>();
            builder.Services.AddSingleton<RecentlyViewedProductService>();

            builder.Services.AddTransient<HomePage>();
            builder.Services.AddTransient<ProductDetailsPage>();
            builder.Services.AddTransient<CategoryPage>();
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<CartPage>();
            builder.Services.AddTransient<ProfilePage>();
            builder.Services.AddTransient<RecentlyViewedPage>();

            builder.Services.AddTransient<HomePageViewModel>();
            builder.Services.AddTransient<BaseViewModel>();
            builder.Services.AddTransient<ProductDetailsViewModel>();
            builder.Services.AddTransient<CategoryPageViewModel>();
            builder.Services.AddTransient<RecentlyViewedPageViewModel>();
            builder.Services.AddTransient<CartViewModel>();
            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<ProfilePageViewModel>();



#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
