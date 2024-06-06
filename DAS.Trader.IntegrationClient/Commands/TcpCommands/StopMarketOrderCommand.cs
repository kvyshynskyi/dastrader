using DAS.Trader.IntegrationClient.Common;
using DAS.Trader.IntegrationClient.Enums;

namespace DAS.Trader.IntegrationClient.Commands.TcpCommands
{
    /// <summary>
    ///     Stop Market: Neworder token b/s symbol route share STOPMKT StopPrice
    ///     Example:
    ///     NEWORDER 4 S MSFT SMAT 100 STOPMKT 210.5 TIF=DAY
    /// </summary>
    public sealed class StopMarketOrderCommand : BaseNewOrderCommand
    {
        public StopMarketOrderCommand(string token, OrderAction action, string symbol,
            string route, string share, string stopPrice, TimeInForce timeInForce = TimeInForce.Day) :
            base(token, action, symbol, route, share, "STOPMKT", stopPrice, timeInForce.GetDescription()!)
        {
        }
    }
}