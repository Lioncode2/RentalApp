using StarterApp.Database.Data.Repositories;
using StarterApp.Database.Models;

namespace StarterApp.Services;

public class RentalService : IRentalService
{
    private readonly IRentalRepository _rentalRepo;
    private readonly IItemRepository _itemRepo;

    public RentalService(IRentalRepository rentalRepo, IItemRepository itemRepo)
    {
        _rentalRepo = rentalRepo;
        _itemRepo = itemRepo;
    }

    public async Task<Rental> RequestRentalAsync(int itemId, int borrowerId, DateTime start, DateTime end)
    {
        if (start >= end) throw new ArgumentException("End date must be after start date");
        if (await _rentalRepo.HasOverlapAsync(itemId, start, end))
            throw new InvalidOperationException("Item is already booked for these dates");

        var item = await _itemRepo.GetByIdAsync(itemId) ?? throw new KeyNotFoundException("Item not found");
        var days = (end - start).Days;
        var rental = new Rental
        {
            ItemId = itemId,
            BorrowerId = borrowerId,
            StartDate = start,
            EndDate = end,
            TotalPrice = item.DailyRate * days,
            Status = RentalStatus.Requested
        };
        return await _rentalRepo.CreateAsync(rental);
    }

    public async Task<Rental> ApproveRentalAsync(int rentalId)
    {
        var rental = await _rentalRepo.GetByIdAsync(rentalId) ?? throw new KeyNotFoundException();
        if (rental.Status != RentalStatus.Requested) throw new InvalidOperationException("Can only approve requested rentals");
        rental.Status = RentalStatus.Approved;
        return await _rentalRepo.UpdateAsync(rental);
    }

    public async Task<Rental> RejectRentalAsync(int rentalId)
    {
        var rental = await _rentalRepo.GetByIdAsync(rentalId) ?? throw new KeyNotFoundException();
        if (rental.Status != RentalStatus.Requested) throw new InvalidOperationException("Can only reject requested rentals");
        rental.Status = RentalStatus.Rejected;
        return await _rentalRepo.UpdateAsync(rental);
    }

    public async Task<Rental> ReturnRentalAsync(int rentalId)
    {
        var rental = await _rentalRepo.GetByIdAsync(rentalId) ?? throw new KeyNotFoundException();
        if (rental.Status != RentalStatus.Approved && rental.Status != RentalStatus.OutForRent)
            throw new InvalidOperationException("Can only return approved or out-for-rent rentals");
        rental.Status = RentalStatus.Completed;
        return await _rentalRepo.UpdateAsync(rental);
    }

    public async Task<IEnumerable<Rental>> GetIncomingAsync(int ownerId)
        => await _rentalRepo.GetIncomingAsync(ownerId);

    public async Task<IEnumerable<Rental>> GetOutgoingAsync(int borrowerId)
        => await _rentalRepo.GetOutgoingAsync(borrowerId);
}
