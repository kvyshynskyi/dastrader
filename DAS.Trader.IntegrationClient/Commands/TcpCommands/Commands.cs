using DAS.Trader.IntegrationClient.Client;
using DAS.Trader.IntegrationClient.Commands.Interfaces;
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

public sealed class OrderServerConnectionStatusCommand : BaseTcpCommand
{
    public OrderServerConnectionStatusCommand() : base(TraderCommandType.ORDER_SERVER_CONNECTION_STATUS_COMMAND)
    {
    }

    public static ITcpCommand Instance => new OrderServerConnectionStatusCommand();
}

public sealed class OrderServerLogOnStatusCommand : BaseTcpCommand
{
    public OrderServerLogOnStatusCommand() : base(TraderCommandType.ORDER_SERVER_LOGON_STATUS_COMMAND)
    {
    }

    public static ITcpCommand Instance => new OrderServerLogOnStatusCommand();
}

public sealed class QuoteServerConnectionStatusCommand : BaseTcpCommand
{
    public QuoteServerConnectionStatusCommand() : base(TraderCommandType.QUOTE_SERVER_CONNECTION_STATUS_COMMAND)
    {
    }

    public static ITcpCommand Instance => new QuoteServerConnectionStatusCommand();
}

public sealed class QuoteServerLogOnStatusCommand : BaseTcpCommand
{
    public QuoteServerLogOnStatusCommand() : base(TraderCommandType.QUOTE_SERVER_LOGON_STATUS_COMMAND)
    {
    }

    public static ITcpCommand Instance => new QuoteServerLogOnStatusCommand();
}

public sealed class ClientCommand : BaseTcpCommand
{
    public ClientCommand() : base(TraderCommandType.CLIENT_COMMAND)
    {
    }

    public static ITcpCommand Instance => new ClientCommand();
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

public sealed class SlAvailQueryCommand : BaseTcpCommand
{
    public SlAvailQueryCommand(string account,string symbol) : base(
        TraderCommandType.SLAVAILQUERY_COMMAND, false, false,
        account, symbol)
    {
    }
}



#endregion
