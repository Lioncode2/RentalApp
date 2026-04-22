using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StarterApp.Database.Data.Repositories;
using StarterApp.Database.Models;

namespace StarterApp.ViewModels;

public partial class CreateItemViewModel : ObservableObject
{
    private readonly IItemRepository _itemRepo;

    [ObservableProperty] private string _title = string.Empty;
    [ObservableProperty] private string _description = string.Empty;
    [ObservableProperty] private decimal _dailyRate;
    [ObservableProperty] private string _category = string.Empty;
    [ObservableProperty] private string _location = string.Empty;

    public CreateItemViewModel(IItemRepository itemRepo)
    {
        _itemRepo = itemRepo;
    }

    [RelayCommand]
    public async Task CreateItemAsync()
    {
        var item = new Item
        {
            Title = Title,
            Description = Description,
            DailyRate = DailyRate,
            Category = Category,
            Location = Location
        };
        await _itemRepo.CreateAsync(item);
    }
}
