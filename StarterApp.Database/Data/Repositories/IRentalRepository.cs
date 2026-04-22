using StarterApp.Database.Models;

namespace StarterApp.Database.Data.Repositories;

public interface IRentalRepository
{
    Task<Rental?> GetByIdAsync(int id);
    Task<IEnumerable<Rental>> GetIncomingAsync(int ownerId);
    Task<IEnumerable<Rental>> GetOutgoingAsync(int borrowerId);
    Task<Rental> CreateAsync(Rental rental);
    Task<Rental> UpdateAsync(Rental rental);
    Task<bool> HasOverlapAsync(int itemId, DateTime start, DateTime end);
}
