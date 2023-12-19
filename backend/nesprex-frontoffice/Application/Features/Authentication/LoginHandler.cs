namespace Application.Features.Authentication;

using Common;
using Infra.MongoDb.Repositories.Users;
using Shared.Domain.Entities;
using Utils;

public record LoginCommand(string Username, string Password) : IRequest<AuthResponse>;

public class LoginHandler(IUsersRepository usersRepository, TokenUtils tokenUtils)
	: IRequestHandler<LoginCommand, AuthResponse>
{
	public async Task<AuthResponse> Handle(
		LoginCommand request,
		CancellationToken cancellationToken)
	{
		var existingUser = await usersRepository.GetByUsername(request.Username);
		if (existingUser == null)
		{
			throw new Exception("Username does not exist.");
		}

		if (!existingUser.Password.Equals(request.Password))
		{
			throw new Exception("Invalid password.");
		}

		var newUser = await usersRepository.Insert(
			new User(
				request.Username,
				request.Password));

		return await AuthResponse.CreateAsync(
			tokenUtils,
			newUser.Id);
	}
}