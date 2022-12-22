using DAS.Trader.IntegrationClient.Enums;

namespace DAS.Trader.IntegrationClient.Commands.TcpCommands
{
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
}