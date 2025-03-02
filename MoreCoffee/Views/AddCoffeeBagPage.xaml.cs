using MoreCoffee.ViewModels;

namespace MoreCoffee.Views;

public partial class AddCoffeeBagPage : ContentPage
{
    public AddCoffeeBagPage(AddCoffeeBagViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
