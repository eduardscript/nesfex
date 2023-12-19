namespace Shared.Extensions.DependantServices.DependencyInjection;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class DependencyInjectionDependantServicesExtensions
{
	public static IServiceCollection AddDependantServices(
		this IServiceCollection services,
		IConfiguration configuration)
	{
		services.Configure<DependantServicesConfiguration>(configuration.GetSection("DependantServices"));

		return services;
	}
}