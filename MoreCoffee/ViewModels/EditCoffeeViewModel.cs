using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MoreCoffee.Models;
using MoreCoffee.Services;

namespace MoreCoffee.ViewModels;

[QueryProperty(nameof(Coffee), "Coffee")]
public partial class EditCoffeeViewModel : ObservableObject
{
    private readonly CoffeeService coffeeService;
    
    [ObservableProperty]
    Coffee? coffee;

    public EditCoffeeViewModel(CoffeeService coffeeService)
    {
        this.coffeeService = coffeeService;
    }

    [RelayCommand]
    async Task SaveCoffee()
    {
        if (Coffee == null)
            return;

        await coffeeService.UpdateCoffeeAsync(Coffee);
        await Shell.Current.GoToAsync("..");
    }
}