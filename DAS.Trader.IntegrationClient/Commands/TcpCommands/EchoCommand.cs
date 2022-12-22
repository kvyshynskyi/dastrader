using DAS.Trader.IntegrationClient.Enums;
using DAS.Trader.IntegrationClient.Interfaces;

namespace DAS.Trader.IntegrationClient.Commands.TcpCommands
{
    public sealed class EchoCommand : BaseTcpCommand
    {
        public EchoCommand() : base(TraderCommandType.ECHO_COMMAND)
        {
        }

        public static ITcpCommand Instance => new EchoCommand();
    }
}