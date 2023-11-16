using System.Text.Json;

namespace ConsoleApp1.Services;

public class RawDataService
{
    public static async Task<RawData> GetRawData(string vertuoJsonPath, string originalJsonPath)
    {
        var vertuoJsonString = await File.ReadAllTextAsync(vertuoJsonPath);
        var originalJsonString = await File.ReadAllTextAsync(originalJsonPath);

        var serializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };

        var data1 = JsonSerializer.Deserialize<RawData>(vertuoJsonString, serializerOptions)!;
        var data2 = JsonSerializer.Deserialize<RawData>(originalJsonString, serializerOptions)!;
        
        var mergedData = new RawData(
            data1.Categories.Concat(data2.Categories).Distinct(new EntityEqualityComparer<ExternalNespressoCategory>(x => x.Id)).ToList(),
            data1.EnabledTechnologies.Concat(data2.EnabledTechnologies).Distinct(new EntityEqualityComparer<Technology>(x => x.Id!)).ToList(),
            data1.Products.Concat(data2.Products).Distinct(new EntityEqualityComparer<Product>(x => x.Id)).ToList()
        );

        return mergedData;
    }
}

class EntityEqualityComparer<T> : IEqualityComparer<T>
{
    private readonly Func<T, string> _keySelector;

    public EntityEqualityComparer(Func<T, string> keySelector)
    {
        _keySelector = keySelector;
    }

    public bool Equals(T? x, T? y)
    {
        if (x == null || y == null)
            return false;

        return EqualityComparer<string>.Default.Equals(_keySelector(x), _keySelector(y));
    }

    public int GetHashCode(T obj)
    {
        return obj == null ? 0 : _keySelector(obj).GetHashCode();
    }
}