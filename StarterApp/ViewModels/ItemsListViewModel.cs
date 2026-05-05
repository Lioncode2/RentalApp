using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StarterApp.Database.Data.Repositories;
using StarterApp.Database.Models;
using System.Collections.ObjectModel;

namespace StarterApp.ViewModels;

public partial class ItemsListViewModel : ObservableObject
{
    private readonly IItemRepository? _itemRepo;

    [ObservableProperty] private ObservableCollection<Item> items = new();
    [ObservableProperty] private bool isLoading;

    public ItemsListViewModel() { }

    public ItemsListViewModel(IItemRepository itemRepo)
    {
        _itemRepo = itemRepo;
    }

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
}
