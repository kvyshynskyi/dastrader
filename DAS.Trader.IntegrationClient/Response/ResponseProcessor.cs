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

    public async Task ListenAsync(NetworkStream networkStream)
    {
        while (!CancellationToken.IsCancellationRequested && networkStream.CanRead)
            await ReadStreamAsync(networkStream);
    }

    private void RiseEvent(ResponseEventArgs e)
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
        var correlationId = Guid.NewGuid();

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

    #region Events

    private event EventHandler<WrapResponseEventArgs> WrapResponse;

    private void OnWrapResponse(WrapResponseEventArgs e)
    {
        WrapResponse?.Invoke(this, e);
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

    public event EventHandler<ResponseEventArgs> OrderBegin
    {
        add => ListEventDelegates.AddHandler(GetEventKey(TraderCommandType.ORDER_BEGIN_RESPONSE), value);
        remove => ListEventDelegates.RemoveHandler(GetEventKey(TraderCommandType.ORDER_BEGIN_RESPONSE), value);
    }

    public event EventHandler<ResponseEventArgs> OrderEnd
    {
        add => ListEventDelegates.AddHandler(GetEventKey(TraderCommandType.ORDER_END_RESPONSE), value);
        remove => ListEventDelegates.RemoveHandler(GetEventKey(TraderCommandType.ORDER_END_RESPONSE), value);
    }

    public event EventHandler<ResponseEventArgs> Order
    {
        add => ListEventDelegates.AddHandler(GetEventKey(TraderCommandType.ORDER_RESPONSE), value);
        remove => ListEventDelegates.RemoveHandler(GetEventKey(TraderCommandType.ORDER_RESPONSE), value);
    }

    public event EventHandler<ResponseEventArgs> PositionBegin
    {
        add => ListEventDelegates.AddHandler(GetEventKey(TraderCommandType.POS_BEGIN_RESPONSE), value);
        remove => ListEventDelegates.RemoveHandler(GetEventKey(TraderCommandType.POS_BEGIN_RESPONSE), value);
    }

    public event EventHandler<ResponseEventArgs> PositionEnd
    {
        add => ListEventDelegates.AddHandler(GetEventKey(TraderCommandType.POS_END_RESPONSE), value);
        remove => ListEventDelegates.RemoveHandler(GetEventKey(TraderCommandType.POS_END_RESPONSE), value);
    }

    public event EventHandler<ResponseEventArgs> Position
    {
        add => ListEventDelegates.AddHandler(GetEventKey(TraderCommandType.POS_RESPONSE), value);
        remove => ListEventDelegates.RemoveHandler(GetEventKey(TraderCommandType.POS_RESPONSE), value);
    }

    public event EventHandler<ResponseEventArgs> TradeBegin
    {
        add => ListEventDelegates.AddHandler(GetEventKey(TraderCommandType.TRADE_BEGIN_RESPONSE), value);
        remove => ListEventDelegates.RemoveHandler(GetEventKey(TraderCommandType.TRADE_BEGIN_RESPONSE), value);
    }

    public event EventHandler<ResponseEventArgs> TradeEnd
    {
        add => ListEventDelegates.AddHandler(GetEventKey(TraderCommandType.TRADE_END_RESPONSE), value);
        remove => ListEventDelegates.RemoveHandler(GetEventKey(TraderCommandType.TRADE_END_RESPONSE), value);
    }

    public event EventHandler<ResponseEventArgs> Trade
    {
        add => ListEventDelegates.AddHandler(GetEventKey(TraderCommandType.TRADE_RESPONSE), value);
        remove => ListEventDelegates.RemoveHandler(GetEventKey(TraderCommandType.TRADE_RESPONSE), value);
    }

    public event EventHandler<ResponseEventArgs> OrderAction
    {
        add => ListEventDelegates.AddHandler(GetEventKey(TraderCommandType.ORDER_ACTION_MESSAGE_RESPONSE), value);
        remove => ListEventDelegates.RemoveHandler(GetEventKey(TraderCommandType.ORDER_ACTION_MESSAGE_RESPONSE), value);
    }

    public event EventHandler<ResponseEventArgs> BuyingPower
    {
        add => ListEventDelegates.AddHandler(GetEventKey(TraderCommandType.BUYING_POWER_RESPONSE), value);
        remove => ListEventDelegates.RemoveHandler(GetEventKey(TraderCommandType.BUYING_POWER_RESPONSE), value);
    }

    public event EventHandler<ResponseEventArgs> ClientCount
    {
        add => ListEventDelegates.AddHandler(GetEventKey(TraderCommandType.CLIENT_RESPONSE), value);
        remove => ListEventDelegates.RemoveHandler(GetEventKey(TraderCommandType.CLIENT_RESPONSE), value);
    }

    public event EventHandler<ResponseEventArgs> ShortInfo
    {
        add => ListEventDelegates.AddHandler(GetEventKey(TraderCommandType.SHORTINFO_RESPONSE), value);
        remove => ListEventDelegates.RemoveHandler(GetEventKey(TraderCommandType.SHORTINFO_RESPONSE), value);
    }

    public event EventHandler<ResponseEventArgs> Quote
    {
        add => ListEventDelegates.AddHandler(GetEventKey(TraderCommandType.QUOTE_RESPONSE), value);
        remove => ListEventDelegates.RemoveHandler(GetEventKey(TraderCommandType.QUOTE_RESPONSE), value);
    }

    public event EventHandler<ResponseEventArgs> LimitPrice
    {
        add => ListEventDelegates.AddHandler(GetEventKey(TraderCommandType.LDLU_RESPONSE), value);
        remove => ListEventDelegates.RemoveHandler(GetEventKey(TraderCommandType.LDLU_RESPONSE), value);
    }

    public event EventHandler<ResponseEventArgs> IPosition
    {
        add => ListEventDelegates.AddHandler(GetEventKey(TraderCommandType.IPOS_RESPONSE), value);
        remove => ListEventDelegates.RemoveHandler(GetEventKey(TraderCommandType.IPOS_RESPONSE), value);
    }

    public event EventHandler<ResponseEventArgs> IOrder
    {
        add => ListEventDelegates.AddHandler(GetEventKey(TraderCommandType.IORDER_RESPONSE), value);
        remove => ListEventDelegates.RemoveHandler(GetEventKey(TraderCommandType.IORDER_RESPONSE), value);
    }

    public event EventHandler<ResponseEventArgs> ITrade
    {
        add => ListEventDelegates.AddHandler(GetEventKey(TraderCommandType.ITRADE_RESPONSE), value);
        remove => ListEventDelegates.RemoveHandler(GetEventKey(TraderCommandType.ITRADE_RESPONSE), value);
    }

    public event EventHandler<ResponseEventArgs> TimeSalesQuote
    {
        add => ListEventDelegates.AddHandler(GetEventKey(TraderCommandType.TS_RESPONSE), value);
        remove => ListEventDelegates.RemoveHandler(GetEventKey(TraderCommandType.TS_RESPONSE), value);
    }

    public event EventHandler<ResponseEventArgs> Level2Quote
    {
        add => ListEventDelegates.AddHandler(GetEventKey(TraderCommandType.LV2_RESPONSE), value);
        remove => ListEventDelegates.RemoveHandler(GetEventKey(TraderCommandType.LV2_RESPONSE), value);
    }

    public event EventHandler<ResponseEventArgs> ChartData
    {
        add => ListEventDelegates.AddHandler(GetEventKey(TraderCommandType.BAR_RESPONSE), value);
        remove => ListEventDelegates.RemoveHandler(GetEventKey(TraderCommandType.BAR_RESPONSE), value);
    }

    #endregion
}