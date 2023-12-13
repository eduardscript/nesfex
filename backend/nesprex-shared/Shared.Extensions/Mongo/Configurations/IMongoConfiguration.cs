namespace Shared.Extensions.Mongo.Configurations;

public interface IMongoConfiguration
{
    public string DatabaseName { get; set; }

    public string ConnectionString { get; set; }
}