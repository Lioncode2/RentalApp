using StarterApp.Services;

namespace StarterApp.Services;

// Abstracts GPS location from the rest of the app
// ViewModels and repositories never call Geolocation directly - always through this service
// This makes location easy to mock in unit tests
public class LocationService : ILocationService
{
    // Gets the current device GPS location
    // Falls back to Edinburgh coordinates if GPS is unavailable or times out
    public async Task<(double Latitude, double Longitude)> GetCurrentLocationAsync()
    {
        try
        {
            // Try last known location first (faster, no GPS spin-up needed)
            var location = await Geolocation.GetLastKnownLocationAsync();
            if (location != null)
                return (location.Latitude, location.Longitude);

            // Request fresh GPS location with 5 second timeout
            var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(5));
            location = await Geolocation.GetLocationAsync(request);
            return location != null ? (location.Latitude, location.Longitude) : (55.9533, -3.1883);
        }
        catch
        {
            // Default to Edinburgh if GPS fails
            return (55.9533, -3.1883);
        }
    }
}
