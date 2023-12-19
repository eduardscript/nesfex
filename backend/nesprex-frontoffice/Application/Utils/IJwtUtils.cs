namespace Application.Utils;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Configurations;
using Infra.MongoDb.Repositories.Users;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shared.Domain.Entities;

public interface IJwtUtils
{
	string GenerateToken(
		Guid userId);

	Guid GetUserIdFromToken(
		string token);
}

public class JwtUtils : IJwtUtils
{
	private readonly JwtOptions _jwtOptions;

	public JwtUtils(
		IOptions<JwtOptions> jwtOptions) => _jwtOptions = jwtOptions.Value;

	private SymmetricSecurityKey SecurityKey => new(Encoding.ASCII.GetBytes(_jwtOptions.SecretKey));

	public string GenerateToken(
		Guid userId)
	{
		var tokenDescriptor = new SecurityTokenDescriptor
		{
			Issuer = _jwtOptions.ValidIssuer,
			Audience = _jwtOptions.ValidAudiences.First(),
			Subject = new ClaimsIdentity(
				new Claim[]
				{
					new(
						ClaimTypes.NameIdentifier,
						userId.ToString())
				}),
			Expires = DateTime.UtcNow.AddMinutes(5),
			SigningCredentials = new SigningCredentials(
				SecurityKey,
				SecurityAlgorithms.HmacSha256Signature)
		};

		var tokenHandler = new JwtSecurityTokenHandler();

		var token = tokenHandler.CreateToken(tokenDescriptor);
		var tokenString = tokenHandler.WriteToken(token);

		return tokenString;
	}

	public Guid GetUserIdFromToken(
		string token)
	{
		var tokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuerSigningKey = true,
			IssuerSigningKey = SecurityKey,
			ValidateIssuer = false,
			ValidateAudience = true,
			ValidateLifetime = true,
			ValidAudiences = _jwtOptions.ValidAudiences,
			ValidIssuer = _jwtOptions.ValidIssuer
		};

		var tokenHandler = new JwtSecurityTokenHandler();
		tokenHandler.ValidateToken(
			token,
			tokenValidationParameters,
			out var validatedToken);

		var jwtToken = (JwtSecurityToken)validatedToken;
		var userId = Guid.Parse(
			jwtToken.Claims.First(x => x.Type == ClaimTypes.NameIdentifier)
					.Value);

		return userId;
	}
}

public interface IRefreshTokenUtils
{
	Task<RefreshToken> GenerateRefreshToken(
		Guid userId);

	Task RevokeRefreshToken(
		Guid userId);
}

public class RefreshTokenUtils(IUsersRepository usersRepository) : IRefreshTokenUtils
{
	public Task<RefreshToken> GenerateRefreshToken(
		Guid userId)
	{
		var refreshToken = new RefreshToken(
			Guid
				.NewGuid()
				.ToString());

		return Task.FromResult(refreshToken);
	}

	public Task RevokeRefreshToken(
		Guid userId) => throw new NotImplementedException();
}

public class TokenUtils(IJwtUtils jwtUtils, IRefreshTokenUtils refreshTokenUtils)
{
	public IRefreshTokenUtils RefreshTokenUtils { get; } = refreshTokenUtils;
	public IJwtUtils JwtUtils { get; } = jwtUtils;
}