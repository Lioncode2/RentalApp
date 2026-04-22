namespace StarterApp.Database.Models;

public enum RentalStatus
{
    Requested, Approved, Rejected, OutForRent, Overdue, Returned, Completed
}

public class Rental
{
    public int Id { get; set; }
    public int ItemId { get; set; }
    public Item? Item { get; set; }
    public int BorrowerId { get; set; }
    public User? Borrower { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal TotalPrice { get; set; }
    public RentalStatus Status { get; set; } = RentalStatus.Requested;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
