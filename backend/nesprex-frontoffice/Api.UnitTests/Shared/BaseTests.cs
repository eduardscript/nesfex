using AutoFixture;

namespace Api.UnitTests.Shared;

public abstract class BaseTests
{
    protected readonly Fixture fixture = new();
    
    protected IGetTechnologies_Technologies GenerateTechnologies()
    {
        var userTechnologies = Substitute.For<IGetTechnologies_Technologies>();
        userTechnologies.Name.Returns(fixture.Create<string>());

        var userCategoryTechnologies = Substitute.For<IGetTechnologies_Technologies_Categories>();
        userCategoryTechnologies.Name.Returns(fixture.Create<string>());

        userTechnologies.Categories.Returns(new List<IGetTechnologies_Technologies_Categories>()
            { userCategoryTechnologies });

        var capsule = Substitute.For<IGetTechnologies_Technologies_Categories_Capsules>();
        capsule.Name.Returns(fixture.Create<string>());

        userCategoryTechnologies.Capsules.Returns(new List<IGetTechnologies_Technologies_Categories_Capsules>()
            { capsule });

        return userTechnologies;
    }
}