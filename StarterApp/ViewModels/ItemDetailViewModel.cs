using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StarterApp.Database.Data.Repositories;
using StarterApp.Database.Models;
using StarterApp.Services;

namespace StarterApp.ViewModels;

public partial class ItemDetailViewModel : ObservableObject
{
    private readonly IRentalService? _rentalService;
    private readonly IAuthenticationService? _authService;

    [ObservableProperty] private Item? item;

    public ItemDetailViewModel() { }

    public ItemDetailViewModel(IRentalService rentalService, IAuthenticationService authService)
    {
        _rentalService = rentalService;
        _authService = authService;
    }

    [RelayCommand]
    public async Task RequestRentalAsync()
    {
        if (_rentalService == null || Item == null) return;
        try
        {
            var userId = _authService?.CurrentUser?.Id ?? 0;
            await _rentalService.RequestRentalAsync(Item.Id, userId, DateTime.UtcNow.AddDays(1), DateTime.UtcNow.AddDays(3));
            await Application.Current!.Windows[0].Page!.DisplayAlert("Success", "Rental requested!", "OK");
        }
        catch (Exception ex)
        {
            await Application.Current!.Windows[0].Page!.DisplayAlert("Error", ex.Message, "OK");
        }
    }
}
