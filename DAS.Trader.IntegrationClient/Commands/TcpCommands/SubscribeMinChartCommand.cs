namespace DAS.Trader.IntegrationClient.Commands.TcpCommands
{
    public sealed class SubscribeMinChartCommand : BaseSubscribeCommand
    {
        public SubscribeMinChartCommand(string symbol, DateTime startDate, DateTime? endDate,
            int minType) : base(symbol,
            "MINCHART",
            startDate.ToString("YYYY/MM/DD-HH:MM"),
            endDate?.ToString("YYYY/MM/DD-HH:MM") ?? "LATEST",
            minType.ToString())
        {
        }
    }
}