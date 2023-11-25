namespace Shared.Extensions.Configurations;

public interface IMongoConfiguration
{
    public string DatabaseName { get; set; }

    public string ConnectionString { get; set; }
}