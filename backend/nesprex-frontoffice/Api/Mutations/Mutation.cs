using Api.Entities;
using Api.Repositories;

namespace Api.Mutations;

public class Mutation(IUserTechnologiesRepository userTechnologiesRepository)
{
    public async Task<AddedUserTechnology> AddTechnologyWithCapsules(Technology technology)
    {
        await userTechnologiesRepository.AddUserTechnology(technology);
        
        await Task.CompletedTask;
        
        return new AddedUserTechnology(technology);
    }

    public async Task<UpdatedExecuted> AddCapsuleQuantity(string capsuleName, int quantity)
    {
        await Task.CompletedTask;
        
        return new UpdatedExecuted(capsuleName, quantity);
    }
    
    public async Task<UpdatedExecuted> RemoveCapsuleQuantity(string capsuleName, int quantity)
    {
        await Task.CompletedTask;
        
        return new UpdatedExecuted(capsuleName, quantity);
    }

    public class MutationType : InputObjectType<Technology>
    {
        protected override void Configure(IInputObjectTypeDescriptor<Technology> descriptor)
        {
            descriptor.Field(f => f.CreatedAt).Ignore();

            base.Configure(descriptor);
        }
    }
}

public record AddedUserTechnology(
    Technology Technology);
    
public record UpdatedExecuted(
    string Name,
    int Quantity);