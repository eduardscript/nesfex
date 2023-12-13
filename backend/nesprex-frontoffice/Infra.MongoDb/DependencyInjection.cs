using Infra.MongoDb.Repositories.UsersTechnology;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Domain.Entities;
using Shared.Extensions.Mongo.DependencyInjection;

namespace Infra.MongoDb;

public static class DependencyInjection
{
    public static IServiceCollection AddInfraMongoDb(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddMongo(configuration)
            .AddCollection<UserTechnology, IUserTechnologiesRepository, UserTechnologiesRepository>("user_technologies");

        return services;
    }
}