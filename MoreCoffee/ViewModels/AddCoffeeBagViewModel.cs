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
    
    [ObservableProperty]
    private string totalOunces = "12";  // Default to 12oz bag

    [ObservableProperty]
    private string? totalOuncesErrorMessage;

    [ObservableProperty]
    private bool hasTotalOuncesError;

    public bool IsValid => !HasTotalOuncesError;

    public List<string> RoastLevels { get; } = Enum.GetNames<RoastLevel>().ToList();

    public AddCoffeeBagViewModel(BagOfCoffeeService coffeeService)
    {
        _coffeeService = coffeeService;
    }
    
    partial void OnTotalOuncesChanged(string value)
    {
        ValidateTotalOunces();
    }
    
    [RelayCommand]
    private void ValidateTotalOunces()
    {
        if (!double.TryParse(TotalOunces, out double totalOuncesValue) || totalOuncesValue <= 0)
        {
            TotalOuncesErrorMessage = "Total ounces must be a valid positive number";
            HasTotalOuncesError = true;
        }
        else
        {
            TotalOuncesErrorMessage = null;
            HasTotalOuncesError = false;
        }
    }

    [RelayCommand(CanExecute = nameof(IsValid))]
    private async Task SaveCoffeeBagAsync()
    {
        if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Roaster))
            return;

        if (!double.TryParse(TotalOunces, out double totalOuncesValue))
        {
            TotalOuncesErrorMessage = "Please enter a valid number for total ounces";
            HasTotalOuncesError = true;
            return;
        }

        var bag = new BagOfCoffee
        {
            Name = Name,
            Roaster = Roaster,
            Origin = Origin,
            RoastDate = RoastDate,
            RoastLevel = Enum.TryParse<RoastLevel>(SelectedRoastLevel, out var parsedRoastLevel) ? parsedRoastLevel : RoastLevel.Medium,
            TastingNotes = TastingNotes,
            TotalOunces = totalOuncesValue,
            RemainingOunces = totalOuncesValue  // Initially, remaining = total
        };

        await _coffeeService.AddBagOfCoffeeAsync(bag);
        await Shell.Current.GoToAsync("..");
    }
}
