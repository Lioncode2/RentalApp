using StarterApp.Services;
using Microsoft.Extensions.Logging;
using StarterApp.ViewModels;
using StarterApp.Database.Data;
using StarterApp.Views;
using System.Diagnostics;
using StarterApp.Services;

namespace StarterApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.Services.AddDbContext<AppDbContext>();

        builder.Services.AddSingleton<IAuthenticationService, AuthenticationService>();
        builder.Services.AddSingleton<INavigationService, NavigationService>();

        builder.Services.AddSingleton<AppShellViewModel>();
        builder.Services.AddSingleton<AppShell>();
        builder.Services.AddSingleton<App>();

        builder.Services.AddTransient<MainViewModel>();
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddSingleton<LoginViewModel>();
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddSingleton<RegisterViewModel>();
        builder.Services.AddTransient<RegisterPage>();
        builder.Services.AddTransient<UserListViewModel>();
        builder.Services.AddTransient<UserListPage>();
        builder.Services.AddTransient<UserDetailPage>();
        builder.Services.AddTransient<UserDetailViewModel>();
        builder.Services.AddSingleton<TempViewModel>();
        builder.Services.AddTransient<TempPage>();

        // Rental App Services
        builder.Services.AddScoped<StarterApp.Database.Data.Repositories.IItemRepository, StarterApp.Database.Data.Repositories.ItemRepository>();
        builder.Services.AddScoped<StarterApp.Database.Data.Repositories.IRentalRepository, StarterApp.Database.Data.Repositories.RentalRepository>();
        builder.Services.AddScoped<StarterApp.Database.Data.Repositories.IReviewRepository, StarterApp.Database.Data.Repositories.ReviewRepository>();
        builder.Services.AddScoped<StarterApp.Services.IRentalService, StarterApp.Services.RentalService>();
        builder.Services.AddScoped<StarterApp.Services.ILocationService, StarterApp.Services.LocationService>();
        builder.Services.AddTransient<StarterApp.ViewModels.ItemsListViewModel>();
        builder.Services.AddTransient<StarterApp.ViewModels.CreateItemViewModel>();
        builder.Services.AddTransient<StarterApp.ViewModels.RentalsViewModel>();
        builder.Services.AddTransient<StarterApp.ViewModels.NearbyItemsViewModel>();
        builder.Services.AddTransient<StarterApp.ViewModels.ReviewsViewModel>();
        builder.Services.AddTransient<StarterApp.Views.ItemsListPage>();
        builder.Services.AddTransient<StarterApp.Views.CreateItemPage>();
        builder.Services.AddTransient<StarterApp.Views.RentalsPage>();
        builder.Services.AddTransient<StarterApp.Views.NearbyItemsPage>();
        builder.Services.AddTransient<StarterApp.Views.ReviewsPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}