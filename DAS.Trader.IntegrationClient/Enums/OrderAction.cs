using System.ComponentModel;

namespace DAS.Trader.IntegrationClient.Enums;

public enum OrderAction
{
    [Description("B")] Buy,
    [Description("S")] Sell,
    [Description("SS")] Short,
    [Description("BO")] BuyToOpen,
    [Description("BC")] BuyToClose,
    [Description("SO")] SellToOpen,
    [Description("SC")] SellToClose
}