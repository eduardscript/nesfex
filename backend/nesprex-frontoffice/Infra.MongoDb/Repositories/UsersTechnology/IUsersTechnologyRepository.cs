using MongoDB.Driver;
using Shared.Domain.Entities;

namespace Infra.MongoDb.Repositories.UsersTechnology;

public interface IUserTechnologiesRepository
{
    Task<IEnumerable<UserTechnology>> GetUserTechnologies(
        Guid userId);

    Task AddTechnology(UserTechnology technology);

    Task UpdateCapsulesByTechnologyName(
        string technologyName,
        IEnumerable<SelectedCapsule> capsules,
        int quantity);

    Task DeleteCapsulesByTechnologyName(
        string technologyName,
        IEnumerable<SelectedCapsule> capsules);

    Task DeleteTechnology(string technologyName);
}

public class UserTechnologiesRepository : IUserTechnologiesRepository
{
    private readonly IMongoCollection<UserTechnology> _technologies;

    public UserTechnologiesRepository(IMongoDatabase database)
    {
        _technologies = database.GetCollection<UserTechnology>("Technologies");
    }

    public async Task<IEnumerable<UserTechnology>> GetUserTechnologies(Guid userId)
    {
        var filter = Builders<UserTechnology>.Filter.Eq(t => t.UserId, userId);
        return await _technologies.Find(filter).ToListAsync();
    }

    public async Task AddTechnology(UserTechnology technology)
    {
        await _technologies.InsertOneAsync(technology);
    }

    public async Task UpdateCapsulesByTechnologyName(string technologyName, IEnumerable<SelectedCapsule> capsules,
        int quantity)
    {
        var filter = Builders<UserTechnology>.Filter.Eq(t => t.Name, technologyName);
        var update = Builders<UserTechnology>.Update.Set(t => t.Capsules, capsules);
        await _technologies.UpdateOneAsync(filter, update);
    }

    public async Task DeleteCapsulesByTechnologyName(string technologyName, IEnumerable<SelectedCapsule> capsules)
    {
        var filter = Builders<UserTechnology>.Filter.Eq(t => t.Name, technologyName);
        var update = Builders<UserTechnology>.Update.PullFilter(t => t.Capsules, c => capsules.Contains(c));
        await _technologies.UpdateOneAsync(filter, update);
    }

    public async Task DeleteTechnology(string technologyName)
    {
        var filter = Builders<UserTechnology>.Filter.Eq(t => t.Name, technologyName);
        await _technologies.DeleteOneAsync(filter);
    }
}