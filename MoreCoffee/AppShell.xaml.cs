using MoreCoffee.Views;

namespace MoreCoffee;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        
        Routing.RegisterRoute("EditCoffeePage", typeof(Views.EditCoffeePage));
        Routing.RegisterRoute(nameof(AddCoffeeBagPage), typeof(AddCoffeeBagPage));
    }
}
