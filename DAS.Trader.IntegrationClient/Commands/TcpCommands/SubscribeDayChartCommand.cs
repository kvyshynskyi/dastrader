namespace DAS.Trader.IntegrationClient.Commands.TcpCommands
{
    public sealed class SubscribeDayChartCommand : BaseSubscribeCommand
    {
        public SubscribeDayChartCommand(string symbol, DateTime startDate, DateTime endDate) : base(symbol,
            "DAYCHART",
            startDate.ToString("YYYY/MM/DD"),
            endDate.ToString("YYYY/MM/DD"))
        {
        }
    }
}