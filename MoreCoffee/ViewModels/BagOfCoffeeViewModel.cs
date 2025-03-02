using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MoreCoffee.Models;
using MoreCoffee.Services;
using System.Collections.ObjectModel;

namespace MoreCoffee.ViewModels;

public partial class BagOfCoffeeViewModel : ObservableObject, INavigationAwareAsync
{
    private readonly BagOfCoffeeService _coffeeService;

    [ObservableProperty]
    private ObservableCollection<BagOfCoffee> coffeeBags;

    public BagOfCoffeeViewModel(BagOfCoffeeService coffeeService)
    {
        _coffeeService = coffeeService;
        CoffeeBags = new ObservableCollection<BagOfCoffee>();
    }

    private async Task LoadCoffeeBagsAsync()
    {
        var bags = await _coffeeService.GetBagsOfCoffeeAsync();
        CoffeeBags.Clear();
        foreach (var bag in bags.OrderByDescending(b => b.DateAdded))
            CoffeeBags.Add(bag);
    }

    [RelayCommand]
    private async Task NavigateToAddBag()
    {
        await Shell.Current.GoToAsync("AddCoffeeBagPage");
    }

    [RelayCommand]
    async Task DeleteCoffeeBag(BagOfCoffee bag)
    {
        if (bag == null)
            return;

        await _coffeeService.DeleteBagOfCoffeeAsync(bag);
        CoffeeBags.Remove(bag);
    }

    public async Task OnNavigatedToAsync()
    {
        await LoadCoffeeBagsAsync();
    }

    public Task OnNavigatedFromAsync()
    {
        return Task.CompletedTask;
    }
}