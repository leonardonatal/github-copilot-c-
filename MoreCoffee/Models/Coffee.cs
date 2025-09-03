using SQLite;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MoreCoffee.Models;

public class Coffee : INotifyPropertyChanged
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    
    private string? name;
    public string? Name 
    { 
        get => name; 
        set => SetProperty(ref name, value); 
    }
    
    private double ounces;
    public double Ounces 
    { 
        get => ounces; 
        set => SetProperty(ref ounces, value); 
    }
    
    private DateTime dateAdded = DateTime.Now;
    public DateTime DateAdded 
    { 
        get => dateAdded; 
        set => SetProperty(ref dateAdded, value); 
    }

    // Add BagOfCoffeeId to connect with BagOfCoffee model
    private int bagOfCoffeeId;
    [Indexed]
    public int BagOfCoffeeId 
    { 
        get => bagOfCoffeeId; 
        set => SetProperty(ref bagOfCoffeeId, value); 
    }

    // Add property to store bag name for display purposes
    private string? bagName;
    public string? BagName 
    { 
        get => bagName; 
        set => SetProperty(ref bagName, value); 
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(storage, value))
            return false;

        storage = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}