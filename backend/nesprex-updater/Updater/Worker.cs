using Shared.Domain.Entities;
using Updater.Repositories.Technology;
using Updater.Services;

namespace Updater;

public class Worker(
    IKafkaService kafkaService,
    ITechnologyRepository technologyRepository,
    ILogger<Worker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (logger.IsEnabled(LogLevel.Information))
            {
                logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            }

            var techonology = await kafkaService.Consume<Technology>();

            await technologyRepository.InsertAsync(techonology);

            await Task.Delay(1000, stoppingToken);
        }
    }
}