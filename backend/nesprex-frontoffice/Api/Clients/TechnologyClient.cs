namespace Api.Clients;

using Utilities;

public interface ITechnologyClient
{
	Task<IGetTechnologies_Technologies> GetTechnologies(
		IReadOnlyList<string> technologyNames);
}

public class TechnologyClient(IBackofficeClient backofficeClient) : ITechnologyClient
{
	public async Task<IGetTechnologies_Technologies> GetTechnologies(
		IReadOnlyList<string> technologyNames)
	{
		var technologiesResult = await backofficeClient.GetTechnologies.ExecuteAsync(technologyNames);

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