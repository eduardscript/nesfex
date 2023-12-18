using Infra.MongoDb.Repositories.Users;
using Infra.MongoDb.Repositories.UsersTechnology;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization.Conventions;
using Shared.Domain.Entities;
using Shared.Extensions.Mongo.DependencyInjection;

namespace Infra.MongoDb;

public static class DependencyInjection
{
    public static IServiceCollection AddInfraMongoDb(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var conventionPack = new ConventionPack { new IgnoreExtraElementsConvention(true) };
        ConventionRegistry.Register("IgnoreExtraElements", conventionPack, type => true);
        
        services
            .AddMongo(configuration)
            .AddCollection<UserTechnologies, IUserTechnologiesRepository, UserTechnologiesRepository>("user_technologies")
            .AddCollection<User, IUsersRepository, UsersRepository>("users");

        return services;
    }
}