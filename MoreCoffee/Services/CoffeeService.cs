using SQLite;
using MoreCoffee.Models;

namespace MoreCoffee.Services;

public class CoffeeService
{
    SQLiteAsyncConnection Database;

    public CoffeeService()
    {
        var databasePath = Path.Combine(FileSystem.AppDataDirectory, "coffee.db");
        Database = new SQLiteAsyncConnection(databasePath);
        Database.CreateTableAsync<Coffee>().Wait();
    }

    public async Task<List<Coffee>> GetCoffeesAsync()
    {
        return await Database.Table<Coffee>().OrderByDescending(c => c.DateAdded).ToListAsync();
    }

    public async Task<int> AddCoffeeAsync(Coffee coffee)
    {
        return await Database.InsertAsync(coffee);
    }
}