using Domain.Entities.External;
using Domain.Entities.Internal;
using Domain.Services;
using Handler.Configurations;
using Microsoft.Extensions.Options;
using Technology = Domain.Entities.Internal.Technology;

namespace Handler.Services;

public interface IDataProvider
{
    Task<IEnumerable<Technology>> GetTechnologies();

    Task<IEnumerable<ImageData>> GetImagesData();
}

public class FileDataProvider : IDataProvider
{
    private readonly ServiceConfiguration _serviceConfiguration;
    private readonly ILogger<FileDataProvider> _logger;

    const string VertuoJsonPath = @"C:\Users\edscriptdev\Desktop\New folder\data\vertuo.json";
    const string OriginalJsonPath = @"C:\Users\edscriptdev\Desktop\New folder\data\original.json";

    private RawData? _cachedRawData;
    private DateTime _lastCacheUpdateTime;

    public FileDataProvider(IOptions<ServiceConfiguration> serviceConfiguration, ILogger<FileDataProvider> logger)
    {
        _serviceConfiguration = serviceConfiguration.Value;
        _logger = logger;
    }
    
    public async Task<IEnumerable<Technology>> GetTechnologies()
    {
        return (await GetCachedRawData()).MapRawDataToDomainEntities();
    }

    public async Task<IEnumerable<ImageData>> GetImagesData()
    {
        return (await GetCachedRawData()).MapRawDataToDomainEntities().GetDomainEntitiesImages();
    }

    private async Task<RawData> GetCachedRawData()
    {
        if (_cachedRawData is not null && !((_lastCacheUpdateTime - DateTime.Now).Duration() > _serviceConfiguration.Interval))
        {
            return _cachedRawData!;
        }

        _cachedRawData = await GetRawData();
        _lastCacheUpdateTime = DateTime.Now;
            
        _logger.LogError("Cache updated at: {time}", _lastCacheUpdateTime);

        return _cachedRawData;
    }

    private async Task<RawData> GetRawData()
    {
        var vertuoJson = await File.ReadAllTextAsync(VertuoJsonPath);
        var originalJson = await File.ReadAllTextAsync(OriginalJsonPath);

        return await RawDataService.GetRawData(vertuoJson, originalJson);
    }
}