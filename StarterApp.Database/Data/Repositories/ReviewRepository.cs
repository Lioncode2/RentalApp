using Microsoft.EntityFrameworkCore;
using StarterApp.Database.Models;

namespace StarterApp.Database.Data.Repositories;

public class ReviewRepository : IReviewRepository
{
    private readonly AppDbContext _context;

    public ReviewRepository(AppDbContext context) => _context = context;

    public async Task<IEnumerable<Review>> GetByItemIdAsync(int itemId)
        => await _context.Reviews.Include(r => r.Reviewer).Where(r => r.ItemId == itemId).ToListAsync();

    public async Task<Review> CreateAsync(Review review)
    {
        _context.Reviews.Add(review);
        await _context.SaveChangesAsync();
        return review;
    }

    public async Task<double> GetAverageRatingAsync(int itemId)
    {
        var reviews = await _context.Reviews.Where(r => r.ItemId == itemId).ToListAsync();
        return reviews.Any() ? reviews.Average(r => r.Rating) : 0;
    }
}
