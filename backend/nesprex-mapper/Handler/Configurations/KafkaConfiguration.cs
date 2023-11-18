namespace Handler.Configurations;

public sealed class KafkaConfiguration
{
    public string BootstrapServer { get; set; } = default!;
    public KafkaProducerConfiguration Producer { get; set; } = default!;
    public string ImageUrl { get; set; } = default!;
}

public sealed class KafkaProducerConfiguration
{
    public string ClientId { get; set; } = default!;

    public bool DeleteTopicsOnStartup { get; set; }

    public List<KafkaTopicConfiguration> Topics { get; set; } = default!;
}

public sealed class KafkaTopicConfiguration
{
    public string Name { get; set; } = default!;
    public string Topic { get; set; } = default!;
    public bool EmptyProduce { get; set; }
}
