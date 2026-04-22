using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StarterApp.Database.Models;
using StarterApp.Services;

namespace StarterApp.ViewModels;

public partial class MainViewModel : BaseViewModel
{
    private readonly IAuthenticationService _authService;
    private readonly INavigationService _navigationService;

    [ObservableProperty]
    private User? currentUser;

    [ObservableProperty]
    private string welcomeMessage = string.Empty;

    [ObservableProperty]
    private bool isAdmin;

    public MainViewModel()
    {
        Title = "Dashboard";
    }

    public MainViewModel(IAuthenticationService authService, INavigationService navigationService)
    {
        _authService = authService;
        _navigationService = navigationService;
        Title = "Dashboard";
        LoadUserData();
    }

    private void LoadUserData()
    {
        CurrentUser = _authService.CurrentUser;
        IsAdmin = _authService.HasRole("Admin");
        if (CurrentUser != null)
        {
            WelcomeMessage = $"Welcome, {CurrentUser.FullName}!";
        }
    }

    [RelayCommand]
    private async Task LogoutAsync()
    {
        var result = await Application.Current.MainPage.DisplayAlert("Logout", "Are you sure you want to logout?", "Yes", "No");
        if (result)
        {
            await _authService.LogoutAsync();
            await _navigationService.NavigateToAsync("LoginPage");
        }
    }

    [RelayCommand]
    private async Task NavigateToProfileAsync()
    {
        await _navigationService.NavigateToAsync("TempPage");
    }

    [RelayCommand]
    private async Task NavigateToSettingsAsync()
    {
        await _navigationService.NavigateToAsync("TempPage");
    }

    [RelayCommand]
    private async Task NavigateToUserListAsync()
    {
        if (!IsAdmin)
        {
            await Application.Current.MainPage.DisplayAlert("Access Denied", "You don't have permission to access admin features.", "OK");
            return;
        }
        await _navigationService.NavigateToAsync("UserListPage");
    }

    [RelayCommand]
    private async Task NavigateToItemsAsync()
    {
        await _navigationService.NavigateToAsync("items");
    }

    [RelayCommand]
    private async Task NavigateToCreateItemAsync()
    {
        await _navigationService.NavigateToAsync("createitem");
    }

    [RelayCommand]
    private async Task NavigateToRentalsAsync()
    {
        await _navigationService.NavigateToAsync("rentals");
    }

    [RelayCommand]
    private async Task NavigateToNearbyAsync()
    {
        await _navigationService.NavigateToAsync("nearby");
    }

    [RelayCommand]
    private async Task NavigateToReviewsAsync()
    {
        await _navigationService.NavigateToAsync("reviews");
    }

    [RelayCommand]
    private async Task RefreshDataAsync()
    {
        try
        {
            IsBusy = true;
            LoadUserData();
            await Task.Delay(1000);
        }
        catch (Exception ex)
        {
            SetError($"Failed to refresh data: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }
}
