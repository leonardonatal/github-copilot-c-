using SQLite;

namespace MoreCoffee.Models;

public class Coffee
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string? Name { get; set; }
    public double Ounces { get; set; }
    public DateTime DateAdded { get; set; } = DateTime.Now;
}