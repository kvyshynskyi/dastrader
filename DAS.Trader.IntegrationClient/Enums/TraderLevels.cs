using System.ComponentModel;

namespace DAS.Trader.IntegrationClient.Enums;

public enum TraderLevels
{
    [Description("Lv1")] Level1,
    [Description("Lv2")] REGIONAL_LEVEL2,
    [Description("Lv2 INET")] TOTAL_VIEW_LEVEL2,
    [Description("Lv2 ARCA")] ARCA_LEVEL2,
    [Description("Lv2 BATS")] BATS_LEVEL2
}