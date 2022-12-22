using DAS.Trader.IntegrationClient.Enums;

namespace DAS.Trader.IntegrationClient.Commands.TcpCommands
{
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
}