using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Shared.Extensions.Mongo.Configurations;

namespace Shared.Extensions.Mongo.DependencyInjection;

public static class DependencyInjectionMongoDbExtensions
{
    public static IServiceCollection AddCollection<TEntity, TRepository, TImplementation>(
        this IServiceCollection services,
        string collectionName)
        where TRepository : class
        where TImplementation : class, TRepository
        where TEntity : class
    {
        services.AddSingleton<TRepository, TImplementation>();

        services.AddSingleton<IMongoCollection<TEntity>>(sp =>
        {
            var mongoDb = sp.GetRequiredService<IMongoDatabase>();

            return mongoDb.GetCollection<TEntity>(collectionName);
        });

        return services;
    }

    public static IServiceCollection AddMongo(this IServiceCollection services, IConfiguration configuration)
    {
        var settings = configuration.GetSection("Mongo").Get<MongoConfiguration>()!;

        services.AddSingleton<IMongoClient>(_ => new MongoClient(settings.ConnectionString));

        var sp = services.BuildServiceProvider();
        
        services.AddSingleton<IMongoDatabase>(_ =>
        {
            var mongoClient = sp.GetRequiredService<IMongoClient>();

            return mongoClient.GetDatabase(settings.DatabaseName);
        });

        return services;
    }
}

internal class MongoConfiguration : IMongoConfiguration
{
    public string ConnectionString { get; set; } = default!;
    
    public string DatabaseName { get; set; } = default!;
}