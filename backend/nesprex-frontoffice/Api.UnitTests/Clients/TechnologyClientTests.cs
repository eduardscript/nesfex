namespace Api.UnitTests.Clients;

using Api.Clients;
using StrawberryShake;

public class TechnologyClientTests : BaseTests
{
	[Fact]
	public async Task ShouldThrow_WhenUserTechnologyDoesNotExist()
	{
		// Arrange
		var backofficeClient = Substitute.For<IBackofficeClient>();

		var operationResult = Substitute.For<IOperationResult<IGetTechnologiesResult>>();

		operationResult.Data.Returns(new GetTechnologiesResult(new List<IGetTechnologies_Technologies>()));

		backofficeClient
			.GetTechnologies
			.ExecuteAsync(Arg.Any<IReadOnlyList<string>>())
			.Returns(operationResult);

		// Act
		var technologyClient = new TechnologyClient(backofficeClient);

		var userTechnologyNames = fixture.Create<IReadOnlyList<string>>();

		var exception
			= await Assert.ThrowsAsync<GraphQLException>(() => technologyClient.GetTechnologies(userTechnologyNames));

		// Assert
		var expectedMessage = Errors.BuildMachineNotFound(userTechnologyNames);

		Assert.NotNull(exception);

		var exceptionError = Assert.Single(exception.Errors);
		Assert.Equal(
			expectedMessage.Message,
			exceptionError.Message);

		Assert.Equal(
			expectedMessage.Code,
			exceptionError.Code);

		// await technologyClient
		//     .Received()
		//     .GetTechnologies(Arg.Is<IReadOnlyList<string>>(ut => ut.SequenceEqual(userTechnologyNames)));
	}

	[Fact]
	public async Task ShouldThrow_WhenBackofficeServiceIsDown()
	{
		// Arrange
		var backofficeClient = Substitute.For<IBackofficeClient>();

		var operationResult = Substitute.For<IOperationResult<IGetTechnologiesResult>>();

		operationResult.Data.Returns((IGetTechnologiesResult)null!);

		backofficeClient
			.GetTechnologies
			.ExecuteAsync(Arg.Any<IReadOnlyList<string>>())
			.Returns(operationResult);

		// Act
		var technologyClient = new TechnologyClient(backofficeClient);

		var userTechnologyNames = fixture.Create<IReadOnlyList<string>>();

		var exception
			= await Assert.ThrowsAsync<GraphQLException>(() => technologyClient.GetTechnologies(userTechnologyNames));

		// Assert
		var expectedMessage = Errors.BuildServiceIsDown(Constants.Services.Backoffice);

		Assert.NotNull(exception);

		var exceptionError = Assert.Single(exception.Errors);
		Assert.Equal(
			expectedMessage.Message,
			exceptionError.Message);

		Assert.Equal(
			expectedMessage.Code,
			exceptionError.Code);
	}
}