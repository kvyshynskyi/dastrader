using DAS.Trader.IntegrationClient.Enums;
using DAS.Trader.IntegrationClient.Interfaces;

namespace DAS.Trader.IntegrationClient.Commands.TcpCommands
{
    public sealed class QuitCommand : BaseTcpCommand
    {
        public QuitCommand() : base(TraderCommandType.QUIT_COMMAND)
        {
        }

        public static ITcpCommand Instance => new QuitCommand();
    }
}