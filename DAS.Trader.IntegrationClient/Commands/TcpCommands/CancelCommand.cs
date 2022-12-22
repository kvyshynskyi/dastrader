using DAS.Trader.IntegrationClient.Enums;

namespace DAS.Trader.IntegrationClient.Commands.TcpCommands
{
    public sealed class CancelCommand : BaseTcpCommand
    {
        public CancelCommand(int? orderId) : base(TraderCommandType.CANCEL_COMMAND, false, false,
            orderId?.ToString() ?? "ALL")
        {
        }
    }
}