using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MoreCoffee.Models;
using MoreCoffee.Services;
using System.Collections.ObjectModel;

namespace MoreCoffee.ViewModels;

public partial class MainPageViewModel : ObservableObject, INavigationAwareAsync
{
    private readonly CoffeeService _coffeeService;

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

    public MainPageViewModel(CoffeeService coffeeService)
    {
        _coffeeService = coffeeService;
        CoffeeGroups = new ObservableCollection<CoffeeGroup>();
        LoadCoffeesAsync().ConfigureAwait(false);
    }

    private async Task LoadCoffeesAsync()
    {
        var coffeeList = await _coffeeService.GetCoffeesAsync();
        
        // Group coffees by date
        var groups = coffeeList
            .GroupBy(c => c.DateAdded.Date)
            .OrderByDescending(g => g.Key)
            .Select(g => new CoffeeGroup(g.Key, new ObservableCollection<Coffee>(g)))
            .ToList();

        CoffeeGroups.Clear();
        foreach (var group in groups)
            CoffeeGroups.Add(group);

        var totalOunces = coffeeList.Sum(c => c.Ounces);
        TotalOuncesText = $"Total Coffee Consumed: {totalOunces}oz";
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

        var coffee = new Coffee
        {
            Name = Name,
            Ounces = ouncesValue,
            DateAdded = SelectedDate
        };

        await _coffeeService.AddCoffeeAsync(coffee);
        
        // Clear the entries
        Name = string.Empty;
        Ounces = string.Empty;

        // Reload the list
        await LoadCoffeesAsync();
    }

    [RelayCommand]
    async Task EditCoffee(Coffee coffee)
    {
        if (coffee == null)
            return;

        var navigationParameter = new Dictionary<string, object>
        {
            { "Coffee", coffee }
        };
        await Shell.Current.GoToAsync("EditCoffeePage", navigationParameter);
    }

    public async Task OnNavigatedToAsync()
    {
        await LoadCoffeesAsync();
    }

    public Task OnNavigatedFromAsync()
    {
        return Task.CompletedTask;
    }
}

public class CoffeeGroup : ObservableCollection<Coffee>
{
    public DateTime Date { get; }
    public string DisplayDate => Date.ToString("D");
    public double TotalOunces => this.Sum(c => c.Ounces);

    public CoffeeGroup(DateTime date, ObservableCollection<Coffee> coffees) : base(coffees)
    {
        Date = date;
    }
}