using SQLite;
using MoreCoffee.Models;

namespace MoreCoffee.Services;

public class DatabaseMigrationService
{
    private readonly SQLiteAsyncConnection _database;
    private readonly string _databasePath;

    public DatabaseMigrationService()
    {
        _databasePath = Path.Combine(FileSystem.AppDataDirectory, "coffee.db");
        _database = new SQLiteAsyncConnection(_databasePath);
    }

    public async Task MigrateAsync()
    {
        // Create a backup of the database
        var backupPath = Path.Combine(FileSystem.AppDataDirectory, $"coffee_backup_{DateTime.Now:yyyyMMdd_HHmmss}.db");
        File.Copy(_databasePath, backupPath, true);

        // Check if Coffee table exists
        var coffeeTableInfo = await _database.GetTableInfoAsync("Coffee");
        if (coffeeTableInfo.Count > 0)
        {
            // Check if BagOfCoffeeId column exists
            var hasBagOfCoffeeIdColumn = coffeeTableInfo.Any(c => c.Name == "BagOfCoffeeId");
            var hasBagNameColumn = coffeeTableInfo.Any(c => c.Name == "BagName");

            if (!hasBagOfCoffeeIdColumn)
            {
                await _database.ExecuteAsync("ALTER TABLE Coffee ADD COLUMN BagOfCoffeeId INTEGER NOT NULL DEFAULT 0");
            }

            if (!hasBagNameColumn)
            {
                await _database.ExecuteAsync("ALTER TABLE Coffee ADD COLUMN BagName TEXT");
            }
        }

        // Check if BagOfCoffee table exists
        var bagTableInfo = await _database.GetTableInfoAsync("BagOfCoffee");
        if (bagTableInfo.Count > 0)
        {
            // Check if TotalOunces and RemainingOunces columns exist
            var hasTotalOuncesColumn = bagTableInfo.Any(c => c.Name == "TotalOunces");
            var hasRemainingOuncesColumn = bagTableInfo.Any(c => c.Name == "RemainingOunces");

            if (!hasTotalOuncesColumn)
            {
                await _database.ExecuteAsync("ALTER TABLE BagOfCoffee ADD COLUMN TotalOunces REAL NOT NULL DEFAULT 0");
            }

            if (!hasRemainingOuncesColumn)
            {
                await _database.ExecuteAsync("ALTER TABLE BagOfCoffee ADD COLUMN RemainingOunces REAL NOT NULL DEFAULT 0");
            }
        }
    }
}