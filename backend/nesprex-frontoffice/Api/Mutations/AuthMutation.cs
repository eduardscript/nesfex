using Api.Services;

namespace Api.Mutations;

public record LoginInput(
    string Username,
    string Password);

public record AuthResult(
    string Token,
    string RefreshToken);

[ExtendObjectType(typeof(Mutation))]
public class AuthMutation
{
    public async Task<AuthResult> Login(
        [Service] IAuthService authService,
        LoginInput input)
    {
        var user = await authService.Login(input.Username, input.Password);
        if (user == null)
        {
            throw new GraphQLException("Invalid username or password.");
        }
        
        var token = await authService.GenerateJwtToken(user);

        return new AuthResult(token, user.RefreshTokens.Last().Value);
    }

    public async Task<AuthResult> Register(
        [Service] IAuthService authService,
        LoginInput input)
    {
        var user = await authService.Register(input.Username, input.Password);
        if (user == null)
        {
            throw new GraphQLException("Invalid username or password.");
        }
        
        var token = await authService.GenerateJwtToken(user);

        return new AuthResult(token, user.RefreshTokens.Last().Value);
    }
}