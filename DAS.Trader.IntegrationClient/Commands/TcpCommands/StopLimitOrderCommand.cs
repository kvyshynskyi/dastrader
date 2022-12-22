using DAS.Trader.IntegrationClient.Common;
using DAS.Trader.IntegrationClient.Enums;

namespace DAS.Trader.IntegrationClient.Commands.TcpCommands
{
    /// <summary>
    ///     Stop Limit: Neworder token b/s symbol route share STOPLMT StopPrice Price
    ///     Example:
    ///     NEWORDER 5 B MSFT SMAT 100 STOPLMT 210.5 210.8 TIF=DAY
    /// </summary>
    public sealed class StopLimitOrderCommand : BaseNewOrderCommand
    {
        public StopLimitOrderCommand(string token, OrderAction action, string symbol,
            string route, string share, string stopPrice, string price, TimeInForce timeInForce = TimeInForce.Day) :
            base(token, action, symbol, route, share, "STOPLMT", stopPrice, price, timeInForce.GetDescription()!)
        {
        }
    }
}