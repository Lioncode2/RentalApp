using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StarterApp.Services;
using StarterApp.Database.Models;
using System.Collections.ObjectModel;

namespace StarterApp.ViewModels;

// Handles the My Rentals page
// Shows rentals where current user is the owner (incoming) and renter (outgoing)
public partial class RentalsViewModel : ObservableObject
{
    private readonly IRentalService? _rentalService;
    private readonly IAuthenticationService? _authService;

    // Items I own that others want to rent
    [ObservableProperty] private ObservableCollection<Rental> incoming = new();

    // Items I have requested to rent from others
    [ObservableProperty] private ObservableCollection<Rental> outgoing = new();

    [ObservableProperty] private bool isLoading;

    public RentalsViewModel() { }

    public RentalsViewModel(IRentalService rentalService, IAuthenticationService authService)
    {
        _rentalService = rentalService;
        _authService = authService;
    }

    // Loads both incoming and outgoing rentals for the current user
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

    // Approves a rental - accepts object parameter from XAML and converts to int
    [RelayCommand]
    public async Task ApproveAsync(object parameter)
    {
        if (_rentalService == null) return;
        if (!int.TryParse(parameter?.ToString(), out int rentalId)) return;
        try
        {
            await _rentalService.ApproveRentalAsync(rentalId);
            await LoadRentalsAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error approving rental: {ex.Message}");
        }
    }

    // Rejects a rental - accepts object parameter from XAML and converts to int
    [RelayCommand]
    public async Task RejectAsync(object parameter)
    {
        if (_rentalService == null) return;
        if (!int.TryParse(parameter?.ToString(), out int rentalId)) return;
        try
        {
            await _rentalService.RejectRentalAsync(rentalId);
            await LoadRentalsAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error rejecting rental: {ex.Message}");
        }
    }
}
