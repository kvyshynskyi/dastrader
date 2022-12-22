using DAS.Trader.IntegrationClient.Common;
using DAS.Trader.IntegrationClient.Enums;

namespace DAS.Trader.IntegrationClient.Commands.TcpCommands
{
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
            base(token, action, symbol, route, share, "MKT", timeInForce.GetDescription()!)
        {
        }
    }
}