using DAS.Trader.IntegrationClient.Commands;

namespace DAS.Trader.IntegrationClient.Client;

public class ResponseEventHandlerArgs
{
    public string? Message { get; set; }
    public TraderCommandType CommandType { get; set; }
    public string[]? Parameters { get; set; }
}