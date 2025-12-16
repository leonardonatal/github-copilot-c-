// Mock FileSystem for testing
namespace MoreCoffee.Tests.Mocks;

public static class FileSystem
{
    public static string AppDataDirectory => Path.GetTempPath();
}