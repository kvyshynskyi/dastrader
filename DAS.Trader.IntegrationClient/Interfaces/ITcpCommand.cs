using DAS.Trader.IntegrationClient.Commands;
using DAS.Trader.IntegrationClient.Enums;
using DAS.Trader.IntegrationClient.Response;

namespace DAS.Trader.IntegrationClient.Interfaces;

public interface ITcpCommand
{
    TraderCommandType Type { get; }
    string Name { get; }
    string[] Params { get; }
    byte[] ToByteArray(string? command = null);
    string ToString();
    bool WaitForResult { get; }
    bool HasResult { get; }
    object? Result { get; }
    void Subscribe(IResponseProcessor responseProcessor);
    void Unsubscribe(IResponseProcessor responseProcessor);
}