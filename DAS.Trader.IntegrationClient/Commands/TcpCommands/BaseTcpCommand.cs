using System.Text;
using DAS.Trader.IntegrationClient.Client;
using DAS.Trader.IntegrationClient.Common;
using DAS.Trader.IntegrationClient.Enums;
using DAS.Trader.IntegrationClient.Interfaces;
using DAS.Trader.IntegrationClient.Response;

namespace DAS.Trader.IntegrationClient.Commands.TcpCommands;

public abstract class BaseTcpCommand : ITcpCommand
{
    protected BaseTcpCommand(TraderCommandType type, bool waitForResult = false, bool hasResult = false, params string[] @params)
    {
        Type = type;
        WaitForResult = waitForResult;
        HasResult = hasResult;
        Name = type.GetDescription() ?? string.Empty;
        Params = @params;
    }

    public TraderCommandType Type { get; }
    public string Name { get; }
    public string[] Params { get; }

    public byte[] ToByteArray(string? command)
    {
        var buffer = Encoding.ASCII.GetBytes(command ?? ToString());

        return buffer;
    }

    public override string ToString()
    {
        var sb = new StringBuilder();

        sb.Append(Name);

        foreach (var param in Params)
        {
            if (string.IsNullOrWhiteSpace(param))
                continue;

            sb.Append($" {param}");
        }

        sb.Append("\r\n");

        return sb.ToString();
    }

    public bool WaitForResult { get;}
    public bool HasResult { get; protected set; }
    public virtual object? Result { get; protected set; }

    public virtual void Subscribe(ResponseProcessor responseProcessor)
    {
    }

    public virtual void Unsubscribe(ResponseProcessor responseProcessor)
    {
    }
}