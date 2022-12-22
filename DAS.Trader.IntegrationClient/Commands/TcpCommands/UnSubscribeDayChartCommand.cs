namespace DAS.Trader.IntegrationClient.Commands.TcpCommands
{
    public sealed class UnSubscribeDayChartCommand : BaseUnSubscribeCommand
    {
        public UnSubscribeDayChartCommand(string symbol) : base(symbol, "DAYCHART")
        {
        }
    }
}