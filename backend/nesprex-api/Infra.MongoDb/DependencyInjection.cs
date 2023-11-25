using Domain.Repositories;
using Infra.MongoDb.Configurations;
using Infra.MongoDb.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Infra.MongoDb;

public static class DependencyInjection
{
    public static IServiceCollection AddInfraMongoDb(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddOptions<MongoConfiguration>()
            .Bind(configuration.GetSection("Mongo"));

        services.AddSingleton<IMongoClient>(sp =>
        {
            var mongoDbSettings = sp.GetRequiredService<IOptions<MongoConfiguration>>();

            return new MongoClient(mongoDbSettings.Value.ConnectionString);
        });

        services.AddSingleton<IMongoDatabase>(sp =>
        {
            var mongoDbSettings = sp.GetRequiredService<IOptions<MongoConfiguration>>();

            var mongoClient = sp.GetRequiredService<IMongoClient>();

            return mongoClient.GetDatabase(mongoDbSettings.Value.DatabaseName);
        });

        services.AddSingleton<ITechnologyRepository, TechnologyRepository>();

        return services;
    }
}