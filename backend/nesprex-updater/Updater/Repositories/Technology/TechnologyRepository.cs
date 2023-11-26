namespace Updater.Repositories.Technology;

using MongoDB.Driver;
using Shared.Domain.Entities;

public class TechnologyRepository(IMongoCollection<Technology> technologyRepository) : ITechnologyRepository
{
    public async Task<Technology> InsertAsync(Technology technology)
    {
        var existingTechnology = await technologyRepository
            .Find(x => x.Name == technology.Name)
            .FirstOrDefaultAsync();

        if (existingTechnology is null)
        {
            await technologyRepository.InsertOneAsync(technology);
        }
        else
        {
            var updateDefinition = Builders<Technology>.Update
                .Set(t => t.ImageUrl, technology.ImageUrl)
                .Set(t => t.Categories, technology.Categories);

            await technologyRepository.UpdateOneAsync(
                x => x.Id == existingTechnology.Id,
                updateDefinition);
        }

        return technology;
    }
}