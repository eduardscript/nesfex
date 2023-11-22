using System.Text.Json;
using Confluent.Kafka;
using Domain.Utilities.Extensions;
using Handler.Configurations;
using Microsoft.Extensions.Options;

namespace Handler.Services;

public record MessageBag<TEntity>(
    string MessageType,
    string Key,
    TEntity Value);

public interface IKafkaService
{
    public Task Produce<TMessage>(IEnumerable<MessageBag<TMessage>> messageBags);

    public Task DeleteTopics();
}

public class KafkaService : IKafkaService
{
    private readonly KafkaConfiguration _kafkaConfiguration;
    private readonly IAdminClient _adminClient;
    private readonly IProducer<string, string> _producer;
    private readonly ILogger<KafkaService> _logger;

    public KafkaService(
        IOptions<KafkaConfiguration> kafkaConfiguration,
        IAdminClient adminClient,
        IProducer<string, string> producer,
        ILogger<KafkaService> logger)
    {
        _kafkaConfiguration = kafkaConfiguration.Value;
        _adminClient = adminClient;
        _producer = producer;
        _logger = logger;
    }

    public async Task Produce<TMessage>(IEnumerable<MessageBag<TMessage>> messageBags)
    {
        var iteratedMessageBags = messageBags as MessageBag<TMessage>[] ?? messageBags.ToArray();

        var messages = iteratedMessageBags.Select(m => new Message<string, string>
        {
            Key = m.Key.ToKey(),
            Value = JsonSerializer.Serialize(m.Value),
        }).ToList();

        var topics = _kafkaConfiguration.Producer
            .Topics
            .ToDictionary(config => config.Name, config => config.Topic);

        var messageType = iteratedMessageBags.First().MessageType;

        var tasks = messages.Select(async message =>
        {
            var deliveryResult = await _producer.ProduceAsync(topics[messageType], message);
            _logger.LogInformation("[{type} | {key}] Produced - Partition '{partition}' Offset '{offset}'",
                messageType,
                message.Key,
                deliveryResult.TopicPartitionOffset.TopicPartition.Partition.Value,
                deliveryResult.TopicPartitionOffset.Offset.Value);
        });

        await Task.WhenAll(tasks);
    }

    public async Task DeleteTopics()
    {
        var topicsToDelete = _kafkaConfiguration.Producer.Topics.Where(t => t.EmptyProduce)
            .Select(t => t.Topic)
            .ToArray();

        var metadataTopics = _adminClient
            .GetMetadata(TimeSpan.FromSeconds(15))
            .Topics
            .IntersectBy(topicsToDelete, t => t.Topic)
            .ToArray();

        if (metadataTopics.Length != topicsToDelete.Length)
        {
            var notFoundTopics = topicsToDelete.Except(metadataTopics.Select(t => t.Topic));

            throw new Exception($"Some topics were not found: {string.Join(", ", notFoundTopics)}");
        }

        await _adminClient.DeleteTopicsAsync(topicsToDelete);
    }
}