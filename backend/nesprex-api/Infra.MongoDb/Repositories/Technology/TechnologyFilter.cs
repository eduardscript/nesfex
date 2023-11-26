namespace Infra.MongoDb.Repositories.Technology;

public class TechnologyFilter
{
    public IEnumerable<string>? TechnologyNames { get; init; }
    
    public IEnumerable<string>? CategoryNames { get; init; }
    
    public IEnumerable<string>? CapsuleNames { get; init; }
}