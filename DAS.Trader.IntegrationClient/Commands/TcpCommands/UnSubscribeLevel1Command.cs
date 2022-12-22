using DAS.Trader.IntegrationClient.Common;
using DAS.Trader.IntegrationClient.Enums;

namespace DAS.Trader.IntegrationClient.Commands.TcpCommands
{
    public sealed class UnSubscribeLevel1Command : BaseUnSubscribeCommand
    {
        public UnSubscribeLevel1Command(string symbol) : base(symbol, TraderLevels.Level1.GetDescription()!)
        {
        }
    }
}