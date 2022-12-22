namespace DAS.Trader.IntegrationClient.Commands.TcpCommands
{
    public sealed class SlOfferOperationAcceptCommand : BaseSlOfferOperationCommand
    {
        public SlOfferOperationAcceptCommand(string locateOrderId) : base(locateOrderId, "Accept")
        {
        }
    }
}