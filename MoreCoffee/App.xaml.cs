using MoreCoffee.Services;

namespace MoreCoffee;

public partial class App : Application
{
    private readonly DatabaseMigrationService _migrationService;
    
    public App(DatabaseMigrationService migrationService)
    {
        _migrationService = migrationService;
        InitializeComponent();
        
        // Run database migrations
        Task.Run(async () => await _migrationService.MigrateAsync());
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new AppShell());
    }
}