using StarterApp.Database.Models;

namespace StarterApp.Services;

public interface IRentalService
{
    Task<Rental> RequestRentalAsync(int itemId, int borrowerId, DateTime start, DateTime end);
    Task<Rental> ApproveRentalAsync(int rentalId);
    Task<Rental> RejectRentalAsync(int rentalId);
    Task<Rental> ReturnRentalAsync(int rentalId);
    Task<IEnumerable<Rental>> GetIncomingAsync(int ownerId);
    Task<IEnumerable<Rental>> GetOutgoingAsync(int borrowerId);
}
