using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver.Linq;

namespace Infra.MongoDb.Repositories.Technology;

using MongoDB.Driver;
using Shared.Domain.Entities;

public class TechnologyRepository : ITechnologyRepository
{
    private readonly IMongoCollection<Technology> technologyCollection;

    public TechnologyRepository(IMongoCollection<Technology> technologyCollection)
    {
        this.technologyCollection = technologyCollection;
    }

    public async Task<IEnumerable<Technology>> GetTechnologiesAsync(TechnologyFilter? filter = null)
    {
        var pipeline = new List<BsonDocument>();

        var matchFilter = Builders<Technology>.Filter.Empty;

        if (filter?.TechnologyNames != null && filter.TechnologyNames.Any())
        {
            matchFilter &= Builders<Technology>.Filter.In(x => x.Name, filter.TechnologyNames);
        }

        if (filter?.CategoryNames != null && filter.CategoryNames.Any())
        {
            matchFilter &= Builders<Technology>
                .Filter
                .ElemMatch(x => x.Categories, x => filter.CategoryNames.Contains(x.Name));
        }

        var matchStage = new BsonDocument("$match", new BsonDocument("Categories.Name", "Limited Editions"));
        var unwindStage = new BsonDocument("$unwind", "$Categories");
        var secondMatchStage = new BsonDocument("$match", new BsonDocument("Categories.Name", "Limited Editions"));
        var projectStage = new BsonDocument("$project", new BsonDocument("_id", 0)
            .Add("category", new BsonDocument("name", "$Categories.Name")));

        pipeline.Add(matchStage);
        pipeline.Add(unwindStage);
        pipeline.Add(secondMatchStage);
        pipeline.Add(projectStage);

        var aggregationOptions = new AggregateOptions { AllowDiskUse = true };
        var cursor = await technologyCollection.AggregateAsync<Technology>(
            PipelineDefinition<Technology, Technology>.Create(pipeline),
            aggregationOptions
        );

        return await cursor.ToListAsync();
    }
}