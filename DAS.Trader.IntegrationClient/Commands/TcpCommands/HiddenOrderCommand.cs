using DAS.Trader.IntegrationClient.Enums;

namespace DAS.Trader.IntegrationClient.Commands.TcpCommands;

#region (Un)Subscribe Commands

#endregion

#region Short Locate Commands

#endregion

#region New Order Commands

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
