using System;
using System.Net.Http;
using Api.Configurations;
using Microsoft.Extensions.Options;

namespace Api.Extensions;

public static class ServicesExtensions
{
    public static void CheckServiceHealth(this IServiceCollection services)
    {
        var sp = services.BuildServiceProvider();
        
        var servicesToCheck = sp
            .GetRequiredService<IOptions<DependantServicesConfiguration>>()
            .Value
            .DependantServices;

        var client = sp.GetRequiredService<IHttpClientFactory>().CreateClient();

        var exceptions = new List<Exception>();

        foreach (var serviceToCheck in servicesToCheck)
        {
            client.BaseAddress = new Uri(serviceToCheck.Endpoint);

            try
            {
                var response = client.GetAsync(serviceToCheck.Endpoint + "/ping").Result;

                if (!response.IsSuccessStatusCode)
                {
                    exceptions.Add(new Exception($"The /ping endpoint of the {serviceToCheck.Name} service is not reachable."));
                }
            }
            catch (Exception ex)
            {
                exceptions.Add(ex);
            }
        }

        if (exceptions.Any())
        {
            throw new AggregateException(exceptions);
        }
    }
}
