using ConsoleApp1.Services;

namespace ConsoleApp1;

public class Program
{
    static string DesktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
    const string VertuoJsonPath = @"C:\Users\edscriptdev\Desktop\New folder\data\vertuo.json";
    const string OriginalJsonPath = @"C:\Users\edscriptdev\Desktop\New folder\data\original.json";

    public static async Task Main(string[] args)
    {
        var rawData = await RawDataService.GetRawData(VertuoJsonPath, OriginalJsonPath);

        var mappedData = rawData.MapRawDataToDomainEntities();
    }
}