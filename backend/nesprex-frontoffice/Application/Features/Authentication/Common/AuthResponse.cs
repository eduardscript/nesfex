namespace Application.Features.Authentication.Common;

using Utils;

public class AuthResponse
{
	private AuthResponse(
		string token,
		string refreshToken)
	{
		Token = token;
		RefreshToken = refreshToken;
	}

	public string Token { get; private set; }
	public string RefreshToken { get; private set; }

	internal static async Task<AuthResponse> CreateAsync(
		TokenUtils tokenUtils,
		Guid userId)
	{
		var refreshToken = (await tokenUtils.RefreshTokenUtils.GenerateRefreshToken(userId)).Value;
		var token = tokenUtils.JwtUtils.GenerateToken(userId);

		return new AuthResponse(
			token,
			refreshToken);
	}
}