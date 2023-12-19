namespace Api.Mutations;

using Application.Features.Authentication;
using Application.Features.Authentication.Common;

[ExtendObjectType(typeof(Mutation))]
public class AuthMutation
{
	public Task<AuthResponse> Login(
		[Service] IMediator mediator,
		LoginCommand command) => mediator.Send(
		new LoginCommand(
			command.Username,
			command.Password));

	public Task<AuthResponse> Register(
		[Service] IMediator mediator,
		RegisterCommand command) => mediator.Send(command);
}