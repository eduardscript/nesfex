using Domain.Repositories;
using Infra.MongoDb.Configurations;
using Infra.MongoDb.Extensions.DependencyInjection;
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

        services
            .AddMongo()
            .AddCollection<ITechnologyRepository, TechnologyRepository>("technologies");

        return services;
    }
}