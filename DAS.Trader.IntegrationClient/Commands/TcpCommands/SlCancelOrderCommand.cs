using DAS.Trader.IntegrationClient.Enums;

namespace DAS.Trader.IntegrationClient.Commands.TcpCommands
{
    public sealed class SlCancelOrderCommand : BaseTcpCommand
    {
        public SlCancelOrderCommand(string locateOrderId) : base(
            TraderCommandType.SLCANCELORDER_COMMAND, false, false,
            locateOrderId)
        {
        }
    }
}