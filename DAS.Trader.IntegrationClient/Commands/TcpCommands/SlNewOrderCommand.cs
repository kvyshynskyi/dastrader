using DAS.Trader.IntegrationClient.Enums;

namespace DAS.Trader.IntegrationClient.Commands.TcpCommands
{
    public sealed class SlNewOrderCommand : BaseTcpCommand
    {
        public SlNewOrderCommand(string symbol, uint locateShares, string route) : base(
            TraderCommandType.SLNEWORDER_COMMAND, false, false,
            symbol, locateShares.ToString(), route)
        {
        }
    }
}