namespace Handler.Configurations;

public class DataConfiguration
{
    public IEnumerable<DataConfigurationPath> Paths { get; set; } = default!;
}

public class DataConfigurationPath
{
    public string Name { get; set; } = default!;

    public string Path { get; set; } = default!;
}