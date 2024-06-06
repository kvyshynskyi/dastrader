using DAS.Trader.IntegrationClient.Enums;

namespace DAS.Trader.IntegrationClient.Commands.TcpCommands
{
    public abstract class BaseSlOfferOperationCommand : BaseTcpCommand
    {
        protected BaseSlOfferOperationCommand(string locateOrderId, string operation) : base(
            TraderCommandType.SLOFFEROPERATION_COMMAND, false, false,
            locateOrderId, operation)
        {
        }
    }
}