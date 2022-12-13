using DAS.Trader.IntegrationClient.Client;
using DAS.Trader.IntegrationClient.Response;

namespace DAS.Trader.IntegrationClient.Commands.Interfaces;

public interface ITcpCommand
{
    TraderCommandType Type { get; }
    string Name { get; }
    string[] Params { get; }
    byte[] ToByteArray(string? command = null);
    string ToString();
    bool WaitForResult { get; }
    bool HasResult { get; }
    object Result { get; }
    void Subscribe(ResponseProcessor responseProcessor);
    void Unsubscribe(ResponseProcessor responseProcessor);
}