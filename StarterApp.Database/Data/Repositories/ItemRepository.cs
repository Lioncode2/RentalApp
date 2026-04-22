using Microsoft.EntityFrameworkCore;
using StarterApp.Database.Models;

namespace StarterApp.Database.Data.Repositories;

public class ItemRepository : IItemRepository
{
    private readonly AppDbContext _context;

    public ItemRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Item>> GetAllAsync()
        => await _context.Items.Include(i => i.Owner).ToListAsync();

    public async Task<Item?> GetByIdAsync(int id)
        => await _context.Items.Include(i => i.Owner).Include(i => i.Reviews).FirstOrDefaultAsync(i => i.Id == id);

    public async Task<Item> CreateAsync(Item item)
    {
        _context.Items.Add(item);
        await _context.SaveChangesAsync();
        return item;
    }

    public async Task<Item> UpdateAsync(Item item)
    {
        _context.Items.Update(item);
        await _context.SaveChangesAsync();
        return item;
    }

    public async Task DeleteAsync(int id)
    {
        var item = await _context.Items.FindAsync(id);
        if (item != null) { _context.Items.Remove(item); await _context.SaveChangesAsync(); }
    }

    public async Task<IEnumerable<Item>> GetNearbyAsync(double latitude, double longitude, double radiusKm)
    {
        var all = await _context.Items.Include(i => i.Owner).ToListAsync();
        return all.Where(i => i.Latitude.HasValue && i.Longitude.HasValue &&
            GetDistance(latitude, longitude, i.Latitude.Value, i.Longitude.Value) <= radiusKm);
    }

    private static double GetDistance(double lat1, double lon1, double lat2, double lon2)
    {
        const double R = 6371;
        var dLat = (lat2 - lat1) * Math.PI / 180;
        var dLon = (lon2 - lon1) * Math.PI / 180;
        var a = Math.Sin(dLat/2) * Math.Sin(dLat/2) +
                Math.Cos(lat1 * Math.PI / 180) * Math.Cos(lat2 * Math.PI / 180) *
                Math.Sin(dLon/2) * Math.Sin(dLon/2);
        return R * 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1-a));
    }
}
