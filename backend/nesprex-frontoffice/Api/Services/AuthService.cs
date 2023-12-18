using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Infra.MongoDb.Repositories.Users;
using Microsoft.IdentityModel.Tokens;
using User = Shared.Domain.Entities.User;

namespace Api.Services;

public interface IAuthService
{
    Task<User?> Login(string username, string password);
    
    Task<User> Register(string username, string password);

    Task<string> GenerateJwtToken(User user);

    Task<string> GenerateRefreshToken();
}

public class AuthService(IUsersRepository usersRepository) : IAuthService
{
    public async Task<User> Register(string username, string password)
    {
        var existingUser = await usersRepository.GetByUsername(username);
        if (existingUser != null)
        {
            throw new Exception("Username already exists.");
        }

        var user = new User(username, password)
        {
            RefreshTokens = new List<RefreshToken>
            {
                new(await GenerateRefreshToken())
            }
        };

        await usersRepository.Insert(user);

        return user;
    }

    public async Task<User?> Login(string username, string password)
    {
        var existingUser = await usersRepository.GetByUsername(username);

        var refreshToken = await GenerateRefreshToken();
        
        await usersRepository.UpsertRefreshToken(existingUser.Id, new RefreshToken(refreshToken));

        return existingUser;
    }

    public Task<string> GenerateJwtToken(User user)
    {
        var key = new SymmetricSecurityKey(
            "ff842af0677a2c1e84cf8954d4d67dc73791e80c4f10e4460fc4dd71de70cbcc"u8.ToArray());

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Username),
            new Claim(ClaimTypes.Role, "Guest")
        };
        
        if (user.Username == "admin")
        {
            claims = claims.Append(new Claim(ClaimTypes.Role, "Admin")).ToArray();
        }

        var token = new JwtSecurityToken(
            "Api",
            "Api",
            claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: creds);

        return Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
    }

    public Task<string> GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Task.FromResult(Convert.ToBase64String(randomNumber));
    }
}