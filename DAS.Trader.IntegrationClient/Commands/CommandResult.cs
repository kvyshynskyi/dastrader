using DAS.Trader.IntegrationClient.Commands.Interfaces;

namespace DAS.Trader.IntegrationClient.Commands;

internal class CommandResult:ICommandResult
{
    public static ICommandResult SuccessResult => new CommandResult {Success = true};

    public bool Success { get; set; }
    public string Message { get; set; }
}