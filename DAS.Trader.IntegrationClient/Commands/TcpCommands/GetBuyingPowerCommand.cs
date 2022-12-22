using DAS.Trader.IntegrationClient.Enums;
using DAS.Trader.IntegrationClient.Interfaces;

namespace DAS.Trader.IntegrationClient.Commands.TcpCommands
{
    public sealed class GetBuyingPowerCommand : BaseTcpCommand
    {
        public GetBuyingPowerCommand() : base(TraderCommandType.GET_BUYING_POWER_COMMAND)
        {
        }

        public static ITcpCommand Instance => new GetBuyingPowerCommand();
    }
}