using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StarterApp.Database.Data.Repositories;
using StarterApp.Database.Models;
using StarterApp.Services;
using System.Collections.ObjectModel;

namespace StarterApp.ViewModels;

// Handles the Nearby Items page
// Uses LocationService to get GPS coordinates and finds items within the radius
public partial class NearbyItemsViewModel : ObservableObject
{
    private readonly IItemRepository? _itemRepo;
    private readonly ILocationService? _locationService;

    // Items found within the search radius
    [ObservableProperty] private ObservableCollection<Item> nearbyItems = new();

    // Search radius in km - bound to the slider on screen
    [ObservableProperty] private double radiusKm = 5.0;

    [ObservableProperty] private bool isLoading;

    public NearbyItemsViewModel() { }

    public NearbyItemsViewModel(IItemRepository itemRepo, ILocationService locationService)
    {
        _itemRepo = itemRepo;
        _locationService = locationService;
    }

    // Gets current GPS location then queries the database for nearby items
    // LocationService is abstracted so we can mock it in tests
    [RelayCommand]
    public async Task SearchNearbyAsync()
    {
        if (_itemRepo == null || _locationService == null) return;
        IsLoading = true;
        try
        {
            var (lat, lon) = await _locationService.GetCurrentLocationAsync();
            var items = await _itemRepo.GetNearbyAsync(lat, lon, RadiusKm);
            NearbyItems = new ObservableCollection<Item>(items);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error searching nearby: {ex.Message}");
        }
        finally
        {
            IsLoading = false;
        }
    }
}
