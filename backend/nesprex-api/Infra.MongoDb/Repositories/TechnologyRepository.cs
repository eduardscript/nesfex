using Shared.Domain.Entities;
using Shared.Domain.Repositories;
using Shared.Domain.Repositories.Filters;
using MongoDB.Driver;

namespace Infra.MongoDb.Repositories;

public class TechnologyRepository(IMongoCollection<Technology> technologyRepository) : ITechnologyRepository
{
    public async Task<IEnumerable<Technology>> GetTechnologiesAsync(TechnologyFilter filter)
    {
        // TODO - Implement filter and logic to build the filter
        var filterDefinition = Builders<Technology>.Filter.Empty;
        
        var technologies = await technologyRepository
            .Aggregate()
            .ToListAsync();
        
        return technologies;

    }
}