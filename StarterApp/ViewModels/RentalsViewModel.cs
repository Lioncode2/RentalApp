using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StarterApp.Services;
using StarterApp.Database.Models;
using System.Collections.ObjectModel;

namespace StarterApp.ViewModels;

public partial class RentalsViewModel : ObservableObject
{
    private readonly IRentalService? _rentalService;
    private readonly IAuthenticationService? _authService;

    [ObservableProperty] private ObservableCollection<Rental> incoming = new();
    [ObservableProperty] private ObservableCollection<Rental> outgoing = new();
    [ObservableProperty] private bool isLoading;

    public RentalsViewModel() { }

    public RentalsViewModel(IRentalService rentalService, IAuthenticationService authService)
    {
        _rentalService = rentalService;
        _authService = authService;
    }

    [RelayCommand]
    public async Task LoadRentalsAsync()
    {
        if (_rentalService == null || _authService == null) return;
        IsLoading = true;
        try
        {
            var userId = _authService.CurrentUser?.Id ?? 0;
            var incomingResult = await _rentalService.GetIncomingAsync(userId);
            var outgoingResult = await _rentalService.GetOutgoingAsync(userId);
            Incoming = new ObservableCollection<Rental>(incomingResult);
            Outgoing = new ObservableCollection<Rental>(outgoingResult);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading rentals: {ex.Message}");
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    public async Task ApproveAsync(int rentalId)
    {
        if (_rentalService == null) return;
        await _rentalService.ApproveRentalAsync(rentalId);
        await LoadRentalsAsync();
    }

    [RelayCommand]
    public async Task RejectAsync(int rentalId)
    {
        if (_rentalService == null) return;
        await _rentalService.RejectRentalAsync(rentalId);
        await LoadRentalsAsync();
    }
}
