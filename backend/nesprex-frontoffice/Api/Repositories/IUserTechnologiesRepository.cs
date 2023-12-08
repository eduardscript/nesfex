using System.Xml.Linq;
using Api.Entities;

namespace Api.Repositories;

public interface IUserTechnologiesRepository
{
    Task<IEnumerable<Technology>> GetUserTechnologies();
    
    Task AddUserTechnology(Technology technology);
    
    Task UpdateCapsuleQuantity(string capsuleName, int quantity);
}

public class TempUserTechnologiesRepository : IUserTechnologiesRepository
{
    private readonly List<Technology> _technologies = new();

    public Task<IEnumerable<Technology>> GetUserTechnologies()
    {
        return Task.FromResult(_technologies.AsEnumerable());
    }

    public Task AddUserTechnology(Technology technology)
    {
        _technologies.Add(technology);

        return Task.CompletedTask;
    }

    public Task UpdateCapsuleQuantity(string capsuleName, int quantity)
    {
        var technology = _technologies.FirstOrDefault(t => t.Capsules.Any(c => c.Name == capsuleName));

        if (technology is null)
        {
            throw new Exception($"Technology with capsule {capsuleName} not found");
        }

        var capsule = technology.Capsules.FirstOrDefault(c => c.Name == capsuleName);

        if (capsule is null)
        {
            throw new Exception($"Capsule {capsuleName} not found");
        }

        
        var capsulesList = technology.Capsules.ToList();
        capsulesList.Remove(capsule);
        capsulesList.Add(new SelectedCapsule(capsuleName, quantity));
        
        _technologies.Remove(technology);
        _technologies.Add(technology with { Capsules = capsulesList });

        return Task.CompletedTask;
    }
}