using DAS.Trader.IntegrationClient.Enums;

namespace DAS.Trader.IntegrationClient.Commands.TcpCommands
{
    public abstract class BaseUnSubscribeCommand : BaseTcpCommand
    {
        protected BaseUnSubscribeCommand(string symbol, params string[] parameters) : base(
            TraderCommandType.UNSB_COMMAND, false, false,
            new[] { symbol }.Concat(parameters).ToArray())
        {
        }
    }
}