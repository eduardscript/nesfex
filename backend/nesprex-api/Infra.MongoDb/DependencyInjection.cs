using Infra.MongoDb.Configurations;
using Infra.MongoDb.Repositories;
using Infra.MongoDb.Repositories.Technology;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Extensions.DependencyInjection;

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