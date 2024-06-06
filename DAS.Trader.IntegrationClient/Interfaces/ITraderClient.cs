using System.Net.Sockets;

using DAS.Trader.IntegrationClient.Response;

namespace DAS.Trader.IntegrationClient.Interfaces;

public interface ITraderClient : IDisposable
{
    Task ConnectAsync();
    INetworkStream GetStream();
    Task<ICommandResult> SendCommandAsync(ITcpCommand command);
    event EventHandler<ResponseEventArgs> PriceInquiry;
    event EventHandler<ResponseEventArgs> LoginResponse;
    event EventHandler<ResponseEventArgs> SlOrder;
    event EventHandler<ResponseEventArgs> SlOrderBegin;
    event EventHandler<ResponseEventArgs> SlOrderEnd;
    event EventHandler<ResponseEventArgs> OrderBegin;
    event EventHandler<ResponseEventArgs> OrderEnd;
    event EventHandler<ResponseEventArgs> Order;
    event EventHandler<ResponseEventArgs> PositionBegin;
    event EventHandler<ResponseEventArgs> PositionEnd;
    event EventHandler<ResponseEventArgs> Position;
    event EventHandler<ResponseEventArgs> TradeBegin;
    event EventHandler<ResponseEventArgs> TradeEnd;
    event EventHandler<ResponseEventArgs> Trade;
    event EventHandler<ResponseEventArgs> OrderAction;
    event EventHandler<ResponseEventArgs> BuyingPower;
    event EventHandler<ResponseEventArgs> ClientCount;
    event EventHandler<ResponseEventArgs> ShortInfo;
    event EventHandler<ResponseEventArgs> Quote;
    event EventHandler<ResponseEventArgs> LimitPrice;
    event EventHandler<ResponseEventArgs> IPosition;
    event EventHandler<ResponseEventArgs> IOrder;
    event EventHandler<ResponseEventArgs> ITrade;
    event EventHandler<ResponseEventArgs> TimeSalesQuote;
    event EventHandler<ResponseEventArgs> Level2Quote;
    event EventHandler<ResponseEventArgs> ChartData;
}