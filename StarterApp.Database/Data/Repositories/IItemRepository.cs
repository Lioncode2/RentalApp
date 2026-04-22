using StarterApp.Database.Models;

namespace StarterApp.Database.Data.Repositories;

public interface IItemRepository
{
    Task<IEnumerable<Item>> GetAllAsync();
    Task<Item?> GetByIdAsync(int id);
    Task<Item> CreateAsync(Item item);
    Task<Item> UpdateAsync(Item item);
    Task DeleteAsync(int id);
    Task<IEnumerable<Item>> GetNearbyAsync(double latitude, double longitude, double radiusKm);
}
