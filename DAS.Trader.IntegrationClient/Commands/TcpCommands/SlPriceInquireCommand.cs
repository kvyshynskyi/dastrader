using DAS.Trader.IntegrationClient.Enums;

namespace DAS.Trader.IntegrationClient.Commands.TcpCommands
{
    public sealed class SlPriceInquireCommand : BaseTcpCommand
    {
        public SlPriceInquireCommand(string symbol, uint locateShares = 100, string route = "ALLROUTE") : base(
            TraderCommandType.SLPRICEINQUIRE_COMMAND, false, false,
            symbol, locateShares.ToString(), route)
        {
        }
    }
}