using DAS.Trader.IntegrationClient.Common;
using DAS.Trader.IntegrationClient.Enums;

namespace DAS.Trader.IntegrationClient.Commands.TcpCommands
{
    public sealed class UnSubscribeLevel2Command : BaseUnSubscribeCommand
    {
        public UnSubscribeLevel2Command(string symbol) : base(symbol, TraderLevels.REGIONAL_LEVEL2.GetDescription()!)
        {
        }
    }
}