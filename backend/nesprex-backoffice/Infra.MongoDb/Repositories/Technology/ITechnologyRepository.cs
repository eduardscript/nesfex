namespace Infra.MongoDb.Repositories.Technology;

using Shared.Domain.Entities;

public interface ITechnologyRepository
{
    Task<IEnumerable<Technology>> GetTechnologiesAsync(TechnologyFilter? filter);
}