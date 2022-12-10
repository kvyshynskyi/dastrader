using DAS.Trader.IntegrationClient.Client;
using DAS.Trader.IntegrationClient.Commands.Interfaces;

namespace DAS.Trader.IntegrationClient.Commands.TcpCommands;

public sealed class LoginCommand : BaseTcpCommand
{
    public LoginCommand(string login, string password) : base(TraderCommandType.LOGIN_COMMAND, true,false, login, password)
    {
    }

    public override void Subscribe(ResponseProcessor responseProcessor)
    {
        responseProcessor.LoginResponse += ResponseProcessor_LoginResponse;
    }

    private void ResponseProcessor_LoginResponse(object? sender, ResponseEventHandlerArgs e)
    {
        this.Result = e.Message;
        this.HasResult = true;
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

