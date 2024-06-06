namespace DAS.Trader.IntegrationClient.Commands.TcpCommands
{
    public sealed class SubscribeMinChartCommand : BaseSubscribeCommand
    {
        public SubscribeMinChartCommand(string symbol, DateTime startDate, DateTime? endDate,
            int minType) : base(symbol,
            "MINCHART",
            startDate.ToString("yyyy/MM/dd-HH:mm"),
            endDate?.ToString("yyyy/MM/dd-HH:mm") ?? "LATEST",
            minType.ToString())
        {
        }
    }
}