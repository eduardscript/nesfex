using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Shared.Extensions.Configurations;

namespace Shared.Extensions.DependencyInjection;

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

    public static IServiceCollection AddMongo(this IServiceCollection services, IMongoConfiguration mongoConfiguration)
    {
        services.AddSingleton<IMongoClient>(sp => new MongoClient(mongoConfiguration.ConnectionString));

        services.AddSingleton<IMongoDatabase>(sp =>
        {
            var mongoClient = sp.GetRequiredService<IMongoClient>();

            return mongoClient.GetDatabase(mongoConfiguration.DatabaseName);
        });

        return services;
    }
}