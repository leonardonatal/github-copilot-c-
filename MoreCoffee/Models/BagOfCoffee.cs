using SQLite;

namespace MoreCoffee.Models;

public class BagOfCoffee
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string? Roaster { get; set; }
    public DateTime RoastDate { get; set; }
    public string? Name { get; set; }
    public RoastLevel RoastLevel { get; set; }
    public string? Origin { get; set; }
    public DateTime DateAdded { get; set; } = DateTime.Now;
    public string? TastingNotes { get; set; }
    
    // Add tracking for total and remaining quantity
    public double TotalOunces { get; set; } = 0;
    public double RemainingOunces { get; set; } = 0;
    
    // Helper property to calculate percentage remaining
    [Ignore]
    public double PercentRemaining => TotalOunces > 0 ? (RemainingOunces / TotalOunces) * 100 : 0;
    
    // Helper property to get display text for the bag
    [Ignore]
    public string DisplayName => $"{Roaster} - {Name} ({RemainingOunces:F1}oz / {TotalOunces:F1}oz)";
}