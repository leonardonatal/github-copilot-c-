# Coffee Tracker App

A .NET MAUI application to track daily coffee consumption with detailed statistics and visualizations.

## Features

- Track coffee consumption with name and amount in ounces
- Group coffee entries by date
- View total coffee consumption
- Visualize consumption trends with interactive charts
- Store data locally using SQLite
- Cross-platform support (iOS, Android, macOS, Windows)

## Technology Stack

### Framework
- .NET MAUI 9.0 (Multi-platform App UI)
- C# 12.0

### Architecture & Patterns
- MVVM (Model-View-ViewModel)
- Dependency Injection
- Repository Pattern

### NuGet Packages
- `CommunityToolkit.Mvvm` (8.4.0) - MVVM architecture implementation with source generators
- `sqlite-net-pcl` (1.9.172) - SQLite local database integration
- `Syncfusion.Maui.Charts` (28.2.6) - Advanced charting capabilities
- `Microsoft.Maui.Controls` - Core MAUI controls
- `Microsoft.Extensions.Logging.Debug` - Debug logging support

### Storage
- SQLite database for local data persistence
- Stores coffee entries with:
  - Name
  - Amount (ounces)
  - Date added

### User Interface
- Modern XAML-based UI
- Responsive grid layouts
- Collection views with grouping
- Date picker for entry dates
- Bar charts for consumption visualization

## Getting Started

1. Clone the repository
2. Ensure you have .NET MAUI workload installed
3. Open the solution in Visual Studio 2022 or Visual Studio Code
4. Build and run the application

## Development Prerequisites

- .NET 9.0 SDK
- Visual Studio 2022 (Windows) or Visual Studio Code with .NET MAUI workload
- Xcode (for iOS/macOS development)
- Android SDK (for Android development)

## Platform Support

- iOS 15.0+
- Android API 21+
- macOS 15.0+ (via Mac Catalyst)
- Windows 10.0.17763.0+