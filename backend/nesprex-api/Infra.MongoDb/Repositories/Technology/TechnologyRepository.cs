using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver.Linq;

namespace Infra.MongoDb.Repositories.Technology;

using MongoDB.Driver;
using Shared.Domain.Entities;

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
            var categoryMatchStage = new BsonDocument("$match",
                new BsonDocument("Categories.Name",
                    new BsonDocument("$in", new BsonArray(filter.CategoryNames))
                )
            );
            
            pipeline.Add(categoryMatchStage);

            var unwindStage = new BsonDocument("$unwind", "$Categories");
            
            pipeline.Add(unwindStage);

            var categoryMatchUnwindStage = new BsonDocument("$match",
                new BsonDocument("Categories.Name",
                    new BsonDocument("$in", new BsonArray(filter.CategoryNames))
                )
            );

            pipeline.Add(categoryMatchUnwindStage);

            var projectStage = new BsonDocument("$project",
                new BsonDocument
                {
                    { "Name", "$Name" },
                    { "ImageUrl", "$ImageUrl" },
                    {
                        "Categories", new BsonArray
                        {
                            new BsonDocument
                            {
                                { "Name", "$Categories.Name" },
                                { "Description", "$Categories.Description" },
                                { "ImageUrl", "$Categories.ImageUrl" },
                                { "Capsules", "$Categories.Capsules" },
                            }
                        }
                    }
                }
            );

            pipeline.Add(projectStage);
        }
        
        if (filter?.CapsuleNames != null && filter.CapsuleNames.Any())
        {
            var capsuleMatchStage = new BsonDocument("$match",
                new BsonDocument("Categories.Capsules.Name",
                    new BsonDocument("$in", new BsonArray(filter.CapsuleNames))
                )
            );

            pipeline.Add(capsuleMatchStage);

            var unwindStage = new BsonDocument("$unwind", "$Categories");
            pipeline.Add(unwindStage);

            var unwindCapsuleStage = new BsonDocument("$unwind", "$Categories.Capsules");
            pipeline.Add(unwindCapsuleStage);

            var capsuleMatchUnwindStage = new BsonDocument("$match",
                new BsonDocument("Categories.Capsules.Name",
                    new BsonDocument("$in", new BsonArray(filter.CapsuleNames))
                )
            );
            pipeline.Add(capsuleMatchUnwindStage);

            var projectStage = new BsonDocument("$project",
                new BsonDocument
                {
                    { "Name", "$Name" },
                    { "ImageUrl", "$ImageUrl" },
                    {
                        "Categories", new BsonArray
                        {
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
                                            { "ImageUrl", "$Categories.Capsules.ImageUrl" },
                                        }
                                    }
                                },
                            }
                        }
                    }
                }
            );

            pipeline.Add(projectStage);
        }

        var aggregationOptions = new AggregateOptions { AllowDiskUse = true };

        var cursor = await technologyCollection.AggregateAsync<Technology>(
            pipeline,
            aggregationOptions
        );

        return await cursor.ToListAsync();
    }
}