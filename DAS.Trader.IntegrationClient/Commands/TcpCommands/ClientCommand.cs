using DAS.Trader.IntegrationClient.Enums;
using DAS.Trader.IntegrationClient.Interfaces;

namespace DAS.Trader.IntegrationClient.Commands.TcpCommands
{
    public sealed class ClientCommand : BaseTcpCommand
    {
        public ClientCommand() : base(TraderCommandType.CLIENT_COMMAND)
        {
        }

        public static ITcpCommand Instance => new ClientCommand();
    }
}