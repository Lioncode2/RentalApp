using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StarterApp.Database.Data.Repositories;
using StarterApp.Database.Models;
using StarterApp.Services;
using System.Collections.ObjectModel;

namespace StarterApp.ViewModels;

// Handles the Browse Items page
// Loads all items from the database and handles rental requests
public partial class ItemsListViewModel : ObservableObject
{
    private readonly IItemRepository? _itemRepo;
    private readonly IRentalService? _rentalService;
    private readonly IAuthenticationService? _authService;

    // List of items shown on screen - updates UI automatically when changed
    [ObservableProperty] private ObservableCollection<Item> items = new();

    // Shows loading spinner while fetching data
    [ObservableProperty] private bool isLoading;

    // Empty constructor needed for XAML
    public ItemsListViewModel() { }

    // DI container calls this constructor and injects the services
    public ItemsListViewModel(IItemRepository itemRepo, IRentalService rentalService, IAuthenticationService authService)
    {
        _itemRepo = itemRepo;
        _rentalService = rentalService;
        _authService = authService;
    }

    // Fetches all items from the database and displays them on screen
    [RelayCommand]
    public async Task LoadItemsAsync()
    {
        if (_itemRepo == null) return;
        IsLoading = true;
        try
        {
            var result = await _itemRepo.GetAllAsync();
            Items = new ObservableCollection<Item>(result);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading items: {ex.Message}");
        }
        finally
        {
            IsLoading = false;
        }
    }

    // Submits a rental request for the selected item
    // RentalService handles the business rules like double booking and price calculation
    [RelayCommand]
    public async Task RequestRentalAsync(Item item)
    {
        if (_rentalService == null || _authService == null) return;
        try
        {
            var userId = _authService.CurrentUser?.Id ?? 0;
            var startDate = DateTime.UtcNow.AddDays(1);
            var endDate = DateTime.UtcNow.AddDays(3);
            await _rentalService.RequestRentalAsync(item.Id, userId, startDate, endDate);
            await Application.Current!.Windows[0].Page!.DisplayAlert("Success", $"Rental requested for {item.Title}!", "OK");
        }
        catch (Exception ex)
        {
            await Application.Current!.Windows[0].Page!.DisplayAlert("Error", ex.Message, "OK");
        }
    }
}
