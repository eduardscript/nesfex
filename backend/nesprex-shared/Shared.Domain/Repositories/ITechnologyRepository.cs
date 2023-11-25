using Shared.Domain.Entities;
using Shared.Domain.Repositories.Filters;

namespace Shared.Domain.Repositories;

public interface ITechnologyRepository
{
    Task<IEnumerable<Technology>> GetTechnologiesAsync(TechnologyFilter filter);
}