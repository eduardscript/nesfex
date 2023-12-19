namespace Shared.Extensions.DependantServices;

public class DependantService
{
	public string Name { get; set; }

	public string Endpoint { get; set; }
}

public class DependantServicesConfiguration
{
	public List<DependantService> DependantServices { get; set; } = new();
}