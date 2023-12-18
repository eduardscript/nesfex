using MongoDB.Driver;
using Shared.Domain.Entities;

namespace Infra.MongoDb.Repositories.Users;

public interface IUsersRepository
{
    Task<User> GetByUsername(string username);

    Task<User> GetById(Guid userId);

    Task<User> UpsertRefreshToken(Guid userId, RefreshToken refreshToken);

    Task Insert(User user);
}

public class UsersRepository(IMongoCollection<User> users) : IUsersRepository
{
    public async Task<User> GetByUsername(string username)
    {
        var filter = Builders<User>.Filter.Eq("Username", username);

        var result = await users
            .Find(filter)
            .SingleOrDefaultAsync();

        return result;
    }

    public async Task<User> GetById(Guid userId)
    {
        var filter = Builders<User>.Filter.Eq("_id", userId);

        var result = await users
            .Find(filter)
            .SingleOrDefaultAsync();

        return result;
    }

    public Task<User> UpsertRefreshToken(Guid userId, RefreshToken refreshToken)
    {
        var filter = Builders<User>.Filter.Eq("_id", userId);
        var update = Builders<User>.Update
            .Set("RefreshTokens", refreshToken);

        var result = users
            .FindOneAndUpdateAsync(filter, update, new FindOneAndUpdateOptions<User>
            {
                IsUpsert = true,
            });

        return result;
    }

    public async Task Insert(User user)
    {
        await users.InsertOneAsync(user);
    }
}