using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Shared.Extensions.Mongo.DependencyInjection;

namespace Shared.Extensions.DependantServices.DependencyInjection;

public static class DependencyInjectionDependantServicesExtensions
{
    public static IServiceCollection AddDependantServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<DependantServicesConfiguration>(configuration.GetSection("DependantServices"));

        return services;
    }
}