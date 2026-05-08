using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StarterApp.Database.Data.Repositories;
using StarterApp.Database.Models;
using StarterApp.Services;

namespace StarterApp.ViewModels;

// Handles the Create Item page where users list items for rent
public partial class CreateItemViewModel : ObservableObject
{
    private readonly IItemRepository? _itemRepo;
    private readonly IAuthenticationService? _authService;
    private readonly INavigationService? _navigationService;

    [ObservableProperty] private string title = string.Empty;
    [ObservableProperty] private string description = string.Empty;
    // Using string so user can type any price - parsed to decimal on save
    [ObservableProperty] private string dailyRateText = string.Empty;
    [ObservableProperty] private string category = string.Empty;
    [ObservableProperty] private string location = string.Empty;
    [ObservableProperty] private string errorMessage = string.Empty;
    [ObservableProperty] private bool hasError;

    public CreateItemViewModel() { }

    public CreateItemViewModel(IItemRepository itemRepo, IAuthenticationService authService, INavigationService navigationService)
    {
        _itemRepo = itemRepo;
        _authService = authService;
        _navigationService = navigationService;
    }

    // Validates input, creates the item and saves it to the database
    // Adds random coordinates near Edinburgh for location-based search demo
    [RelayCommand]
    public async Task CreateItemAsync()
    {
        if (_itemRepo == null) return;
        if (string.IsNullOrWhiteSpace(Title))
        {
            ErrorMessage = "Title is required";
            HasError = true;
            return;
        }
        if (!decimal.TryParse(DailyRateText, out decimal dailyRate))
        {
            ErrorMessage = "Please enter a valid daily rate";
            HasError = true;
            return;
        }
        HasError = false;
        var item = new Item
        {
            Title = Title,
            Description = Description,
            DailyRate = dailyRate,
            Category = Category,
            Location = Location,
            OwnerId = _authService?.CurrentUser?.Id ?? 0,
            // Random coordinates near Edinburgh for nearby search to work
            Latitude = 55.9533 + (new Random().NextDouble() - 0.5) * 0.05,
            Longitude = -3.1883 + (new Random().NextDouble() - 0.5) * 0.05
        };
        await _itemRepo.CreateAsync(item);
        await _navigationService!.NavigateBackAsync();
    }
}
