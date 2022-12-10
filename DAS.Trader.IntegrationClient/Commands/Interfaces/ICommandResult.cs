namespace DAS.Trader.IntegrationClient.Commands.Interfaces;

public interface ICommandResult
{
    bool Success { get; }
    string Message { get; }
}