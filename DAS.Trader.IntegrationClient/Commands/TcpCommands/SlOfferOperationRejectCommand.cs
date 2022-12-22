namespace DAS.Trader.IntegrationClient.Commands.TcpCommands
{
    public sealed class SlOfferOperationRejectCommand : BaseSlOfferOperationCommand
    {
        public SlOfferOperationRejectCommand(string locateOrderId) : base(locateOrderId, "Reject")
        {
        }
    }
}