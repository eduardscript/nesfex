using Domain.Entities;
using Domain.Repositories.Filters;

namespace Domain.Repositories;

public interface ITechnologyRepository
{
    Task<IEnumerable<Technology>> GetTechnologiesAsync(TechnologyFilter filter);
}