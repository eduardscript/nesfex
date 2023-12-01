using MongoDB.Bson;

namespace Infra.MongoDb.Repositories.Technology;

using MongoDB.Driver;
using Shared.Domain.Entities;

public class PipelineBuilder
{
    private readonly List<BsonDocument> _pipeline = new();
    
    private BsonDocument _groupStage = new("$group",
        new BsonDocument
        {
            { "_id", "$_id" },
            { "Name", new BsonDocument("$first", "$Name") },
            { "ImageUrl", new BsonDocument("$first", "$ImageUrl") },
            { "Categories", new BsonDocument("$push", "$Categories") }
        }
    );

    public PipelineBuilder MatchTechnology(IList<string>? technologyNames)
    {
        if (technologyNames?.Any() == true)
        {
            var technologyMatchStage = new BsonDocument("$match",
                new BsonDocument("Name",
                    new BsonDocument("$in", new BsonArray(technologyNames))
                )
            );
            
            _pipeline.Add(technologyMatchStage);
        }

        return this;
    }

    public PipelineBuilder MatchCategory(IList<string>? categoryNames)
    {
        if (categoryNames?.Any() == true)
        {
            var matchCategoryStage = new BsonDocument("$match",
                new BsonDocument("Categories.Name",
                    new BsonDocument("$in", new BsonArray(categoryNames))
                )
            );

            _pipeline.Add(matchCategoryStage);

            var unwindStage = new BsonDocument("$unwind", "$Categories");
            _pipeline.Add(unwindStage);

            var matchCategoryUnwindStage = new BsonDocument("$match",
                new BsonDocument("Categories.Name",
                    new BsonDocument("$in", new BsonArray(categoryNames))
                )
            );

            _pipeline.Add(matchCategoryUnwindStage);

            _pipeline.Add(_groupStage);
        }

        return this;
    }

    public PipelineBuilder MatchCapsule(IList<string>? capsuleNames)
    {
        if (capsuleNames?.Any() == true)
        {
            var matchStage = new BsonDocument("$match", new BsonDocument("Categories.Capsules.Name",
                new BsonDocument("$in", new BsonArray(capsuleNames))));

            var unwindStage = new BsonDocument("$unwind", "$Categories");
            var unwindCapsuleStage = new BsonDocument("$unwind", "$Categories.Capsules");

            var projectStage = new BsonDocument("$project", new BsonDocument
            {
                { "Name", "$Name" },
                { "ImageUrl", "$ImageUrl" },
                {
                    "Categories", new BsonDocument
                    {
                        {
                            "$cond", new BsonArray
                            {
                                new BsonDocument("$in",
                                    new BsonArray { "$Categories.Capsules.Name", new BsonArray(capsuleNames) }),
                                new BsonDocument
                                {
                                    { "Name", "$Categories.Name" },
                                    { "Description", "$Categories.Description" },
                                    { "ImageUrl", "$Categories.ImageUrl" },
                                    {
                                        "Capsules", new BsonArray
                                        {
                                            new BsonDocument
                                            {
                                                { "Name", "$Categories.Capsules.Name" },
                                                { "Description", "$Categories.Capsules.Description" },
                                                { "Price", "$Categories.Capsules.Price" },
                                                { "Info", "$Categories.Capsules.Info" },
                                                { "ImageUrl", "$Categories.Capsules.ImageUrl" }
                                            }
                                        }
                                    }
                                },
                                BsonNull.Value
                            }
                        }
                    }
                }
            });

            _pipeline.Add(matchStage);
            _pipeline.Add(unwindStage);
            _pipeline.Add(unwindCapsuleStage);
            _pipeline.Add(matchStage);
            _pipeline.Add(projectStage);
            _pipeline.Add(_groupStage);
        }

        return this;
    }

    public PipelineBuilder SortByName()
    {
        _pipeline.Add(new BsonDocument("$sort", new BsonDocument
        {
            { "Name", 1 }
        }));

        return this;
    }

    public List<BsonDocument> Build()
    {
        return _pipeline;
    }
}

public class TechnologyRepository(IMongoCollection<Technology> technologyCollection) : ITechnologyRepository
{
    public async Task<IEnumerable<Technology>> GetTechnologiesAsync(TechnologyFilter? filter = null)
    {
        var pipelineBuilder = new PipelineBuilder();

        pipelineBuilder
            .MatchTechnology(filter?.TechnologyNames)
            .MatchCategory(filter?.CategoryNames)
            .MatchCapsule(filter?.CapsuleNames)
            .SortByName();

        var aggregationOptions = new AggregateOptions { AllowDiskUse = true };

        var cursor = await technologyCollection.AggregateAsync<Technology>(
            pipelineBuilder.Build(), // Build the pipeline
            aggregationOptions
        );

        return await cursor.ToListAsync();
    }
}