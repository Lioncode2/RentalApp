using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StarterApp.Database.Data.Repositories;
using StarterApp.Database.Models;
using StarterApp.Services;
using System.Collections.ObjectModel;

namespace StarterApp.ViewModels;

// Handles the Reviews page
// Allows users to submit star ratings and comments, and view existing reviews
public partial class ReviewsViewModel : ObservableObject
{
    private readonly IReviewRepository? _reviewRepo;
    private readonly IAuthenticationService? _authService;

    // All reviews for the current item
    [ObservableProperty] private ObservableCollection<Review> reviews = new();

    // Star rating selected by user (1 to 5)
    [ObservableProperty] private int rating = 5;

    // Comment text typed by user
    [ObservableProperty] private string comment = string.Empty;

    // Calculated average of all ratings for the item
    [ObservableProperty] private double averageRating;

    // The item being reviewed - defaults to 1 for demo purposes
    [ObservableProperty] private int currentItemId = 1;

    [ObservableProperty] private bool isLoading;

    public ReviewsViewModel() { }

    public ReviewsViewModel(IReviewRepository reviewRepo, IAuthenticationService authService)
    {
        _reviewRepo = reviewRepo;
        _authService = authService;
    }

    // Loads all reviews for the current item and calculates average rating
    [RelayCommand]
    public async Task LoadReviewsAsync()
    {
        if (_reviewRepo == null) return;
        IsLoading = true;
        try
        {
            var result = await _reviewRepo.GetByItemIdAsync(CurrentItemId);
            Reviews = new ObservableCollection<Review>(result);
            AverageRating = await _reviewRepo.GetAverageRatingAsync(CurrentItemId);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading reviews: {ex.Message}");
        }
        finally
        {
            IsLoading = false;
        }
    }

    // Saves a new review to the database then reloads the list
    [RelayCommand]
    public async Task SubmitReviewAsync()
    {
        if (_reviewRepo == null) return;
        try
        {
            var review = new Review
            {
                ItemId = CurrentItemId,
                ReviewerId = _authService?.CurrentUser?.Id ?? 1,
                Rating = Rating,
                Comment = Comment
            };
            await _reviewRepo.CreateAsync(review);
            Comment = string.Empty;
            Rating = 5;
            await Application.Current!.Windows[0].Page!.DisplayAlert("Success", "Review submitted!", "OK");
            await LoadReviewsAsync();
        }
        catch (Exception ex)
        {
            await Application.Current!.Windows[0].Page!.DisplayAlert("Error", ex.Message, "OK");
        }
    }
}
