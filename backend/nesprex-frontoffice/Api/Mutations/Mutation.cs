using Api.Mutations.Requests;
using Api.Mutations.Results;
using Api.Utilities;
using Infra.MongoDb.Repositories.UsersTechnology;
using Microsoft.AspNetCore.Mvc;

namespace Api.Mutations;

public class Mutation
{
    public async Task<AddedTechnology> AddTechnologyWithCapsules(
        [FromServices] IBackofficeClient backofficeClient,
        [FromServices] IUserTechnologiesRepository userTechnologiesRepository,
        UserTechnology technology)
    {
        var technologiesResult = await backofficeClient
            .GetTechnologies
            .ExecuteAsync(new[] { technology.Name });

        if (technologiesResult.Data is null)
        {
            throw new GraphQLException(Errors.BuildServiceIsDown(Constants.Services.Backoffice));
        }
        
        var technologyResult = technologiesResult.Data.Technologies.FirstOrDefault();
        if (technologyResult is null)
        {
            throw new GraphQLException(Errors.BuildMachineNotFound(technology.Name));
        }
        
        var existingCapsules = technologyResult.Categories
            .SelectMany(tc => tc.Capsules)
            .Select(c => c.Name)
            .ToHashSet();
        
        var capsulesNotFound = technology.Capsules
            .Where(c => !existingCapsules.Contains(c.Name))
            .Select(c => c.Name)
            .ToList();

        if (capsulesNotFound.Count != 0)
        {
            throw new GraphQLException(Errors.BuildCapsulesNotFound(capsulesNotFound, technology.Name));
        }
        
        return new AddedTechnology(technology);
    }

    public async Task<UpdatedExecuted> UpdateCapsuleQuantity(
        string capsuleName,
        QuantityOperation quantityOperation)
    {
        return new UpdatedExecuted(capsuleName, quantityOperation.Quantity);
    }
}

