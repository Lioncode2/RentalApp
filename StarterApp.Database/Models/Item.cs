namespace StarterApp.Database.Models;

public class Item
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal DailyRate { get; set; }
    public string Category { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public int OwnerId { get; set; }
    public User? Owner { get; set; }
    public ICollection<Rental> Rentals { get; set; } = new List<Rental>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
}
