using Shared.Domain.Entities;
using Shared.Domain.Repositories;
using Shared.Domain.Repositories.Filters;

namespace Api.Queries;

public class Query
{
    public Task<IEnumerable<Technology>> GetTechnologiesAsync(
        [Service] ITechnologyRepository technologyRepository,
        TechnologyFilter filter)
    {
        return technologyRepository.GetTechnologiesAsync(filter);
    }
}