namespace Application.Features.Authentication;

using Common;
using Infra.MongoDb.Repositories.Users;
using Shared.Domain.Entities;
using Utils;

public record RegisterCommand(string Username, string Password) : IRequest<AuthResponse>;

public class RegisterHandler : IRequestHandler<RegisterCommand, AuthResponse>
{
	private readonly IUsersRepository _usersRepository;
	private readonly TokenUtils _tokenUtils;

	public RegisterHandler(
		IUsersRepository usersRepository,
		TokenUtils tokenUtils)
	{
		_usersRepository = usersRepository;
		_tokenUtils = tokenUtils;
	}

	public async Task<AuthResponse> Handle(
		RegisterCommand request,
		CancellationToken cancellationToken)
	{
		var existingUser = await _usersRepository.GetByUsername(request.Username);
		if (existingUser is not null)
		{
			throw new Exception("Username already exists.");
		}

		var user = new User(
			request.Username,
			request.Password);

		await _usersRepository.Insert(user);

		return await AuthResponse.CreateAsync(
			_tokenUtils,
			user.Id);
	}
}