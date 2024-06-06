namespace DAS.Trader.IntegrationClient.Commands.TcpCommands
{
    public sealed class SubscribeDayChartCommand : BaseSubscribeCommand
    {
        public SubscribeDayChartCommand(string symbol, DateTime startDate, DateTime endDate) : base(symbol,
            "DAYCHART",
            startDate.ToString("yyyy/MM/dd"),
            endDate.ToString("yyyy/MM/dd"))
        {
        }
    }
}