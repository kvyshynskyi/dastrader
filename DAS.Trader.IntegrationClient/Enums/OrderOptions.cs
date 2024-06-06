using System.ComponentModel;

namespace DAS.Trader.IntegrationClient.Enums;

public enum OrderOptions
{
    [Description("MID")] MID,
    [Description("AGG")] AGG,
    [Description("PRIM")] PRIM,
    [Description("LAST")] LAST
}