namespace Infra.MongoDb.Repositories.UsersTechnology;

using MongoDB.Bson;
using MongoDB.Driver;
using Shared.Domain.Entities;

public interface IUserTechnologiesRepository
{
	Task<UserTechnologies> GetUserTechnologies(
		Guid userId,
		string? technologyName = null);

	Task AddUserTechnology(
		UserTechnologies technologies);

	Task UpdateCapsuleQuantityByTechnologyName(
		Guid userId,
		string technologyName,
		SelectedCapsule capsule);

	Task DeleteCapsulesByTechnologyName(
		string technologyName,
		IEnumerable<SelectedCapsule> capsules);

	Task DeleteTechnology(
		string technologyName);
}

public class UserTechnologiesRepository : IUserTechnologiesRepository
{
	private readonly IMongoCollection<UserTechnologies> _technologies;

	public UserTechnologiesRepository(
		IMongoDatabase database) => _technologies = database.GetCollection<UserTechnologies>("Technologies");

	public async Task<UserTechnologies> GetUserTechnologies(
		Guid userId,
		string? technologyName = null)
	{
		var filter = Builders<UserTechnologies>.Filter.And(
			Builders<UserTechnologies>.Filter.Eq(
				t => t.UserId,
				userId));

		if (!string.IsNullOrWhiteSpace(technologyName))
		{
			filter &= Builders<UserTechnologies>.Filter.ElemMatch(
				t => t.Technologies,
				t => t.Name == technologyName);
		}

		return await _technologies
					.Find(filter)
					.SingleAsync();
	}

	public async Task AddUserTechnology(
		UserTechnologies technologies)
	{
		await _technologies.InsertOneAsync(technologies);
	}

	public async Task UpdateCapsuleQuantityByTechnologyName(
		Guid userId,
		string technologyName,
		SelectedCapsule capsule)
	{
		var filter = Builders<UserTechnologies>.Filter.Eq(
			t => t.UserId,
			userId) & Builders<UserTechnologies>.Filter.ElemMatch(
			t => t.Technologies,
			tech => tech.Name == technologyName && tech.Capsules.Any(c => c.Name == capsule.Name));

		var update = Builders<UserTechnologies>.Update.Set(
			"Technologies.$[tech].Capsules.$[cap].Quantity",
			capsule.Quantity);

		var arrayFilters = new List<ArrayFilterDefinition>
		{
			new BsonDocumentArrayFilterDefinition<BsonDocument>(
				new BsonDocument(
					"tech.Name",
					technologyName)),
			new BsonDocumentArrayFilterDefinition<BsonDocument>(
				new BsonDocument(
					"cap.Name",
					capsule.Name))
		};

		var updateOptions = new UpdateOptions { ArrayFilters = arrayFilters };

		await _technologies.UpdateOneAsync(
			filter,
			update,
			updateOptions);
	}

	public async Task DeleteCapsulesByTechnologyName(
		string technologyName,
		IEnumerable<SelectedCapsule> capsules) => throw new NotImplementedException();

	public async Task DeleteTechnology(
		string technologyName) => throw new NotImplementedException();
}