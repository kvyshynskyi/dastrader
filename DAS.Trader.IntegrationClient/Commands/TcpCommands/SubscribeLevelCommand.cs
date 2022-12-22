using DAS.Trader.IntegrationClient.Common;
using DAS.Trader.IntegrationClient.Enums;

namespace DAS.Trader.IntegrationClient.Commands.TcpCommands
{
    public sealed class SubscribeLevelCommand : BaseSubscribeCommand
    {
        public SubscribeLevelCommand(string symbol, TraderLevels level) : base(symbol, level.GetDescription()!)
        {
        }
    }
}