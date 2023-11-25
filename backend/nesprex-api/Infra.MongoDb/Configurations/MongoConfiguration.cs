namespace Infra.MongoDb.Configurations;

public class MongoConfiguration
{
    public string ConnectionString { get; set; } = default!;
    
    public string DatabaseName { get; set; } = default!;
}