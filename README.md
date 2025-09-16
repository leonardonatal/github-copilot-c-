# Coffee Tracker App (MoreCoffee)

A .NET MAUI application to track daily coffee consumption with detailed statistics and visualizations.

## Technology Stack

### Framework
- .NET MAUI 9.0 (Multi-platform App UI)
- C# 12.0

### NuGet Packages
- `CommunityToolkit.Mvvm` (8.4.0) - MVVM architecture implementation with source generators
- `sqlite-net-pcl` (1.9.172) - SQLite local database integration
- `Syncfusion.Maui.Charts` (28.2.7) - Advanced charting capabilities
- `Microsoft.Maui.Controls` (9.0.40) - Core MAUI controls
- `Microsoft.Extensions.Logging.Debug` (9.0.2) - Debug logging support

## Features

- Track coffee consumption with name and amount in ounces
- Manage your inventory of coffee bags with detailed information
- Group coffee entries by date and type
- View total coffee consumption and consumption patterns
- Visualize consumption trends with interactive charts
- Store data locally using SQLite
- Cross-platform support (iOS, Android, macOS, Windows)

## Installation Guide

### Prerequisites

To build and run this application, you'll need:

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Visual Studio 2022](https://visualstudio.microsoft.com/vs/) (17.10 or later)
- For Mac development:
  - [Visual Studio for Mac](https://visualstudio.microsoft.com/vs/mac/) or
  - [Visual Studio Code](https://code.visualstudio.com/) with the [C# Dev Kit extension](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csdevkit)

### Getting Started

1. Clone the repository
   ```bash
   git clone https://github.com/jamesmontemagno/app-coffeetracker.git
   cd app-coffeetracker
   ```

2. Install Dependencies
   The project uses NuGet packages which will be restored automatically when building.

3. Build and Run
   
   **Using Visual Studio 2022:**
   - Open the solution file `MoreCoffee.sln`
   - Select your target platform (Windows or macOS)
   - Press F5 to build and run the application

## Unit Testing

This project follows comprehensive testing practices using xUnit and follows our custom testing guidelines defined in [.github/copilot-xunit-testing-instructions.md](.github/copilot-xunit-testing-instructions.md).

### Testing Philosophy

We create comprehensive, maintainable, and reliable unit tests that follow the Arrange-Act-Assert (AAA) pattern and support the Coffee Tracker app's MVVM architecture and async database operations.

### Running Tests

**Using Visual Studio:**
- Open Test Explorer (Test > Test Explorer)
- Build the solution
- Click "Run All Tests"

**Using Command Line:**
```bash
# Run all tests
dotnet test MoreCoffee.Tests

# Run tests with verbose output
dotnet test MoreCoffee.Tests --logger "console;verbosity=detailed"

# Run specific test class
dotnet test --filter "ClassName=SimpleCoffeeTests"
```

### Test Project Structure

The `MoreCoffee.Tests` project contains:
- **SimpleCoffeeTests.cs** - Basic database operation tests for Coffee model
- **Service Tests** - Comprehensive tests for service layer functionality
- **Mock implementations** - Test-specific versions of services for isolation

### Creating New Tests

When adding new tests, follow the guidelines in our [xUnit Testing Instructions](.github/copilot-xunit-testing-instructions.md):

1. **Use the AAA Pattern**: Arrange-Act-Assert structure
2. **Meaningful Names**: `MethodName_Scenario_ExpectedResult`
3. **FluentAssertions**: Use readable assertions like `.Should().Be()`
4. **Test Isolation**: Each test uses its own temporary database
5. **Proper Cleanup**: Implement `IDisposable` for resource cleanup

### Example Test Structure

```csharp
[Fact]
public async Task AddCoffeeAsync_WithValidCoffee_ShouldReturnPositiveId()
{
    // Arrange - Set up test data and dependencies
    await _database.CreateTableAsync<Coffee>();
    var coffee = new Coffee { Name = "Test Coffee", Ounces = 8.0 };
    
    // Act - Execute the method being tested
    var result = await _database.InsertAsync(coffee);
    
    // Assert - Verify the expected outcome
    result.Should().BeGreaterThan(0);
}
```

For detailed testing guidelines and patterns specific to this project, refer to our [comprehensive xUnit testing instructions](.github/copilot-xunit-testing-instructions.md).

## Demo Presentation Guide

### Setup for Demo

1. **Prepare Environment**:
   - Ensure all prerequisites are installed (see Installation Guide)
   - Build the app for your target platform
   - Create sample data by adding coffee bags and consumption entries

2. **Demo Flow**:
   - **Introduction** (2 min): Explain the app's purpose and target audience
   - **App Overview** (3 min): Show the main screens and navigation
   - **Feature Demonstration** (10 min: Walk through each core feature
   - **Technical Implementation** (5 min): Highlight key technical aspects
   - **Q&A** (5 min): Address questions from the audience

3. **Sample Demo Script**:
   ```
   "Welcome to MoreCoffee, a cross-platform app built with .NET MAUI that helps coffee enthusiasts track their coffee consumption and manage their coffee inventory.
   
   Let me walk you through the main features:
   
   First, here's the home screen where you can see your recent coffee consumption and quickly add new entries.
   
   Next, let's look at the Coffee Bags page where you can manage your inventory. You can add details like the roaster, origin, roast level, and tasting notes.
   
   Finally, the Statistics page gives you insights into your coffee consumption patterns with interactive charts.
   
   Now, let's add a new coffee bag and log some consumption to see how it all works together..."
   ```

### Key Demo Points

1. **Demonstrate basic code completion**:
   - Open a file (e.g., `MainPage.xaml.cs`)
   - Start writing a new method and observe how Copilot suggests implementations
   - Example: `private async Task LoadCoffeeData()`

2. **Show context-aware suggestions**:
   - Highlight how Copilot understands your project's models and patterns
   - Example: When working with the `RoastLevel` enum, it suggests appropriate values

3. **Demonstrate comment-to-code**:
   - Write a detailed comment describing functionality
   - Example: `// Create a method to filter coffee by roast level and return the total ounces consumed`
   - Show how Copilot generates the entire implementation

4. **Show multi-file awareness**:
   - Demonstrate how Copilot can suggest code that references classes or methods defined in other files
   - Example: Implementing a new feature that uses the `CoffeeService`


### Copilot Chat Reference (For Presenter)

Before transitioning to GitHub Copilot inline suggestions, ensure you've covered these Copilot Chat features:

1. **@workspace command**: Enables Copilot to access and understand your entire workspace
   ```
   @workspace Help me understand the structure of this project
   ```

2. **Slash commands**: Special instructions for specific tasks
   ```
   /explain [code snippet]
   /fix [problematic code]
   /tests [code to test]
   /docs [code to document]
   ```

3. **# file inclusion**: Reference specific files in your questions
   ```
   # MoreCoffee/Models/RoastLevel.cs How can I extend this enum?
   ```

4. **Model switching**: Changing between different AI models
   - Gemini 2.5: Faster responses, good for simpler tasks
   - GPT-5 / Claude Sonnet 4: More capable for complex reasoning, code generation, and understanding

## Using GitHub Copilot Inline Mode

### Focused Context with Inline Mode

Inline mode allows you to get more focused suggestions by working directly within your code. This approach provides less scope context but often results in more precise code completion for specific tasks.

Example use case: Adding friendly names to enums

```
// You can ask Copilot inline to enhance the RoastLevel enum by adding 
// friendly display names using attributes:

public enum RoastLevel
{
    [Description("Light Roast")] // Copilot can suggest these attributes 
    Light,
    
    [Description("Medium Light Roast")]
    MediumLight,
    
    // ... and so on
}
```

Using inline mode is perfect for small, targeted changes where you want GitHub Copilot to focus specifically on the code you're currently working with, rather than considerandoing the broader project context.

## Using Copilot for Debugging and Bug Fixing

### Analyzing Exceptions with Copilot

GitHub Copilot can help identify and fix potential bugs in your code. Let's demonstrate this with a real example from our application.

In the `EditCoffeeViewModel.cs` file, the `SaveCoffee` method throws an exception when editing coffee with invalid ounce values:

```
[RelayCommand]
async Task SaveCoffee()
{
    if (Coffee == null)
        return;

    if (Coffee.Ounces <= 0)
        throw new ArgumentException("Ounces must be greater than zero.");

    // Additional code...
}
```

To analyze this with Copilot:

1. **Set a breakpoint** in the `SaveCoffee` method
2. **Run the application in debug mode**
3. **Edit a coffee entry** with invalid ounces (e.g., entering text or a negative number)
4. **When the breakpoint is hit**, hover over variables to inspect their values
5. **Ask Copilot** how to improve this code with proper validation:

Example Copilot Chat query:
```
I have an exception thrown when users enter invalid values for coffee ounces.
How can I implement input validation to prevent this exception and provide a better user experience?
```

Copilot might suggest implementing client-side validation:
- Adding input validation in the XAML
- Using data annotations in the model
- Adding validation logic before saving
- Implementing proper error handling with user-friendly messages

This approach demonstrates how to use GitHub Copilot as a powerful debugging assistant to identify, understand, and fix potential bugs in your application.

## Using GitHub Copilot in Agent Mode

### Multi-File Feature Implementation

GitHub Copilot's Agent mode excels at complex tasks that span multiple files. Let's demonstrate this with a practical example: adding a "Delete Coffee" button to our app.

To implement this feature, we need to modify three different files:

1. **Add a Delete button to EditCoffeePage.xaml**
2. **Create a DeleteCoffee command in EditCoffeeViewModel.cs**
3. **Implement a DeleteCoffeeAsync method in CoffeeService.cs**

Here's how to use Copilot Agent mode for this task:

1. **Ask Copilot to implement the feature across all required files:**
   ```
   @workspace I need to add a "Delete Coffee" feature that allows users to remove coffee entries. 
   This requires:
   1. Adding a red "Delete" button to EditCoffeePage.xaml below the Save button
   2. Creating a DeleteCoffee command in EditCoffeeViewModel.cs with user confirmation
   3. Implementing a DeleteCoffeeAsync method in CoffeeService.cs
   
   Can you generate the code for all three files?
   ```

2. **Review Copilot's multi-file implementation:**
   - Examine how it adds the button with proper styling and positioning
   - Check the command implementation with confirmation dialog
   - Verify the service method properly deletes the entry from the database

3. **Apply the suggested changes:**
   - Use Copilot's suggestions to update each file
   - Make any necessary adjustments to match your project's specific needs

This example showcases GitHub Copilot's ability to understand complex relationships between files and implement features that span multiple components of your application architecture.

## Using GitHub Copilot for Custom Git Commit Messages

### Setting Up Custom Commit Message Templates

GitHub Copilot can help you generate consistent, informative commit messages that follow your team's conventions. Let's set up a custom commit message format with a summary paragraph and emoji-tagged bullet points.

#### Configure Visual Studio Settings

1. **Open Visual Studio 2022**
2. Navigate to __Tools > Options > GitHub Copilot__
3. Under the __Commit Message__ section, enable __Use custom commit message template__
4. Click __Edit Template__ to customize your format

#### Creating a Custom Commit Template

You can use Copilot to help generate a template. For example, ask Copilot:

```
Help me create a Git commit message template with:
1. First line as a concise summary of changes (50-72 characters)
2. A blank line after the summary
3. A paragraph explaining the changes in more detail
4. A bullet list with emoji prefixes for different types of changes
```

#### Example Custom Commit Format

```
[Summary of changes in 50-72 characters]

Detailed explanation of the changes made and the reasoning behind them.
This provides context for future developers reviewing the commit history.

• 🚀 [New feature details]
• 🐛 [Bug fix details]
• ⚡ [Performance improvement details]
• 📚 [Documentation update details]
• 🧹 [Code cleanup details]
```

#### Using Copilot to Generate Commit Messages

Once your template is set up, you can ask Copilot to generate a commit message based on your staged changes:

1. Stage your changes using `git add`
2. Open the commit dialog in Visual Studio
3. Type a comment like: `// Generate a commit message for these changes following our template`
4. Copilot will analyze your changes and generate a commit message following your template

#### Example Copilot-Generated Commit Message

```
Implement coffee deletion functionality with user confirmation

Added the ability for users to delete coffee entries from the database with
a confirmation dialog to prevent accidental deletions.

• 🚀 Added delete button to EditCoffeePage.xaml
• ✨ Created DeleteCoffee command in EditCoffeeViewModel.cs
• 🔧 Implemented DeleteCoffeeAsync method in CoffeeService.cs
• 🧪 Added confirmation dialog before deletion
```

This approach ensures consistency in your commit messages and makes your repository history more informative and easier to navigate.

## Using GitHub Extensions for Documentation

### Installing Mermaid Extensions from GitHub Marketplace

GitHub natively supports Mermaid diagrams in markdown files, but to enhance your development experience with advanced visualization capabilities, you can install additional extensions:

 **Access GitHub Marketplace**:
   - Go to [GitHub Marketplace](https://github.com/marketplace)
   - Search for "Mermaid" to find related extensions
   - [Mermaid Chart](https://github.com/marketplace/mermaid-chart): Create, edit, and share diagrams using Mermaid syntax
   - For web-based tools: Follow the authentication process to connect to your GitHub account

### Generating Documentation with Mermaid

Using Mermaid, you can create various diagrams to document your application. Here's how to create an enhanced ER diagram for the Coffee Tracker models:

1. **Create a Basic ER Diagram**:
   Create a basic ER diagram showing my data models for (BAG_OF_COFFEE, COFFEE, ROAS_LEVEL) in a separate documentation md file.


## Using Github Copilot for Custom Instructions

### What are Custom Instructions?

Custom instructions are personalized prompts or guidelines that you provide to GitHub Copilot to tailor its behavior and responses to better suit your coding style and project requirements.
They help Copilot understand your preferences, such as preferred coding conventions, specific libraries or frameworks you use, and any other relevant context that can improve the quality of its suggestions.

##Purpose of Custom Instructions

    1. **Tailor Suggestions**: Custom instructions allow you to guide Copilot's code suggestions to align with your coding style and project needs.
    2. **Improve Relevance**: By providing context about your project, Copilot can generate more relevant and useful code snippets.
    3. **Enhance Productivity**: Custom instructions can help Copilot generate code that requires fewer modifications, speeding up your development process.
    
 ### How to Set Up Custom Instructions

 You can ask Copilot to set up custom instructions by providing specific guidelines or preferences. For example:
 ```
 @workspace Set up custom instructions for GitHub Copilot to:
 1. Use C# coding conventions, such as PascalCase for method names and camelCase for local variables.
 2. Prefer using async/await for asynchronous operations.
 3. Include XML documentation comments for public methods and classes.
 4. Use the MVVM pattern for UI development.
 5. Follow SOLID principles in class design.
 ```
 Custom instructions help Copilot understand your coding preferences and project context, leading to more accurate and useful code suggestions.