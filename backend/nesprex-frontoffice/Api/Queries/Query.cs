using Api.Entities;
using Api.Repositories;

namespace Api.Queries;

public class Query(IUserTechnologiesRepository userTechnologiesRepository)
{
    /// <summary>
    /// Get all users selected technologies with its capsules and quantities
    /// </summary>
    /// <param name="userId">The user id</param>
    /// <returns></returns>
    public Task<IEnumerable<Technology>> GetTechnologies(Guid userId)
    {
        return userTechnologiesRepository.GetUserTechnologies();
    }
}