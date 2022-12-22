namespace DAS.Trader.IntegrationClient.Commands.TcpCommands
{
    public sealed class UnSubscribeMinChartCommand : BaseUnSubscribeCommand
    {
        public UnSubscribeMinChartCommand(string symbol) : base(symbol, "MINCHART")
        {
        }
    }
}