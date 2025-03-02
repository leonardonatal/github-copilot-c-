using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MoreCoffee.Models;
using MoreCoffee.Services;

namespace MoreCoffee.ViewModels;

public partial class AddCoffeeBagViewModel : ObservableObject
{
    private readonly BagOfCoffeeService _coffeeService;

    [ObservableProperty]
    private string roaster = string.Empty;

    [ObservableProperty]
    private string name = string.Empty;

    [ObservableProperty]
    private string origin = string.Empty;

    [ObservableProperty]
    private DateTime roastDate = DateTime.Today;

    [ObservableProperty]
    private string tastingNotes = string.Empty;

    [ObservableProperty]
    private string selectedRoastLevel = "Medium";

    public List<string> RoastLevels { get; } = Enum.GetNames<RoastLevel>().ToList();

    public AddCoffeeBagViewModel(BagOfCoffeeService coffeeService)
    {
        _coffeeService = coffeeService;
    }

    [RelayCommand]
    private async Task SaveCoffeeBagAsync()
    {
        if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Roaster))
            return;

        var bag = new BagOfCoffee
        {
            Name = Name,
            Roaster = Roaster,
            Origin = Origin,
            RoastDate = RoastDate,
            RoastLevel = Enum.TryParse<RoastLevel>(SelectedRoastLevel, out var parsedRoastLevel) ? parsedRoastLevel : RoastLevel.Medium,
            TastingNotes = TastingNotes
        };

        await _coffeeService.AddBagOfCoffeeAsync(bag);
        await Shell.Current.GoToAsync("..");
    }
}
