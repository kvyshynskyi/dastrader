using DAS.Trader.IntegrationClient.Enums;
using DAS.Trader.IntegrationClient.Interfaces;

namespace DAS.Trader.IntegrationClient.Commands.TcpCommands
{
    public sealed class PosrefreshCommand : BaseTcpCommand
    {
        public PosrefreshCommand() : base(TraderCommandType.POSREFRESH_COMMAND)
        {
        }

        public static ITcpCommand Instance => new PosrefreshCommand();
    }
}