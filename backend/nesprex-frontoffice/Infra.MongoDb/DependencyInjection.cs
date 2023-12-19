namespace Infra.MongoDb;

using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization.Conventions;
using Repositories.Users;
using Repositories.UsersTechnology;
using Shared.Domain.Entities;
using Shared.Extensions.Mongo.DependencyInjection;

public static class DependencyInjection
{
	public static IServiceCollection AddInfraMongoDb(
		this IServiceCollection services)
	{
		var conventionPack = new ConventionPack { new IgnoreExtraElementsConvention(true) };
		ConventionRegistry.Register(
			"IgnoreExtraElements",
			conventionPack,
			_ => true);

		services
			.AddMongo()
			.AddCollection<UserTechnologies, IUserTechnologiesRepository, UserTechnologiesRepository>(
				"user_technologies")
			.AddCollection<User, IUsersRepository, UsersRepository>("users");

		return services;
	}
}