using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StarterApp.Database.Data.Repositories;
using StarterApp.Database.Models;
using StarterApp.Services;

namespace StarterApp.ViewModels;

public partial class CreateItemViewModel : ObservableObject
{
    private readonly IItemRepository? _itemRepo;
    private readonly IAuthenticationService? _authService;
    private readonly INavigationService? _navigationService;

    [ObservableProperty] private string title = string.Empty;
    [ObservableProperty] private string description = string.Empty;
    [ObservableProperty] private decimal dailyRate;
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
        HasError = false;
        var item = new Item
        {
            Title = Title,
            Description = Description,
            DailyRate = DailyRate,
            Category = Category,
            Location = Location,
            OwnerId = _authService?.CurrentUser?.Id ?? 0
        };
        await _itemRepo.CreateAsync(item);
        await _navigationService!.NavigateBackAsync();
    }
}
