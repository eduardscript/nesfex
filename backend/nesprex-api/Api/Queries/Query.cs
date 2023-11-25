using Domain.Entities;
using Domain.Repositories;
using Domain.Repositories.Filters;

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