namespace Api.Mutations.Types;

public class TechnologyMutationType : InputObjectType<UserTechnology>
{
	protected override void Configure(IInputObjectTypeDescriptor<UserTechnology> descriptor)
	{
		descriptor.Field(f => f.CreatedAt).Ignore();

		base.Configure(descriptor);
	}
}