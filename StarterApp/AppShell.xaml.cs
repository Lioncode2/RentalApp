using StarterApp.ViewModels;
using StarterApp.Views;
namespace StarterApp;
public partial class AppShell : Shell
{
    public AppShell(AppShellViewModel viewModel)
    {
        BindingContext = viewModel;
        InitializeComponent();
        Routing.RegisterRoute("items", typeof(ItemsListPage));
        Routing.RegisterRoute("createitem", typeof(CreateItemPage));
        Routing.RegisterRoute("rentals", typeof(RentalsPage));
        Routing.RegisterRoute("nearby", typeof(NearbyItemsPage));
        Routing.RegisterRoute("reviews", typeof(ReviewsPage));
    }
}
