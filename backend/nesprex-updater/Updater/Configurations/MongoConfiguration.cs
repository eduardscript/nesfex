using Shared.Extensions.Configurations;

namespace Updater.Configurations;

public class MongoConfiguration : IMongoConfiguration
{
    public string DatabaseName { get; set; } = default!;

    public string ConnectionString { get; set; } = default!;
}