namespace DAS.Trader.IntegrationClient.Commands.TcpCommands
{
    public sealed class UnSubscribeTimeSalesCommand : BaseUnSubscribeCommand
    {
        public UnSubscribeTimeSalesCommand(string symbol) : base(symbol, "tms")
        {
        }
    }
}