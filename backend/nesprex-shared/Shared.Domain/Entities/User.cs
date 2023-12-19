namespace Shared.Domain.Entities;

using Common;

public record User(string Username, string Password) : BaseEntity
{
	public IEnumerable<RefreshToken> RefreshTokens { get; set; } = ArraySegment<RefreshToken>.Empty;
}

public record RefreshToken(string Value);