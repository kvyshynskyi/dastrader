namespace DAS.Trader.IntegrationClient.Commands.TcpCommands
{
    public sealed class SubscribeTimeSalesCommand : BaseSubscribeCommand
    {
        public SubscribeTimeSalesCommand(string symbol) : base(symbol, "tms")
        {
        }
    }
}