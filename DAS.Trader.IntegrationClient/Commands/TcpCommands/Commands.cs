using DAS.Trader.IntegrationClient.Commands.Interfaces;
using DAS.Trader.IntegrationClient.Common;
using DAS.Trader.IntegrationClient.Enums;
using DAS.Trader.IntegrationClient.Response;

namespace DAS.Trader.IntegrationClient.Commands.TcpCommands;

public sealed class LoginCommand : BaseTcpCommand
{
    public LoginCommand(string login, string password, string account) : base(TraderCommandType.LOGIN_COMMAND,
        true, false, login, password, account)
    {
    }

    public override void Subscribe(ResponseProcessor responseProcessor)
    {
        responseProcessor.LoginResponse += ResponseProcessor_LoginResponse;
    }

    private void ResponseProcessor_LoginResponse(object? sender, ResponseEventArgs e)
    {
        Result = e.Parameters?.Length == 1 ? e.Parameters[0] : e.Message ?? string.Empty;

        HasResult = true;
    }

    public override void Unsubscribe(ResponseProcessor responseProcessor)
    {
        responseProcessor.LoginResponse -= ResponseProcessor_LoginResponse;
    }
}

public sealed class QuitCommand : BaseTcpCommand
{
    public QuitCommand() : base(TraderCommandType.QUIT_COMMAND)
    {
    }

    public static ITcpCommand Instance => new QuitCommand();
}

public sealed class ClientCommand : BaseTcpCommand
{
    public ClientCommand() : base(TraderCommandType.CLIENT_COMMAND)
    {
    }

    public static ITcpCommand Instance => new ClientCommand();
}

public sealed class PosrefreshCommand : BaseTcpCommand
{
    public PosrefreshCommand() : base(TraderCommandType.POSREFRESH_COMMAND)
    {
    }

    public static ITcpCommand Instance => new PosrefreshCommand();
}

public sealed class CancelCommand : BaseTcpCommand
{
    public CancelCommand(int? orderId) : base(TraderCommandType.CANCEL_COMMAND, false, false,
        orderId?.ToString() ?? "ALL")
    {
    }
}

public sealed class EchoCommand : BaseTcpCommand
{
    public EchoCommand() : base(TraderCommandType.ECHO_COMMAND)
    {
    }

    public static ITcpCommand Instance => new EchoCommand();
}

public sealed class GetBuyingPowerCommand : BaseTcpCommand
{
    public GetBuyingPowerCommand() : base(TraderCommandType.GET_BUYING_POWER_COMMAND)
    {
    }

    public static ITcpCommand Instance => new GetBuyingPowerCommand();
}

public sealed class GetShortInfoCommand : BaseTcpCommand
{
    public GetShortInfoCommand(string symbol) : base(TraderCommandType.GET_SHORTINFO_COMMAND, false, false, symbol)
    {
    }
}

#region (Un)Subscribe Commands

public abstract class BaseSubscribeCommand : BaseTcpCommand
{
    protected BaseSubscribeCommand(string symbol, params string[] parameters) : base(
        TraderCommandType.SB_COMMAND, false, false,
        new[] { symbol }.Concat(parameters).ToArray())
    {
    }
}

public sealed class SubscribeLevelCommand : BaseSubscribeCommand
{
    public SubscribeLevelCommand(string symbol, TraderLevels level) : base(symbol, level.GetDescription())
    {
    }
}

public sealed class SubscribeTimeSalesCommand : BaseSubscribeCommand
{
    public SubscribeTimeSalesCommand(string symbol) : base(symbol, "tms")
    {
    }
}

public sealed class SubscribeDayChartCommand : BaseSubscribeCommand
{
    public SubscribeDayChartCommand(string symbol, DateTime startDate, DateTime endDate) : base(symbol,
        "DAYCHART",
        startDate.ToString("YYYY/MM/DD"),
        endDate.ToString("YYYY/MM/DD"))
    {
    }
}

public sealed class SubscribeMinChartCommand : BaseSubscribeCommand
{
    public SubscribeMinChartCommand(string symbol, DateTime startDate, DateTime? endDate,
        int minType) : base(symbol,
        "MINCHART",
        startDate.ToString("YYYY/MM/DD-HH:MM"),
        endDate?.ToString("YYYY/MM/DD-HH:MM") ?? "LATEST",
        minType.ToString())
    {
    }
}

public abstract class BaseUnSubscribeCommand : BaseTcpCommand
{
    protected BaseUnSubscribeCommand(string symbol, params string[] parameters) : base(
        TraderCommandType.UNSB_COMMAND, false, false,
        new[] { symbol }.Concat(parameters).ToArray())
    {
    }
}

public sealed class UnSubscribeLevel1Command : BaseUnSubscribeCommand
{
    public UnSubscribeLevel1Command(string symbol) : base(symbol, TraderLevels.Level1.GetDescription())
    {
    }
}

public sealed class UnSubscribeLevel2Command : BaseUnSubscribeCommand
{
    public UnSubscribeLevel2Command(string symbol) : base(symbol, TraderLevels.REGIONAL_LEVEL2.GetDescription())
    {
    }
}

public sealed class UnSubscribeTimeSalesCommand : BaseUnSubscribeCommand
{
    public UnSubscribeTimeSalesCommand(string symbol) : base(symbol, "tms")
    {
    }
}

public sealed class UnSubscribeDayChartCommand : BaseUnSubscribeCommand
{
    public UnSubscribeDayChartCommand(string symbol) : base(symbol, "DAYCHART")
    {
    }
}

public sealed class UnSubscribeMinChartCommand : BaseUnSubscribeCommand
{
    public UnSubscribeMinChartCommand(string symbol) : base(symbol, "MINCHART")
    {
    }
}

#endregion

#region Short Locate Commands

public sealed class SlPriceInquireCommand : BaseTcpCommand
{
    public SlPriceInquireCommand(string symbol, uint locateShares = 100, string route = "ALLROUTE") : base(
        TraderCommandType.SLPRICEINQUIRE_COMMAND, false, false,
        symbol, locateShares.ToString(), route)
    {
    }
}

public sealed class SlNewOrderCommand : BaseTcpCommand
{
    public SlNewOrderCommand(string symbol, uint locateShares, string route) : base(
        TraderCommandType.SLNEWORDER_COMMAND, false, false,
        symbol, locateShares.ToString(), route)
    {
    }
}

public sealed class SlCancelOrderCommand : BaseTcpCommand
{
    public SlCancelOrderCommand(string locateOrderId) : base(
        TraderCommandType.SLCANCELORDER_COMMAND, false, false,
        locateOrderId)
    {
    }
}

public abstract class BaseSlOfferOperationCommand : BaseTcpCommand
{
    protected BaseSlOfferOperationCommand(string locateOrderId, string operation) : base(
        TraderCommandType.SLOFFEROPERATION_COMMAND, false, false,
        locateOrderId, operation)
    {
    }
}

public sealed class SlOfferOperationAcceptCommand : BaseSlOfferOperationCommand
{
    public SlOfferOperationAcceptCommand(string locateOrderId) : base(locateOrderId, "Accept")
    {
    }
}

public sealed class SlOfferOperationRejectCommand : BaseSlOfferOperationCommand
{
    public SlOfferOperationRejectCommand(string locateOrderId) : base(locateOrderId, "Reject")
    {
    }
}

#endregion

#region New Order Commands

public abstract class BaseNewOrderCommand : BaseTcpCommand
{
    protected BaseNewOrderCommand(string token, OrderAction action, string symbol, params string[] parameters) :
        base(TraderCommandType.NEWORDER_COMMAND, false, false,
            new[] { token, action.GetDescription(), symbol }.Concat(parameters).ToArray())
    {
    }
}

/// <summary>
///     Limit order:
///     Neworder token b/s symbol route share price
///     Example:
///     NEWORDER 1 B MSFT ARCA 100 200.5 TIF=DAY+
/// </summary>
public sealed class LimitOrderCommand : BaseNewOrderCommand
{
    public LimitOrderCommand(string token, OrderAction action, string symbol,
        string route, string share, string price, TimeInForce timeInForce = TimeInForce.DayPlus) :
        base(token, action, symbol, route, share, price, timeInForce.GetDescription())
    {
    }
}

/// <summary>
///     Market order:
///     Neworder token b/s symbol route share MKT
///     Example:
///     NEWORDER 2 S MSFT SMAT 100 MKT TIF = DAY
/// </summary>
public sealed class MarketOrderCommand : BaseNewOrderCommand
{
    public MarketOrderCommand(string token, OrderAction action, string symbol
        , string route, string share, TimeInForce timeInForce = TimeInForce.Day) :
        base(token, action, symbol, route, share, "MKT", timeInForce.GetDescription())
    {
    }
}

/// <summary>
///     Peg Order:
///     Neworder token b/s symbol route share PEG MID/AGG/PRIM/LAST(optional field) price(optional field)
///     Example:
///     NEWORDER 3 B MSFT INET 100 PEG MID 200.5 TIF=GTC
/// </summary>
public sealed class PegOrderCommand : BaseNewOrderCommand
{
    public PegOrderCommand(string token, OrderAction action, string symbol
        , string route, string share, OrderOptions? orderOptions, string? price,
        TimeInForce timeInForce = TimeInForce.GTC) :
        base(token, action, symbol, route, share, "PEG", orderOptions?.GetDescription() ?? string.Empty,
            price ?? string.Empty, timeInForce.GetDescription())
    {
    }
}

/// <summary>
///     Stop Market: Neworder token b/s symbol route share STOPMKT StopPrice
///     Example:
///     NEWORDER 4 S MSFT SMAT 100 STOPMKT 210.5 TIF=DAY
/// </summary>
public sealed class StopMarketOrderCommand : BaseNewOrderCommand
{
    public StopMarketOrderCommand(string token, OrderAction action, string symbol,
        string route, string share, string stopPrice, TimeInForce timeInForce = TimeInForce.Day) :
        base(token, action, symbol, route, share, "STOPMKT", stopPrice, timeInForce.GetDescription())
    {
    }
}

/// <summary>
///     Stop Limit: Neworder token b/s symbol route share STOPLMT StopPrice Price
///     Example:
///     NEWORDER 5 B MSFT SMAT 100 STOPLMT 210.5 210.8 TIF=DAY
/// </summary>
public sealed class StopLimitOrderCommand : BaseNewOrderCommand
{
    public StopLimitOrderCommand(string token, OrderAction action, string symbol,
        string route, string share, string stopPrice, string price, TimeInForce timeInForce = TimeInForce.Day) :
        base(token, action, symbol, route, share, "STOPLMT", stopPrice, price, timeInForce.GetDescription())
    {
    }
}

/// <summary>
///     Stop Trailing: Neworder token b/s symbol route share STOPTRAILING TrailPrice
///     Example:
///     NEWORDER 6 S MSFT SMAT 100 STOPTRAILING 0.2
/// </summary>
public sealed class StopTrailingOrderCommand : BaseNewOrderCommand
{
    public StopTrailingOrderCommand(string token, OrderAction action, string symbol,
        string route, string share, string trailPrice) :
        base(token, action, symbol, route, share, "STOPTRAILING", trailPrice)
    {
    }
}

/// <summary>
///     Stop Range: Neworder token b/s symbol route share STOPRANGE/STOPRANGEMKT LowPrice HighPrice
///     Example:
///     NEWORDER 7 B MSFT SMAT 100 STOPRANGE 210.2 210.6
///     NEWORDER 7 B MSFT SMAT 100 STOPRANGEMKT 210.2 210.6
/// </summary>
public sealed class StopRangeOrderCommand : BaseNewOrderCommand
{
    public StopRangeOrderCommand(string token, OrderAction action, string symbol,
        string route, string share, bool isStopRangeMarket, string lowPrice, string highPrice) :
        base(token, action, symbol, route, share,
            isStopRangeMarket ? "STOPRANGEMKT" : "STOPRANGE", lowPrice, highPrice)
    {
    }
}

/// <summary>
///     Hidden order:
///     Neworder token b/s symbol route share price Display = 0 / num
///     NEWORDER 1 B MSFT ARCA 300 200.5 GTC=DAY+ Display=0
/// </summary>
public sealed class HiddenOrderCommand : BaseNewOrderCommand
{
    public HiddenOrderCommand(string token, OrderAction action, string symbol,
        string route, string share, string price, uint display = 0) :
        base(token, action, symbol, route, share, price,
            "GTC=DAY+", $"Display={display}")
    {
    }
}

#endregion
