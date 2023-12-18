using Api.Clients;

namespace Api.UnitTests.Mutations;

public class AddTechnologyWithCapsulesTests : BaseTests
{
    [Fact]
    public async Task AddTechnologyWithCapsules_ShouldThrow_WhenUserTechnologyCapsulesDoesNotExist()
    {
        // Arrange
        var technologyClient = Substitute.For<ITechnologyClient>();
        var backofficeTechnologies = GenerateTechnologies();

        technologyClient.GetTechnologies(Arg.Any<IReadOnlyList<string>>())
            .Returns(backofficeTechnologies);

        var userTechnologiesRepository = Substitute.For<IUserTechnologiesRepository>();

        // Act
        var mutation = new Mutation();
        var userTechnologies = fixture.Create<UserTechnologies>();

        var exception = await Assert.ThrowsAsync<GraphQLException>(() => mutation
            .AddTechnologyWithCapsules(
                userTechnologiesRepository,
                technologyClient,
                userTechnologies));

        // Assert
        var expectedMessage = Errors.BuildCapsulesNotFound(userTechnologies
            .Technologies
            .SelectMany(c => c.Capsules)
            .Select(c => c.Name)
            .ToHashSet());

        Assert.NotNull(exception);
        
        var exceptionError = Assert.Single(exception.Errors);
        Assert.Equal(expectedMessage.Message, exceptionError.Message);
        Assert.Equal(expectedMessage.Code, exceptionError.Code);

        await technologyClient
            .Received()
            .GetTechnologies(Arg.Is<IReadOnlyList<string>>(ut => ut.SequenceEqual(userTechnologies
                .Technologies
                .Select(t => t.Name)
                .ToList())));
        
        await userTechnologiesRepository
            .DidNotReceive()
            .AddUserTechnology(Arg.Any<UserTechnologies>());
    }
}