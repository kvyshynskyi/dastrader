using System.ComponentModel;

namespace DAS.Trader.IntegrationClient.Enums;

public enum TimeInForce
{
    [Description("DAY")] Day,
    [Description("DAY+")] DayPlus,
    [Description("IOC")] IOC,
    [Description("AtOpen")] AtOpen,
    [Description("AtClose")] AtClose,
    [Description("FOK")] FOK,
    [Description("GTC")] GTC
}