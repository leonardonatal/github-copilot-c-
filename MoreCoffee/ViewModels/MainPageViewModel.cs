using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MoreCoffee.Models;
using MoreCoffee.Services;
using System.Collections.ObjectModel;

namespace MoreCoffee.ViewModels;

[QueryProperty(nameof(IsEdited), "IsEdited")]
public partial class MainPageViewModel : ObservableObject, INavigationAwareAsync
{
    private readonly CoffeeService _coffeeService;
    private readonly BagOfCoffeeService _bagOfCoffeeService;

    [ObservableProperty]
    private ObservableCollection<CoffeeGroup> coffeeGroups;

    [ObservableProperty]
    private string totalOuncesText = string.Empty;

    [ObservableProperty]
    private string name = string.Empty;

    [ObservableProperty]
    private string ounces = string.Empty;

    [ObservableProperty]
    private DateTime selectedDate = DateTime.Today;
    
    [ObservableProperty]
    private BagOfCoffee? selectedBag;
    
    [ObservableProperty]
    private ObservableCollection<BagOfCoffee> availableBags;

    [ObservableProperty]
    private bool isEdited;
    
    [ObservableProperty]
    private bool isUsingBag;

    public MainPageViewModel(CoffeeService coffeeService, BagOfCoffeeService bagOfCoffeeService)
    {
        _coffeeService = coffeeService;
        _bagOfCoffeeService = bagOfCoffeeService;
        CoffeeGroups = new ObservableCollection<CoffeeGroup>();
        AvailableBags = new ObservableCollection<BagOfCoffee>();
    }

    private async Task LoadCoffeesAsync()
    {
        var coffeeList = await _coffeeService.GetCoffeesAsync();
        
        // Group coffees by date
        var groups = coffeeList
            .GroupBy(c => c.DateAdded.Date)
            .OrderByDescending(g => g.Key)
            .Select(g => new CoffeeGroup(g.Key, [.. g]))
            .ToList();

        CoffeeGroups = [.. groups];
        OnPropertyChanged(nameof(CoffeeGroups));

        var totalOunces = coffeeList.Sum(c => c.Ounces);
        TotalOuncesText = $"Total Coffee Consumed: {totalOunces}oz";
    }

    private async Task InitializeServicesAsync()
    {
        await _coffeeService.InitializeAsync();
        await _bagOfCoffeeService.InitializeAsync();
    }   

    private async Task LoadAvailableBagsAsync()
    {
        var bags = await _bagOfCoffeeService.GetAvailableBagsOfCoffeeAsync();
        AvailableBags.Clear();
        foreach (var bag in bags)
        {
            AvailableBags.Add(bag);
        }
        
        // Auto-select first bag if there are any available
        if (AvailableBags.Count > 0 && SelectedBag == null)
        {
            SelectedBag = AvailableBags[0];
        }
    }

    [RelayCommand]
    private async Task AddCoffeeAsync()
    {
        if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Ounces))
        {
            await Shell.Current.DisplayAlert("Error", "Please enter both name and ounces", "OK");
            return;
        }

        if (!double.TryParse(Ounces, out double ouncesValue))
        {
            await Shell.Current.DisplayAlert("Error", "Please enter a valid number for ounces", "OK");
            return;
        }
        
        // Check if using a bag and validate
        if (IsUsingBag && SelectedBag == null)
        {
            await Shell.Current.DisplayAlert("Error", "Please select a coffee bag", "OK");
            return;
        }
        
        // Check if there's enough coffee in the bag
        if (IsUsingBag && SelectedBag != null && SelectedBag.RemainingOunces < ouncesValue)
        {
            await Shell.Current.DisplayAlert("Error", 
                $"Not enough coffee in the selected bag. Only {SelectedBag.RemainingOunces:F1}oz remaining.", "OK");
            return;
        }

        var coffee = new Coffee
        {
            Name = Name,
            Ounces = ouncesValue,
            DateAdded = SelectedDate,
            BagOfCoffeeId = IsUsingBag && SelectedBag != null ? SelectedBag.Id : 0,
            BagName = IsUsingBag && SelectedBag != null ? $"{SelectedBag.Roaster} - {SelectedBag.Name}" : null
        };

        try
        {
            await _coffeeService.AddCoffeeAsync(coffee);
            
            // Clear the entries
            Name = string.Empty;
            Ounces = string.Empty;

            // Reload the lists
            await LoadCoffeesAsync();
            await LoadAvailableBagsAsync();
        }
        catch (InvalidOperationException ex)
        {
            await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
        }
    }

    [RelayCommand]
    async Task EditCoffee(Coffee coffee)
    {
        var parameters = new Dictionary<string, object>
        {
            { "Coffee", coffee }
        };
        await Shell.Current.GoToAsync("EditCoffeePage", parameters);
    }
    
    [RelayCommand]
    async Task NavigateToBags()
    {
        await Shell.Current.GoToAsync("BagOfCoffeePage");
    }

    public async Task OnNavigatedToAsync()
    {
        var shouldRefresh = IsEdited || CoffeeGroups.Count == 0;

        if (shouldRefresh)
        {
            await LoadCoffeesAsync();
            IsEdited = false;
        }
        
        // Always refresh available bags
        await LoadAvailableBagsAsync();
    }

    public Task OnNavigatedFromAsync()
    {
        return Task.CompletedTask;
    }
}

public class CoffeeGroup : ObservableCollection<Coffee>
{
    public DateTime Date { get; }
    public string DisplayDate { get; }
    public double TotalOunces { get; }

    public CoffeeGroup(DateTime date, ObservableCollection<Coffee> coffees) : base(coffees)
    {
        Date = date;
        DisplayDate = date.ToString("D");
        TotalOunces = coffees.Sum(c => c.Ounces);
    }
}