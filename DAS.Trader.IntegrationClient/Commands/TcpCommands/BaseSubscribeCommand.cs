using DAS.Trader.IntegrationClient.Enums;

namespace DAS.Trader.IntegrationClient.Commands.TcpCommands
{
    public abstract class BaseSubscribeCommand : BaseTcpCommand
    {
        protected BaseSubscribeCommand(string symbol, params string[] parameters) : base(
            TraderCommandType.SB_COMMAND, false, false,
            new[] { symbol }.Concat(parameters).ToArray())
        {
        }
    }
}