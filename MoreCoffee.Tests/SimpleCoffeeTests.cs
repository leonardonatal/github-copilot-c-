using Xunit;
using FluentAssertions;
using MoreCoffee.Models;
using SQLite;

namespace MoreCoffee.Tests;

public class SimpleCoffeeTests : IDisposable
{
    private readonly string _testDatabasePath;
    private readonly SQLiteAsyncConnection _database;

    public SimpleCoffeeTests()
    {
        _testDatabasePath = Path.Combine(Path.GetTempPath(), $"test_coffee_{Guid.NewGuid()}.db");
        _database = new SQLiteAsyncConnection(_testDatabasePath);
    }

    [Fact]
    public async Task Coffee_CanBeCreatedAndSaved()
    {
        // Arrange
        await _database.CreateTableAsync<Coffee>();
        var coffee = new Coffee
        {
            Name = "Test Coffee",
            Ounces = 8.0,
            DateAdded = DateTime.Now
        };

        // Act
        var result = await _database.InsertAsync(coffee);

        // Assert
        result.Should().BeGreaterThan(0);
        
        var savedCoffees = await _database.Table<Coffee>().ToListAsync();
        savedCoffees.Should().HaveCount(1);
        savedCoffees.First().Name.Should().Be("Test Coffee");
        savedCoffees.First().Ounces.Should().Be(8.0);
    }

    [Fact]
    public async Task Coffee_CanBeUpdated()
    {
        // Arrange
        await _database.CreateTableAsync<Coffee>();
        var coffee = new Coffee
        {
            Name = "Original Coffee",
            Ounces = 8.0,
            DateAdded = DateTime.Now
        };

        var insertResult = await _database.InsertAsync(coffee);
        coffee.Id = insertResult;

        // Act
        coffee.Name = "Updated Coffee";
        coffee.Ounces = 12.0;
        var updateResult = await _database.UpdateAsync(coffee);

        // Assert
        updateResult.Should().Be(1);
        
        var updatedCoffee = await _database.GetAsync<Coffee>(coffee.Id);
        updatedCoffee.Name.Should().Be("Updated Coffee");
        updatedCoffee.Ounces.Should().Be(12.0);
    }

    [Fact]
    public async Task MultipleCoffees_AreOrderedByDateDescending()
    {
        // Arrange
        await _database.CreateTableAsync<Coffee>();
        
        var coffee1 = new Coffee { Name = "Coffee 1", Ounces = 8.0, DateAdded = DateTime.Now.AddDays(-2) };
        var coffee2 = new Coffee { Name = "Coffee 2", Ounces = 10.0, DateAdded = DateTime.Now.AddDays(-1) };
        var coffee3 = new Coffee { Name = "Coffee 3", Ounces = 6.0, DateAdded = DateTime.Now };

        await _database.InsertAsync(coffee1);
        await _database.InsertAsync(coffee2);
        await _database.InsertAsync(coffee3);

        // Act
        var coffees = await _database.Table<Coffee>()
            .OrderByDescending(c => c.DateAdded)
            .ToListAsync();

        // Assert
        coffees.Should().HaveCount(3);
        coffees[0].Name.Should().Be("Coffee 3"); // Most recent first
        coffees[1].Name.Should().Be("Coffee 2");
        coffees[2].Name.Should().Be("Coffee 1"); // Oldest last
    }

    public void Dispose()
    {
        try
        {
            _database?.CloseAsync().Wait();
            
            if (File.Exists(_testDatabasePath))
            {
                // Retry file deletion a few times in case it's still locked
                for (int i = 0; i < 3; i++)
                {
                    try
                    {
                        File.Delete(_testDatabasePath);
                        break;
                    }
                    catch (IOException)
                    {
                        Thread.Sleep(100);
                    }
                }
            }
        }
        catch
        {
            // Ignore cleanup errors
        }
    }
}