namespace Infra.MongoDb.Repositories.Technology;

public class TechnologyFilter
{
    public IList<string>? TechnologyNames { get; init; }
    
    public IList<string>? CategoryNames { get; init; }
    
    public IList<string>? CapsuleNames { get; init; }
}