using StarterApp.Database.Models;

namespace StarterApp.Database.Data.Repositories;

public interface IReviewRepository
{
    Task<IEnumerable<Review>> GetByItemIdAsync(int itemId);
    Task<Review> CreateAsync(Review review);
    Task<double> GetAverageRatingAsync(int itemId);
}
