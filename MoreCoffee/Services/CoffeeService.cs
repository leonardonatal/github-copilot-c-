using SQLite;
using MoreCoffee.Models;

namespace MoreCoffee.Services;

public class CoffeeService
{
    SQLiteAsyncConnection Database;
    private readonly BagOfCoffeeService _bagOfCoffeeService;

    public CoffeeService(BagOfCoffeeService bagOfCoffeeService)
    {
        var databasePath = Path.Combine(FileSystem.AppDataDirectory, "coffee.db");
        Database = new SQLiteAsyncConnection(databasePath);
        _bagOfCoffeeService = bagOfCoffeeService;
    }

    bool isInitialized;
    SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);

    public async Task InitializeAsync()
    {
        await semaphoreSlim.WaitAsync();
        try
        {
            if (!isInitialized)
            {
                await Init();
                isInitialized = true;
            }
        }
        finally
        {
            semaphoreSlim.Release();
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
            
        // If this coffee is from a bag, update the bag's remaining ounces
        if (coffee.BagOfCoffeeId > 0)
        {
            var success = await _bagOfCoffeeService.ConsumeCoffeeFromBagAsync(coffee.BagOfCoffeeId, coffee.Ounces);
            if (!success)
            {
                throw new InvalidOperationException("Not enough coffee in the selected bag");
            }
        }
        
        return await Database.InsertAsync(coffee);
    }

    public async Task<int> UpdateCoffeeAsync(Coffee coffee)
    {
        if (!isInitialized)
            await InitializeAsync();
        return await Database.UpdateAsync(coffee);
    }
}