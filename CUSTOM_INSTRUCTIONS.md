# GitHub Copilot Custom Instructions for Coffee Tracker App

## Project Overview
This is a .NET MAUI application for tracking coffee consumption and managing coffee inventory, built with .NET 9 using the MVVM pattern.

## Code Style Guidelines

### General Conventions
- Use C# 12.0 features when applicable
- Follow Microsoft's C# coding conventions
- Use PascalCase for class names, public properties, and methods
- Use camelCase for private fields and local variables
- Prefix private fields with underscore (e.g., `_coffeeService`)
- Use meaningful, descriptive names for all identifiers

### MAUI-Specific Guidelines
- Use CommunityToolkit.Mvvm source generators for properties and commands
- Prefer ObservableProperty over manual property change notifications
- Use RelayCommand and AsyncRelayCommand for command implementation
- Separate view (XAML) from view models (C#)
- Use XAML resource dictionaries for shared styles and resources

### Database/SQLite Guidelines
- Always initialize services before accessing database methods
- Use async/await for all database operations
- Implement proper error handling for database operations
- Follow the repository pattern used in existing services

### Model Guidelines
- Use SQLite attributes for defining primary keys and indexes
- Include proper data validation
- Implement ToString() overrides for debugging purposes
- Follow the established pattern for new models

## Implementation Patterns

### Adding New Views
1. Create a XAML view in the Views folder
2. Create a corresponding ViewModel in the ViewModels folder
3. Register both in MauiProgram.cs if needed
4. Add navigation in AppShell.xaml and register routes

### Adding New Database Models
1. Create model class in Models folder with SQLite attributes
2. Create a corresponding service in Services folder
3. Register the service in MauiProgram.cs
4. Initialize database table in the service's Init method

### Adding New Features
1. Consider which existing models and services are affected
2. Follow MVVM pattern for UI components
3. Implement proper validation and error handling
4. Add appropriate unit tests if applicable

## Common Tasks

### Implementing Validation
- Prefer client-side validation in view models
- Show appropriate user feedback for validation errors
- Use try/catch blocks for operations that might fail

### Adding Navigation
- Register routes in AppShell.xaml.cs
- Use Shell.Current.GoToAsync for navigation
- Pass parameters using QueryProperty attributes

### Working with SQLite
- Use the existing service pattern for database operations
- Always check for initialization before database operations
- Use async/await for all database access

## Testing Considerations
- Ensure UI components are properly initialized before testing
- Mock dependencies for unit testing
- Test both success and failure scenarios
- Validate data persistence scenarios

By following these custom instructions, you'll maintain consistency with the existing codebase and adhere to the established patterns and practices of the Coffee Tracker application.