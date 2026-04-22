using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StarterApp.Database.Data.Repositories;
using StarterApp.Database.Models;
using StarterApp.Services;
using System.Collections.ObjectModel;

namespace StarterApp.ViewModels;

public partial class NearbyItemsViewModel : ObservableObject
{
    private readonly IItemRepository _itemRepo;
    private readonly ILocationService _locationService;

    [ObservableProperty] private ObservableCollection<Item> _nearbyItems = new();
    [ObservableProperty] private double _radiusKm = 5.0;
    [ObservableProperty] private bool _isLoading;

    public NearbyItemsViewModel(IItemRepository itemRepo, ILocationService locationService)
    {
        _itemRepo = itemRepo;
        _locationService = locationService;
    }

    [RelayCommand]
    public async Task SearchNearbyAsync()
    {
        IsLoading = true;
        var (lat, lon) = await _locationService.GetCurrentLocationAsync();
        var items = await _itemRepo.GetNearbyAsync(lat, lon, RadiusKm);
        NearbyItems = new ObservableCollection<Item>(items);
        IsLoading = false;
    }
}
