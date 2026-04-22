using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StarterApp.Database.Data.Repositories;
using StarterApp.Database.Models;
using System.Collections.ObjectModel;

namespace StarterApp.ViewModels;

public partial class ReviewsViewModel : ObservableObject
{
    private readonly IReviewRepository _reviewRepo;

    [ObservableProperty] private ObservableCollection<Review> _reviews = new();
    [ObservableProperty] private int _rating = 5;
    [ObservableProperty] private string _comment = string.Empty;
    [ObservableProperty] private double _averageRating;
    [ObservableProperty] private int _currentItemId;

    public ReviewsViewModel(IReviewRepository reviewRepo)
    {
        _reviewRepo = reviewRepo;
    }

    [RelayCommand]
    public async Task LoadReviewsAsync(int itemId)
    {
        CurrentItemId = itemId;
        var reviews = await _reviewRepo.GetByItemIdAsync(itemId);
        Reviews = new ObservableCollection<Review>(reviews);
        AverageRating = await _reviewRepo.GetAverageRatingAsync(itemId);
    }

    [RelayCommand]
    public async Task SubmitReviewAsync()
    {
        var review = new Review
        {
            ItemId = CurrentItemId,
            ReviewerId = 1,
            Rating = Rating,
            Comment = Comment
        };
        await _reviewRepo.CreateAsync(review);
        await LoadReviewsAsync(CurrentItemId);
    }
}
