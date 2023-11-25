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
    private readonly DataConfiguration _dataConfiguration;
    private readonly ILogger<FileDataProvider> _logger;

    private RawData? _cachedRawData;
    private DateTime _lastCacheUpdateTime;

    public FileDataProvider(
        IOptions<ServiceConfiguration> serviceConfiguration, 
        IOptions<DataConfiguration> dataConfiguration, 
        ILogger<FileDataProvider> logger)
    {
        _serviceConfiguration = serviceConfiguration.Value;
        _dataConfiguration = dataConfiguration.Value;
        _logger = logger;
    }
    
    public async Task<IEnumerable<Technology>> GetTechnologies()
    {
        return (await GetCachedRawData()).MapRawDataToDomainEntities();
    }

    public async Task<IEnumerable<ImageData>> GetImagesData()
    {
        return (await GetCachedRawData()).MapRawDataToDomainEntities()
            .GetDomainEntitiesImages(_serviceConfiguration.Mapper.ImageUrl);
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
        List<string> rawTechnologiesJson = new();
        foreach (var pathConfiguration in _dataConfiguration.Paths.Where(p => p.Name.StartsWith("technology")))
        {
            rawTechnologiesJson.Add(await File.ReadAllTextAsync(pathConfiguration.Path));
        }
      
        // TODO: Refactor this to be more generic
        return RawDataService.GetRawData(rawTechnologiesJson.First(), rawTechnologiesJson.Last());
    }
}