using DAS.Trader.IntegrationClient.Common;
using DAS.Trader.IntegrationClient.Enums;

namespace DAS.Trader.IntegrationClient.Commands.TcpCommands
{
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
                price ?? string.Empty, timeInForce.GetDescription()!)
        {
        }
    }
}