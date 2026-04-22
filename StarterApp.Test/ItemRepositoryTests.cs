using Moq;
using StarterApp.Database.Data.Repositories;
using StarterApp.Database.Models;

namespace StarterApp.Test;

public class ItemRepositoryTests
{
    [Fact]
    public void Item_HasCorrectProperties()
    {
        // Arrange & Act
        var item = new Item
        {
            Id = 1,
            Title = "Power Drill",
            Description = "A powerful drill",
            DailyRate = 15.00m,
            Category = "Tools",
            Location = "Edinburgh"
        };

        // Assert
        Assert.Equal("Power Drill", item.Title);
        Assert.Equal(15.00m, item.DailyRate);
        Assert.Equal("Tools", item.Category);
    }

    [Fact]
    public void Rental_DefaultStatus_IsRequested()
    {
        var rental = new Rental();
        Assert.Equal(RentalStatus.Requested, rental.Status);
    }

    [Fact]
    public void Review_HasCorrectRating()
    {
        var review = new Review { Rating = 5, Comment = "Great!" };
        Assert.Equal(5, review.Rating);
        Assert.Equal("Great!", review.Comment);
    }

    [Fact]
    public void Item_CollectionsInitialized()
    {
        var item = new Item();
        Assert.NotNull(item.Rentals);
        Assert.NotNull(item.Reviews);
    }
}
