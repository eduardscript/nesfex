namespace Shared.Extensions.Mongo.DependencyInjection;

using Configurations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

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

		services.AddSingleton<IMongoCollection<TEntity>>(
			sp =>
			{
				var mongoDb = sp.GetRequiredService<IMongoDatabase>();

				return mongoDb.GetCollection<TEntity>(collectionName);
			});

		return services;
	}

	public static IServiceCollection AddMongo(
		this IServiceCollection services)
	{
		services
			.AddOptions<MongoConfiguration>()
			.BindConfiguration("Mongo");

		var sp = services.BuildServiceProvider();

		var settings = sp.GetRequiredService<IOptions<MongoConfiguration>>();

		services.AddSingleton<IMongoClient>(_ => new MongoClient(settings.Value.ConnectionString));

		services.AddSingleton<IMongoDatabase>(
			_ =>
			{
				var mongoClient = sp.GetRequiredService<IMongoClient>();

				return mongoClient.GetDatabase(settings.Value.DatabaseName);
			});

		return services;
	}
}

internal class MongoConfiguration : IMongoConfiguration
{
	public string ConnectionString { get; set; } = default!;

	public string DatabaseName { get; set; } = default!;
}