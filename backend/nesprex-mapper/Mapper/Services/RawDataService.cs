using System.Text.Json;
using Mapper.Dtos.RawData;
using Mapper.Utilities.Comparers;

namespace Mapper.Services;

public class RawDataService
{
    public static RawData GetRawData(string vertuoJsonString, string originalJsonString)
    {
        var serializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };

        var data1 = JsonSerializer.Deserialize<RawData>(vertuoJsonString, serializerOptions)!;
        var data2 = JsonSerializer.Deserialize<RawData>(originalJsonString, serializerOptions)!;
        
        var mergedData = new RawData(
            data1.Categories.Concat(data2.Categories).Distinct(new EntityEqualityComparer<ExternalNespressoCategory>(x => x.Id)).ToList(),
            data1.EnabledTechnologies.Concat(data2.EnabledTechnologies).Distinct(new EntityEqualityComparer<Technology>(x => x.Id!)).ToList(),
            data1.Products.Concat(data2.Products).Distinct(new EntityEqualityComparer<Product>(x => x.Id)).ToList());

        return mergedData;
    }
}

