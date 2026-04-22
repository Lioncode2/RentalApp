using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StarterApp.Services;
using StarterApp.Database.Models;
using System.Collections.ObjectModel;

namespace StarterApp.ViewModels;

public partial class RentalsViewModel : ObservableObject
{
    private readonly IRentalService _rentalService;

    [ObservableProperty] private ObservableCollection<Rental> _incoming = new();
    [ObservableProperty] private ObservableCollection<Rental> _outgoing = new();
    [ObservableProperty] private bool _isLoading;

    public RentalsViewModel(IRentalService rentalService)
    {
        _rentalService = rentalService;
    }

    [RelayCommand]
    public async Task LoadRentalsAsync(int userId)
    {
        IsLoading = true;
        var incoming = await _rentalService.GetIncomingAsync(userId);
        var outgoing = await _rentalService.GetOutgoingAsync(userId);
        Incoming = new ObservableCollection<Rental>(incoming);
        Outgoing = new ObservableCollection<Rental>(outgoing);
        IsLoading = false;
    }

    [RelayCommand]
    public async Task ApproveAsync(int rentalId) => await _rentalService.ApproveRentalAsync(rentalId);

    [RelayCommand]
    public async Task RejectAsync(int rentalId) => await _rentalService.RejectRentalAsync(rentalId);
}
