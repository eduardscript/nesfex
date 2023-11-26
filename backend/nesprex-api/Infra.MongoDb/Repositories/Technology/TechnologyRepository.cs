namespace Infra.MongoDb.Repositories.Technology;

using MongoDB.Driver;
using Shared.Domain.Entities;

public class TechnologyRepository(IMongoCollection<Shared.Domain.Entities.Technology> technologyRepository) : ITechnologyRepository
{
    public async Task<IEnumerable<Technology>> GetTechnologiesAsync(TechnologyFilter filter)
    {
        // TODO - Implement filter and logic to build the filter
        var filterDefinition = Builders<Shared.Domain.Entities.Technology>.Filter.Empty;
        
        var technologies = await technologyRepository
            .Aggregate()
            .ToListAsync();
        
        return technologies;

    }
}