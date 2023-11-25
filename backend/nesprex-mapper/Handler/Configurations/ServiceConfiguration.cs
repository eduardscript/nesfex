namespace Handler.Configurations;

public class ServiceConfiguration
{
    public TimeSpan Interval { get; set; }
    
    public TimeSpan PingInterval { get; set; }
    
    public MapperConfiguration Mapper { get; set; } = default!;
}

public class MapperConfiguration
{
    public string ImageUrl { get; set; } = default!;
}