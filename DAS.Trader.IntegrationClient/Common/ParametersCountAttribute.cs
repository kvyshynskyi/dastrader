namespace DAS.Trader.IntegrationClient.Common;

public class ParametersCountAttribute : Attribute
{
    public ParametersCountAttribute(int paramsCount)
    {
        ParamsCount = paramsCount;
    }

    public int ParamsCount { get; }
}