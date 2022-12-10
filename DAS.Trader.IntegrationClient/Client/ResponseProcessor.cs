using System.Collections.Concurrent;
using System.ComponentModel;
using System.Net.Sockets;
using System.Text;
using DAS.Trader.IntegrationClient.Commands;
using DAS.Trader.IntegrationClient.Common;

namespace DAS.Trader.IntegrationClient.Client;

public class ResponseProcessor
{
    private readonly ConcurrentDictionary<TraderCommandType, object> _eventKeys = new();
    protected EventHandlerList ListEventDelegates = new();

    public ResponseProcessor(CancellationToken cancellationToken)
    {
        CancellationToken = cancellationToken;
    }

    public CancellationToken CancellationToken { get; }

    public async Task ListenAsync(NetworkStream networkStream)
    {
        while (!CancellationToken.IsCancellationRequested && networkStream.CanRead)
            await ReadStreamAsync(networkStream);
    }

    public event EventHandler<ResponseEventHandlerArgs> LoginResponse
    {
        add => ListEventDelegates.AddHandler(GetEventKey(TraderCommandType.LOGIN_RESPONSE), value);
        remove => ListEventDelegates.RemoveHandler(GetEventKey(TraderCommandType.LOGIN_RESPONSE), value);
    }

    public event EventHandler<ResponseEventHandlerArgs> PriceInquiry
    {
        add => ListEventDelegates.AddHandler(GetEventKey(TraderCommandType.SLRET_RESPONSE), value);
        remove => ListEventDelegates.RemoveHandler(GetEventKey(TraderCommandType.SLRET_RESPONSE), value);
    }

    protected virtual void RiseEvent(ResponseEventHandlerArgs e)
    {
        var eventHandler = ListEventDelegates[GetEventKey(e.CommandType)] as EventHandler<ResponseEventHandlerArgs>;
        eventHandler?.Invoke(this, e);
    }

    private object GetEventKey(TraderCommandType commandType)
    {
        return _eventKeys.GetOrAdd(commandType, key => new object());
    }

    private async Task ReadStreamAsync(NetworkStream networkStream)
    {
        if (!networkStream.DataAvailable) return;

        var sb = new StringBuilder();

        const int chunkSize = 10_240; //10KB
        var buffer = new byte[chunkSize];
        int bytesRead;

        while ((bytesRead = await networkStream.ReadAsync(buffer)) > 0)
        {
            sb.Append(Encoding.ASCII.GetString(buffer, 0, bytesRead));

            if (bytesRead < chunkSize) break;
        }

        foreach (var line in sb.ToString().Split("\r\n", StringSplitOptions.RemoveEmptyEntries))
        {
            ProceedLine(line);
            Console.WriteLine($"|<--|    {line}");
        }
    }

    private void ProceedLine(string line)
    {
        if (line.StartsWith(TraderCommandType.LOGIN_RESPONSE.GetDescription() ?? string.Empty,
                StringComparison.InvariantCultureIgnoreCase))
        {
            var args = new ResponseEventHandlerArgs
            {
                CommandType = TraderCommandType.LOGIN_RESPONSE,
                Message = line
            };
            RiseEvent(args);
            Console.WriteLine($"|<<<|    Event detected: {TraderCommandType.LOGIN_RESPONSE}");
        }
    }
}