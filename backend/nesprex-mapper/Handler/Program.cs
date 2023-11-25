using Confluent.Kafka;
using Handler;
using Handler.Configurations;
using Handler.Services;
using Microsoft.Extensions.Options;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((builder, services) =>
    {
        services
            .AddOptions<KafkaConfiguration>()
            .Bind(builder.Configuration.GetSection("Kafka"));

        services
            .AddOptions<ServiceConfiguration>()
            .Bind(builder.Configuration.GetSection("Service"));
        
        services
            .AddOptions<DataConfiguration>()
            .Bind(builder.Configuration.GetSection("Data"));

        services.AddSingleton<IAdminClient>(serviceProvider =>
        {
            var kafkaConfiguration = serviceProvider.GetRequiredService<IOptions<KafkaConfiguration>>().Value;

            return new AdminClientBuilder(new AdminClientConfig
            {
                BootstrapServers = kafkaConfiguration.BootstrapServer
            }).Build();
        });

        services.AddSingleton<IProducer<string, string>>(serviceProvider =>
        {
            var kafkaConfiguration = serviceProvider.GetRequiredService<IOptions<KafkaConfiguration>>().Value;

            var producerConfig = new ProducerConfig
            {
                BootstrapServers = kafkaConfiguration.BootstrapServer,
                ClientId = kafkaConfiguration.Producer.ClientId,
                Acks = Acks.All,
            };

            return new ProducerBuilder<string, string>(producerConfig).Build();
        });
        
        services.AddSingleton<IDataProvider, FileDataProvider>();
        services.AddSingleton<IKafkaService, KafkaService>();

        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();