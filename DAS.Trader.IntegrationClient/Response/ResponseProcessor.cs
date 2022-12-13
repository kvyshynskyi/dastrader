using System.Collections.Concurrent;
using System.ComponentModel;
using System.Net.Sockets;
using System.Text;
using DAS.Trader.IntegrationClient.Commands;

namespace DAS.Trader.IntegrationClient.Response;

public partial class ResponseProcessor
{
    private readonly ConcurrentDictionary<TraderCommandType, object> _eventKeys = new();
    protected EventHandlerList ListEventDelegates = new();

    public ResponseProcessor(CancellationToken cancellationToken)
    {
        CancellationToken = cancellationToken;
        WrapResponse += ResponseWrapper.WrapResponse;
    }

    public CancellationToken CancellationToken { get; }

    private event EventHandler<WrapResponseEventArgs> WrapResponse;

    public async Task ListenAsync(NetworkStream networkStream)
    {
        while (!CancellationToken.IsCancellationRequested && networkStream.CanRead)
            await ReadStreamAsync(networkStream);
    }

    public event EventHandler<ResponseEventArgs> LoginResponse
    {
        add => ListEventDelegates.AddHandler(GetEventKey(TraderCommandType.LOGIN_RESPONSE), value);
        remove => ListEventDelegates.RemoveHandler(GetEventKey(TraderCommandType.LOGIN_RESPONSE), value);
    }

    public event EventHandler<ResponseEventArgs> PriceInquiry
    {
        add => ListEventDelegates.AddHandler(GetEventKey(TraderCommandType.SLRET_RESPONSE), value);
        remove => ListEventDelegates.RemoveHandler(GetEventKey(TraderCommandType.SLRET_RESPONSE), value);
    }

    public event EventHandler<ResponseEventArgs> SlOrderBegin
    {
        add => ListEventDelegates.AddHandler(GetEventKey(TraderCommandType.SLORDER_BEGIN_RESPONSE), value);
        remove => ListEventDelegates.RemoveHandler(GetEventKey(TraderCommandType.SLORDER_BEGIN_RESPONSE), value);
    }

    public event EventHandler<ResponseEventArgs> SlOrderEnd
    {
        add => ListEventDelegates.AddHandler(GetEventKey(TraderCommandType.SLORDER_END_RESPONSE), value);
        remove => ListEventDelegates.RemoveHandler(GetEventKey(TraderCommandType.SLORDER_END_RESPONSE), value);
    }

    public event EventHandler<ResponseEventArgs> SlOrder
    {
        add => ListEventDelegates.AddHandler(GetEventKey(TraderCommandType.SLORDER_RESPONSE), value);
        remove => ListEventDelegates.RemoveHandler(GetEventKey(TraderCommandType.SLORDER_RESPONSE), value);
    }

    protected virtual void RiseEvent(ResponseEventArgs e)
    {
        var eventHandler = ListEventDelegates[GetEventKey(e.CommandType)] as EventHandler<ResponseEventArgs>;
        eventHandler?.Invoke(this, e);
    }

    private object GetEventKey(TraderCommandType commandType)
    {
        return _eventKeys.GetOrAdd(commandType, key => new object());
    }

    private async Task ReadStreamAsync(NetworkStream networkStream)
    {
        if (!networkStream.DataAvailable) return;
        Guid correlationId = Guid.NewGuid();

        var sb = new StringBuilder();

        const int chunkSize = 1_024; //1KB
        var buffer = new byte[chunkSize];
        int bytesRead;

        while ((bytesRead = await networkStream.ReadAsync(buffer)) > 0)
        {
            sb.Append(Encoding.ASCII.GetString(buffer, 0, bytesRead));

            if (bytesRead < chunkSize) break;
        }

        Task.Factory.StartNew(() => OnWrapResponse(new WrapResponseEventArgs(sb, correlationId)),
            TaskCreationOptions.LongRunning);
    }

    protected void OnWrapResponse(WrapResponseEventArgs e)
    {
        WrapResponse?.Invoke(this, e);
    }
}