using DAS.Trader.IntegrationClient.Common;
using DAS.Trader.IntegrationClient.Enums;

namespace DAS.Trader.IntegrationClient.Commands.TcpCommands
{
    public abstract class BaseNewOrderCommand : BaseTcpCommand
    {
        protected BaseNewOrderCommand(string token, OrderAction action, string symbol, params string[] parameters) :
            base(TraderCommandType.NEWORDER_COMMAND, false, false,
                new[] { token, action.GetDescription(), symbol }.Concat(parameters).ToArray()!)
        {
        }
    }
}