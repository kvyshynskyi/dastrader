using DAS.Trader.IntegrationClient.Commands;

namespace DAS.Trader.IntegrationClient.Response;

public class ResponseEventArgs:EventArgs
{
    public ResponseEventArgs(Guid correlationId, TraderCommandType commandType, string? message = null,
        params string[]? parameters)
    {
        Message = message;
        CommandType = commandType;
        CorrelationId = correlationId;
        Parameters = parameters;
    }

    public string? Message { get; }
    public TraderCommandType CommandType { get; }
    public Guid CorrelationId { get; }
    public string[]? Parameters { get;}
    
}