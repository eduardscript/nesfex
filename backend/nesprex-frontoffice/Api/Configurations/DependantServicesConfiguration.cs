namespace Api.Configurations;

public class DependantServicesConfiguration
{
    public IEnumerable<DependentService> DependantServices { get; set; }
}

public class DependentService
{
    public string Name { get; set; }
    
    public string Endpoint { get; set; }
}
