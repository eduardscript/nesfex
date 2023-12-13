using Api.Mutations.Results;
using Api.Utilities;
using Infra.MongoDb.Repositories.UsersTechnology;

namespace Api.Mutations;

public interface ITechnologyClient
{
    Task<IGetTechnologies_Technologies> GetTechnologies(IReadOnlyList<string> technologyNames);
}

public class TechnologyClient(IBackofficeClient backofficeClient) : ITechnologyClient
{
    public async Task<IGetTechnologies_Technologies> GetTechnologies(IReadOnlyList<string> technologyNames)
    {
        var technologiesResult = await backofficeClient
            .GetTechnologies
            .ExecuteAsync(technologyNames);

        if (technologiesResult.Data is null)
        {
            throw new GraphQLException(Errors.BuildServiceIsDown(Constants.Services.Backoffice));
        }

        var technologyResult = technologiesResult.Data.Technologies.FirstOrDefault();
        if (technologyResult is null)
        {
            throw new GraphQLException(Errors.BuildMachineNotFound(technologyNames));
        }

        return technologyResult;
    }
}

public class Mutation
{
    public async Task<AddedTechnology> AddTechnologyWithCapsules(
        [FromServices] IUserTechnologiesRepository userTechnologiesRepository,
        [FromServices] ITechnologyClient technologyClient,
        UserTechnologies technology)
    {
        var technologyNames = technology.Technologies
            .Select(t => t.Name)
            .ToList();

        var technologyResult = await technologyClient
            .GetTechnologies(technologyNames);

        var existingCapsules = technologyResult.Categories
            .SelectMany(tc => tc.Capsules)
            .Select(c => c.Name)
            .ToHashSet();

        var capsulesNotFound = technology.Technologies
            .SelectMany(t => t.Capsules)
            .Where(c => !existingCapsules.Contains(c.Name))
            .Select(c => c.Name)
            .ToList();

        if (capsulesNotFound.Count != 0)
        {
            throw new GraphQLException(Errors.BuildCapsulesNotFound(capsulesNotFound));
        }

        await userTechnologiesRepository.AddUserTechnology(
            technology);

        return new AddedTechnology(technology);
    }

    public async Task<UpdatedExecuted> UpdateCapsuleQuantity(
        [FromServices] IUserTechnologiesRepository userTechnologiesRepository,
        Guid userId,
        string technologyName,
        string capsuleName,
        int quantity)
    {
        var userTechnology = await userTechnologiesRepository
            .GetUserTechnologies(userId, technologyName);

        var selectedCapsule = userTechnology
            .Technologies
            .SelectMany(c => c.Capsules)
            .SingleOrDefault(c => c.Name == capsuleName);

        if (selectedCapsule is null)
        {
            throw new GraphQLException(Errors.BuildCapsulesNotFound(new[] { capsuleName }));
        }

        selectedCapsule = selectedCapsule with { Quantity = selectedCapsule.Quantity + quantity };

        await userTechnologiesRepository.UpdateCapsuleQuantityByTechnologyName(
            userId,
            technologyName,
            selectedCapsule);

        return new UpdatedExecuted(capsuleName, selectedCapsule.Quantity);
    }
}