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
    }

    bool isInitialized;
    public async Task InitializeAsync()
    {
        if (!isInitialized)
        {
            await Init();
            isInitialized = true;
        }
    }
    async Task Init()
    {
        await Database.CreateTableAsync<Coffee>();
    }

    public async Task<List<Coffee>> GetCoffeesAsync()
    {
        if (!isInitialized)
            await InitializeAsync();
        return await Database.Table<Coffee>().OrderByDescending(c => c.DateAdded).ToListAsync();
    }

    public async Task<int> AddCoffeeAsync(Coffee coffee)
    {
        if (!isInitialized)
            await InitializeAsync();
        return await Database.InsertAsync(coffee);
    }

    public async Task<int> UpdateCoffeeAsync(Coffee coffee)
    {
        if (!isInitialized)
            await InitializeAsync();
        return await Database.UpdateAsync(coffee);
    }

    public async Task<int> DeleteCoffeeAsync(Coffee coffee)
    {
        if (!isInitialized)
            await InitializeAsync();

        var existingCoffee = await Database.Table<Coffee>().Where(c => c.Id == coffee.Id).FirstOrDefaultAsync();
        if (existingCoffee == null)
            return 0;

        return await Database.DeleteAsync(coffee);
    }
}