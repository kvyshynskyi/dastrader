using DAS.Trader.IntegrationClient.Enums;

namespace DAS.Trader.IntegrationClient.Commands.TcpCommands
{
    public sealed class GetShortInfoCommand : BaseTcpCommand
    {
        public GetShortInfoCommand(string symbol) : base(TraderCommandType.GET_SHORTINFO_COMMAND, false, false, symbol)
        {
        }
    }
}