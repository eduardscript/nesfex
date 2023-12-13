using Api.Entities;

namespace Api.Mutations.Types;

public class TechnologyMutationType : InputObjectType<Technology>
{
    protected override void Configure(IInputObjectTypeDescriptor<Technology> descriptor)
    {
        descriptor.Field(f => f.CreatedAt).Ignore();

        base.Configure(descriptor);
    }
}