namespace Api.Utilities;

public static class Errors
{
    public static IError BuildMachineNotFound(string machineName)
    {
        return ErrorBuilder.New()
            .SetMessage($"{machineName} not found in the machine.")
            .SetCode("MACHINE_NOT_FOUND")
            .Build();
    }

    public static IError BuildCapsulesNotFound(IEnumerable<string> entityNames, string machineName)
    {
        return ErrorBuilder.New()
            .SetMessage($"{string.Join(",", entityNames)} not found in the machine {machineName}.")
            .SetCode("CAPSULES_NOT_FOUND")
            .Build();
    }
    
    public static IError BuildServiceIsDown(string serviceName)
    {
        return ErrorBuilder.New()
            .SetMessage($"{serviceName} is down. Please try again later.")
            .SetCode("SERVICE_IS_DOWN")
            .Build();
    }
}