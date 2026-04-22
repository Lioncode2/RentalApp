using Microsoft.EntityFrameworkCore;
using StarterApp.Database.Models;

namespace StarterApp.Database.Data.Repositories;

public class RentalRepository : IRentalRepository
{
    private readonly AppDbContext _context;

    public RentalRepository(AppDbContext context) => _context = context;

    public async Task<Rental?> GetByIdAsync(int id)
        => await _context.Rentals.Include(r => r.Item).Include(r => r.Borrower).FirstOrDefaultAsync(r => r.Id == id);

    public async Task<IEnumerable<Rental>> GetIncomingAsync(int ownerId)
        => await _context.Rentals.Include(r => r.Item).Include(r => r.Borrower)
            .Where(r => r.Item!.OwnerId == ownerId).ToListAsync();

    public async Task<IEnumerable<Rental>> GetOutgoingAsync(int borrowerId)
        => await _context.Rentals.Include(r => r.Item)
            .Where(r => r.BorrowerId == borrowerId).ToListAsync();

    public async Task<Rental> CreateAsync(Rental rental)
    {
        _context.Rentals.Add(rental);
        await _context.SaveChangesAsync();
        return rental;
    }

    public async Task<Rental> UpdateAsync(Rental rental)
    {
        _context.Rentals.Update(rental);
        await _context.SaveChangesAsync();
        return rental;
    }

    public async Task<bool> HasOverlapAsync(int itemId, DateTime start, DateTime end)
        => await _context.Rentals.AnyAsync(r => r.ItemId == itemId &&
            r.Status != RentalStatus.Rejected && r.Status != RentalStatus.Completed &&
            r.StartDate < end && r.EndDate > start);
}
