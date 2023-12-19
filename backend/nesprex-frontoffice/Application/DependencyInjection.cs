namespace Application;

using Configurations;
using Microsoft.Extensions.DependencyInjection;
using Utils;

public static class DependencyInjection
{
	public static IServiceCollection AddApplication(
		this IServiceCollection services)
	{
		services
			.AddOptions<JwtOptions>()
			.BindConfiguration("Bearer");

		services
			.AddSingleton<IJwtUtils, JwtUtils>()
			.AddSingleton<IRefreshTokenUtils, RefreshTokenUtils>()
			.AddSingleton<TokenUtils>();

		services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

		return services;
	}
}