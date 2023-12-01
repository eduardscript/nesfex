using MongoDB.Bson;

namespace Infra.MongoDb.Repositories.Technology;

using MongoDB.Driver;
using Shared.Domain.Entities;

public class MatchBuilder
{
    private readonly List<BsonDocument> _matchDocument = new();

    public MatchBuilder AddMatch(string fieldName, IEnumerable<object> filterValues)
    {
        _matchDocument.Add(new BsonDocument("$match",
            new BsonDocument(fieldName,
                new BsonDocument("$in", new BsonArray(filterValues))
            )
        ));

        return this;
    }

    public List<BsonDocument> Build()
    {
        return _matchDocument;
    }
}

public class TechnologyRepository(IMongoCollection<Technology> technologyCollection) : ITechnologyRepository
{
    public async Task<IEnumerable<Technology>> GetTechnologiesAsync(TechnologyFilter? filter = null)
    {
        var pipeline = new List<BsonDocument>();

        if (filter?.TechnologyNames != null && filter.TechnologyNames.Any())
        {
            var technologyMatchStage = new BsonDocument("$match",
                new BsonDocument("Name",
                    new BsonDocument("$in", new BsonArray(filter.TechnologyNames))
                )
            );

            pipeline.Add(technologyMatchStage);
        }

        if (filter?.CategoryNames != null && filter.CategoryNames.Any())
        {
            var matchCategoryStage = new BsonDocument("$match",
                new BsonDocument("Categories.Name",
                    new BsonDocument("$in", new BsonArray(filter.CategoryNames))
                )
            );

            pipeline.Add(matchCategoryStage);

            var unwindStage = new BsonDocument("$unwind", "$Categories");
            pipeline.Add(unwindStage);

            var matchCategoryUnwindStage = new BsonDocument("$match",
                new BsonDocument("Categories.Name",
                    new BsonDocument("$in", new BsonArray(filter.CategoryNames))
                )
            );

            pipeline.Add(matchCategoryUnwindStage);

            var groupStage = new BsonDocument("$group",
                new BsonDocument
                {
                    { "_id", "$_id" },
                    { "Name", new BsonDocument("$first", "$Name") },
                    { "ImageUrl", new BsonDocument("$first", "$ImageUrl") },
                    {
                        "Categories", new BsonDocument("$push",
                            new BsonDocument
                            {
                                { "Name", "$Categories.Name" },
                                { "Description", "$Categories.Description" },
                                { "ImageUrl", "$Categories.ImageUrl" },
                                { "Capsules", "$Categories.Capsules" }
                            }
                        )
                    }
                }
            );

            pipeline.Add(groupStage);
        }

        if (filter?.CapsuleNames != null && filter.CapsuleNames.Any())
        {
            var matchStage = new BsonDocument("$match", new BsonDocument("Categories.Capsules.Name",
                new BsonDocument("$in", new BsonArray(filter.CapsuleNames))));

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
                                    new BsonArray { "$Categories.Capsules.Name", new BsonArray(filter.CapsuleNames) }),
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

            var groupByTechnology = new BsonDocument("$group", new BsonDocument
            {
                { "_id", "$_id" },
                { "Name", new BsonDocument("$first", "$Name") },
                { "ImageUrl", new BsonDocument("$first", "$ImageUrl") },
                { "Categories", new BsonDocument("$push", "$Categories") }
            });

            pipeline.Add(matchStage);
            pipeline.Add(unwindStage);
            pipeline.Add(unwindCapsuleStage);
            pipeline.Add(matchStage);
            pipeline.Add(projectStage);
            pipeline.Add(groupByTechnology);
        }

        var aggregationOptions = new AggregateOptions { AllowDiskUse = true };

        pipeline.Add(new BsonDocument("$sort", new BsonDocument
        {
            { "Name", 1 } // 1 for ascending order, -1 for descending order
        }));

        var cursor = await technologyCollection.AggregateAsync<Technology>(
            pipeline,
            aggregationOptions
        );

        return await cursor.ToListAsync();
    }
}