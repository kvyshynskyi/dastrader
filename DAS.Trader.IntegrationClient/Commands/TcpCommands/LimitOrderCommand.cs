using DAS.Trader.IntegrationClient.Common;
using DAS.Trader.IntegrationClient.Enums;

namespace DAS.Trader.IntegrationClient.Commands.TcpCommands
{
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
            base(token, action, symbol, route, share, price, timeInForce.GetDescription()!)
        {
        }
    }
}