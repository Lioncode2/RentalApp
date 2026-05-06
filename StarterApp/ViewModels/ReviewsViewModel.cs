using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StarterApp.Database.Data.Repositories;
using StarterApp.Database.Models;
using StarterApp.Services;
using System.Collections.ObjectModel;

namespace StarterApp.ViewModels;

public partial class ReviewsViewModel : ObservableObject
{
    private readonly IReviewRepository? _reviewRepo;
    private readonly IAuthenticationService? _authService;

    [ObservableProperty] private ObservableCollection<Review> reviews = new();
    [ObservableProperty] private int rating = 5;
    [ObservableProperty] private string comment = string.Empty;
    [ObservableProperty] private double averageRating;
    [ObservableProperty] private int currentItemId = 1;
    [ObservableProperty] private bool isLoading;

    public ReviewsViewModel() { }

    public ReviewsViewModel(IReviewRepository reviewRepo, IAuthenticationService authService)
    {
        _reviewRepo = reviewRepo;
        _authService = authService;
    }

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
