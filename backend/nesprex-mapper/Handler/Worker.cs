using Domain.Entities;
using Handler.Configurations;
using Handler.Services;
using Microsoft.Extensions.Options;

namespace Handler;

public class Worker : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IKafkaService _kafkaService;
    private readonly ServiceConfiguration _serviceConfiguration;
    private readonly ILogger<Worker> _logger;

    public Worker(
        IServiceProvider serviceProvider,
        IKafkaService kafkaService,
        IOptions<ServiceConfiguration> serviceConfiguration,
        ILogger<Worker> logger)
    {
        _serviceProvider = serviceProvider;
        _kafkaService = kafkaService;
        _serviceConfiguration = serviceConfiguration.Value;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogTrace("Nesprex Mapper started at: {time}", DateTimeOffset.Now);

        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _serviceProvider.CreateScope();
            var dataProvider = scope.ServiceProvider.GetRequiredService<IDataProvider>();

            var imagesData = await dataProvider.GetImagesData();

            var technologies = await dataProvider.GetTechnologies();

            var imagesDataMessageBags = imagesData
                .Select(imageData => new MessageBag<ImageData>(
                    "images",
                    imageData.FileName,
                    imageData));

            var technologiesMessageBags = technologies
                .Select(technology => new MessageBag<Technology>(
                    "technologies",
                    technology.Name,
                    technology));

            var tasks = new List<Task>
            {
                _kafkaService.Produce(imagesDataMessageBags),
                _kafkaService.Produce(technologiesMessageBags)
            };

            await Task.WhenAll(tasks);
            
            var waitingTime = DateTime.Now + _serviceConfiguration.Interval;
            while (DateTime.Now < waitingTime && !stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(_serviceConfiguration.PingInterval, stoppingToken);
            }
        }
    }
}