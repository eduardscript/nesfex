namespace Application.Configurations;

public class JwtOptions
{
	public string ValidIssuer { get; set; } = default!;

	public IEnumerable<string> ValidAudiences { get; set; } = default!;

	public string SecretKey { get; set; } = default!;
}