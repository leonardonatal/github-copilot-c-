# GitHub Copilot Custom Instructions for xUnit Testing

## Testing Philosophy
Create comprehensive, maintainable, and reliable unit tests that follow the Arrange-Act-Assert (AAA) pattern and support the Coffee Tracker app's MVVM architecture and async database operations.

## xUnit Testing Framework Guidelines

### Test Project Structure
- Create separate test projects with naming convention: `{ProjectName}.Tests`
- Target the same .NET version as the main project (net9.0)
- Organize tests in folders mirroring the main project structure
- Use meaningful test class names ending with "Tests" (e.g., `CoffeeServiceTests`)

### Required NuGet Packages
```xml
<PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.11.1" />
<PackageReference Include="xunit" Version="2.9.2" />
<PackageReference Include="xunit.runner.visualstudio" Version="2.8.2" />
<PackageReference Include="Moq" Version="4.20.72" />
<PackageReference Include="FluentAssertions" Version="6.12.1" />
```

## Test Writing Guidelines

### Test Method Naming Convention
Use descriptive names that explain: `MethodName_Scenario_ExpectedResult`
```csharp
[Fact]
public async Task AddCoffeeAsync_WithValidCoffee_ShouldReturnPositiveId()

[Fact]
public async Task AddCoffeeAsync_WithInsufficientBagCoffee_ShouldThrowException()
```

### Test Structure - AAA Pattern
```csharp
[Fact]
public async Task MethodName_Scenario_ExpectedResult()
{
    // Arrange - Set up test data and dependencies
    var mockService = new Mock<IDependency>();
    var testObject = new ServiceUnderTest(mockService.Object);
    var inputData = new TestModel { Property = "value" };
    
    // Act - Execute the method being tested
    var result = await testObject.MethodUnderTest(inputData);
    
    // Assert - Verify the expected outcome
    result.Should().BeGreaterThan(0);
    mockService.Verify(x => x.ExpectedMethod(It.IsAny<int>()), Times.Once);
}
```

## MAUI-Specific Testing Patterns

### Testing Services with SQLite
- Use temporary database files for isolation
- Implement IDisposable for cleanup
- Create test-specific service implementations when needed
```csharp
public class CoffeeServiceTests : IDisposable
{
    private readonly string _testDatabasePath;
    private readonly CoffeeService _service;
    
    public CoffeeServiceTests()
    {
        _testDatabasePath = Path.Combine(Path.GetTempPath(), $"test_{Guid.NewGuid()}.db");
        _service = new TestCoffeeService(_testDatabasePath);
    }
    
    public void Dispose()
    {
        if (File.Exists(_testDatabasePath))
            File.Delete(_testDatabasePath);
    }
}
```

### Testing ViewModels with MVVM
- Mock service dependencies
- Test property change notifications
- Test command execution and CanExecute logic
- Verify navigation calls
```csharp
[Fact]
public async Task LoadCoffeeCommand_WhenExecuted_ShouldPopulateCoffeeList()
{
    // Arrange
    var mockCoffeeService = new Mock<ICoffeeService>();
    var expectedCoffees = new List<Coffee> { new Coffee { Name = "Test" } };
    mockCoffeeService.Setup(x => x.GetCoffeesAsync()).ReturnsAsync(expectedCoffees);
    
    var viewModel = new MainPageViewModel(mockCoffeeService.Object);
    
    // Act
    await viewModel.LoadCoffeeCommand.ExecuteAsync(null);
    
    // Assert
    viewModel.Coffees.Should().HaveCount(1);
    viewModel.Coffees.First().Name.Should().Be("Test");
}
```

## Test Categories and Coverage

### Essential Test Types
1. **Happy Path Tests**: Test normal, expected scenarios
2. **Error Handling Tests**: Test exception scenarios and edge cases
3. **Boundary Tests**: Test limits and edge values
4. **Async Operation Tests**: Test async/await patterns properly
5. **Dependency Interaction Tests**: Verify mock calls and interactions

### Database Testing Patterns
```csharp
[Fact]
public async Task AddCoffeeAsync_WithBagReference_ShouldUpdateBagInventory()
{
    // Arrange
    await _service.InitializeAsync();
    var mockBagService = new Mock<IBagOfCoffeeService>();
    mockBagService.Setup(x => x.ConsumeCoffeeFromBagAsync(1, 8.0)).ReturnsAsync(true);
    
    var coffee = new Coffee { BagOfCoffeeId = 1, Ounces = 8.0 };
    
    // Act
    var result = await _service.AddCoffeeAsync(coffee);
    
    // Assert
    result.Should().BeGreaterThan(0);
    mockBagService.Verify(x => x.ConsumeCoffeeFromBagAsync(1, 8.0), Times.Once);
}
```

## Assertion Guidelines

### Use FluentAssertions for Readability
```csharp
// Preferred
result.Should().NotBeNull();
coffees.Should().HaveCount(3);
coffee.Name.Should().Be("Expected Name");
action.Should().Throw<ArgumentException>().WithMessage("Expected message");

// Avoid basic assertions
Assert.NotNull(result);
Assert.Equal(3, coffees.Count);
```

### Async Testing Best Practices
```csharp
// Test async methods properly
[Fact]
public async Task AsyncMethod_Scenario_ExpectedResult()
{
    // Use async/await, not .Result or .Wait()
    var result = await serviceUnderTest.AsyncMethodAsync();
    result.Should().NotBeNull();
}

// Test exception handling in async methods
[Fact]
public async Task AsyncMethod_WithInvalidData_ShouldThrowException()
{
    var action = async () => await service.MethodAsync(invalidData);
    await action.Should().ThrowAsync<ArgumentException>();
}
```

## Mocking Guidelines

### Service Dependencies
```csharp
// Mock external dependencies
var mockCoffeeService = new Mock<ICoffeeService>();
mockCoffeeService.Setup(x => x.GetCoffeesAsync())
    .ReturnsAsync(new List<Coffee>());

// Verify interactions
mockService.Verify(x => x.ExpectedMethod(It.IsAny<Coffee>()), Times.Once);
mockService.VerifyNoOtherCalls();
```

### Setup Return Values
```csharp
// Simple return values
mock.Setup(x => x.GetAsync(It.IsAny<int>())).ReturnsAsync(expectedValue);

// Conditional returns
mock.Setup(x => x.ValidateAsync(It.Is<Coffee>(c => c.Ounces > 0)))
    .ReturnsAsync(true);
mock.Setup(x => x.ValidateAsync(It.Is<Coffee>(c => c.Ounces <= 0)))
    .ReturnsAsync(false);
```

## Test Organization

### Test Class Setup
```csharp
public class ServiceTests : IDisposable
{
    private readonly Mock<IDependency> _mockDependency;
    private readonly ServiceUnderTest _service;
    
    public ServiceTests()
    {
        _mockDependency = new Mock<IDependency>();
        _service = new ServiceUnderTest(_mockDependency.Object);
    }
    
    // Multiple test methods...
    
    public void Dispose()
    {
        // Cleanup resources
    }
}
```

### Test Data Builders
```csharp
// Create helper methods for common test data
private Coffee CreateValidCoffee() => new Coffee
{
    Name = "Test Coffee",
    Ounces = 8.0,
    DateAdded = DateTime.Now
};

private Coffee CreateCoffeeWithBag(int bagId) => new Coffee
{
    Name = "Bag Coffee",
    Ounces = 6.0,
    BagOfCoffeeId = bagId,
    DateAdded = DateTime.Now
};
```

## Running and Debugging Tests

### Command Line
```bash
# Run all tests
dotnet test

# Run tests with verbose output
dotnet test --logger "console;verbosity=detailed"

# Run specific test class
dotnet test --filter "ClassName=CoffeeServiceTests"
```

### Visual Studio
- Use Test Explorer for visual test management
- Set breakpoints in tests for debugging
- Use "Run All Tests" for comprehensive testing

## Coverage and Quality

### Aim for High Coverage
- Target 80%+ code coverage for critical business logic
- Focus on testing public methods and important private methods
- Don't aim for 100% coverage on trivial code

### Test Quality Checklist
- [ ] Tests are independent and can run in any order
- [ ] Each test verifies one specific behavior
- [ ] Test names clearly describe what is being tested
- [ ] Arrange-Act-Assert pattern is followed
- [ ] Proper cleanup is implemented
- [ ] Edge cases and error scenarios are covered
- [ ] Async operations are properly tested

## Integration with Coffee Tracker App

### Service Testing Priorities
1. Database operations (CoffeeService, BagOfCoffeeService)
2. Business logic validation
3. MVVM command execution
4. Data transformation and calculations
5. Error handling and recovery

By following these guidelines, create comprehensive, maintainable xUnit tests that ensure the reliability and quality of the Coffee Tracker application while supporting its MVVM architecture and async operations.