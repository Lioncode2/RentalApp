using StarterApp.Services;
namespace StarterApp.Services;

public class LocationService : ILocationService
{
    public async Task<(double Latitude, double Longitude)> GetCurrentLocationAsync()
    {
        var location = await Geolocation.GetLastKnownLocationAsync();
        if (location != null)
            return (location.Latitude, location.Longitude);
        location = await Geolocation.GetLocationAsync(new GeolocationRequest(GeolocationAccuracy.Medium));
        return location != null ? (location.Latitude, location.Longitude) : (55.9533, -3.1883);
    }
}
