namespace Api.Utilities;

public static class Errors
{
	public static IError BuildMachineNotFound(
		string machineName)
	{
		return BuildMachineNotFound(new[] { machineName });
	}

	public static IError BuildMachineNotFound(
		IEnumerable<string> machineName) =>
		ErrorBuilder.New()
					.SetMessage($"{string.Join(",", machineName)} not found in the machine.")
					.SetCode("MACHINE_NOT_FOUND")
					.Build();

	public static IError BuildCapsulesNotFound(
		IEnumerable<string> entityNames) =>
		ErrorBuilder.New()
					.SetMessage($"{string.Join(",", entityNames)} not found.")
					.SetCode("CAPSULES_NOT_FOUND")
					.Build();

	public static IError BuildCapsulesNotFound(
		IEnumerable<string> entityNames,
		string machineName) =>
		ErrorBuilder.New()
					.SetMessage($"{string.Join(",", entityNames)} not found in the machine {machineName}.")
					.SetCode("CAPSULES_NOT_FOUND")
					.Build();

	public static IError BuildServiceIsDown(
		string serviceName) =>
		ErrorBuilder.New()
					.SetMessage($"{serviceName} is down. Please try again later.")
					.SetCode("SERVICE_IS_DOWN")
					.Build();
}