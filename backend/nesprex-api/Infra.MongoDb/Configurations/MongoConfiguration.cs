using Shared.Extensions.Configurations;

namespace Infra.MongoDb.Configurations;

public class MongoConfiguration : IMongoConfiguration
{
    public string ConnectionString { get; set; } = default!;
    
    public string DatabaseName { get; set; } = default!;
}