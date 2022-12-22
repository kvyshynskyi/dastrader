namespace DAS.Trader.IntegrationClient.Interfaces;

public interface ICommandResult
{
    bool Success { get; }
    string? Message { get; }
}