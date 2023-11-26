using System.Text;
using System.Text.Json;
using Confluent.Kafka;

namespace Updater.Services;

public interface IKafkaService
{
    public Task<TEntity> Consume<TEntity>() where TEntity : class;
}

public class KafkaService : IKafkaService
{
    public async Task<TEntity> Consume<TEntity>() where TEntity : class
    {
        var consumerConfig = new ConsumerConfig
        {
            BootstrapServers = "localhost:9092",
            GroupId = "nesprex-updater",
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnablePartitionEof = true,
        };

        using var consumer = new ConsumerBuilder<string, string>(consumerConfig).Build();

        consumer.Subscribe("nesprex.products");

        var consumeResult = consumer.Consume();
        var message = consumeResult.Message.Value;

        var technology = await JsonSerializer.DeserializeAsync<TEntity>(
            new MemoryStream(Encoding.UTF8.GetBytes(message)));

        consumer.Close();

        return technology!;
    }
}