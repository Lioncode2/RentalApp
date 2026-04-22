using Moq;
using StarterApp.Database.Data.Repositories;
using StarterApp.Database.Models;
using StarterApp.Services;

namespace StarterApp.Test;

public class RentalServiceTests
{
    private readonly Mock<IRentalRepository> _rentalRepoMock = new();
    private readonly Mock<IItemRepository> _itemRepoMock = new();
    private readonly RentalService _service;

    public RentalServiceTests()
    {
        _service = new RentalService(_rentalRepoMock.Object, _itemRepoMock.Object);
    }

    [Fact]
    public async Task RequestRentalAsync_ValidDates_CreatesRental()
    {
        // Arrange
        var item = new Item { Id = 1, DailyRate = 10 };
        _itemRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(item);
        _rentalRepoMock.Setup(r => r.HasOverlapAsync(1, It.IsAny<DateTime>(), It.IsAny<DateTime>())).ReturnsAsync(false);
        _rentalRepoMock.Setup(r => r.CreateAsync(It.IsAny<Rental>())).ReturnsAsync((Rental r) => r);

        // Act
        var result = await _service.RequestRentalAsync(1, 2, DateTime.Today, DateTime.Today.AddDays(3));

        // Assert
        Assert.Equal(RentalStatus.Requested, result.Status);
        Assert.Equal(30, result.TotalPrice);
    }

    [Fact]
    public async Task RequestRentalAsync_InvalidDates_ThrowsException()
    {
        // Arrange & Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() =>
            _service.RequestRentalAsync(1, 2, DateTime.Today.AddDays(3), DateTime.Today));
    }

    [Fact]
    public async Task RequestRentalAsync_Overlap_ThrowsException()
    {
        // Arrange
        _rentalRepoMock.Setup(r => r.HasOverlapAsync(1, It.IsAny<DateTime>(), It.IsAny<DateTime>())).ReturnsAsync(true);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _service.RequestRentalAsync(1, 2, DateTime.Today, DateTime.Today.AddDays(3)));
    }

    [Fact]
    public async Task ApproveRentalAsync_RequestedRental_ApprovesIt()
    {
        // Arrange
        var rental = new Rental { Id = 1, Status = RentalStatus.Requested };
        _rentalRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(rental);
        _rentalRepoMock.Setup(r => r.UpdateAsync(It.IsAny<Rental>())).ReturnsAsync((Rental r) => r);

        // Act
        var result = await _service.ApproveRentalAsync(1);

        // Assert
        Assert.Equal(RentalStatus.Approved, result.Status);
    }

    [Fact]
    public async Task RejectRentalAsync_RequestedRental_RejectsIt()
    {
        // Arrange
        var rental = new Rental { Id = 1, Status = RentalStatus.Requested };
        _rentalRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(rental);
        _rentalRepoMock.Setup(r => r.UpdateAsync(It.IsAny<Rental>())).ReturnsAsync((Rental r) => r);

        // Act
        var result = await _service.RejectRentalAsync(1);

        // Assert
        Assert.Equal(RentalStatus.Rejected, result.Status);
    }
}
