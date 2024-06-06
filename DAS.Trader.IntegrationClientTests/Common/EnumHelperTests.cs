using DAS.Trader.IntegrationClient.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DAS.Trader.IntegrationClient.Tests.Common;

public enum TestEnum
{
    [System.ComponentModel.Description("First Value Description")]
    FirstValue,

    [System.ComponentModel.Description("Second Value Description")]
    SecondValue,

    NoDescriptionValue
}

public enum ParamsCountEnum
{
    [ParametersCount(1)] FirstValue,

    [ParametersCount(2)] SecondValue,

    NoParamsCountValue
}

[TestClass]
public class EnumHelperTests
{
    [TestMethod]
    public void GetDescription_ReturnsCorrectDescription()
    {
        // Arrange
        var enumValue = TestEnum.FirstValue;

        // Act
        var description = enumValue.GetDescription();

        // Assert
        Assert.AreEqual("First Value Description", description);
    }

    [TestMethod]
    public void GetDescription_ReturnsNullForNonEnum()
    {
        // Arrange
        var nonEnumValue = 5;

        // Act
        var description = nonEnumValue.GetDescription();

        // Assert
        Assert.IsNull(description);
    }

    [TestMethod]
    public void GetDescription_ReturnsValueNameWhenNoDescription()
    {
        // Arrange
        var enumValue = TestEnum.NoDescriptionValue;

        // Act
        var description = enumValue.GetDescription();

        // Assert
        Assert.AreEqual("NoDescriptionValue", description);
    }

    [TestMethod]
    public void GetParamsCount_ReturnsCorrectParamsCount()
    {
        // Arrange
        var enumValue = ParamsCountEnum.FirstValue;

        // Act
        var paramsCount = enumValue.GetParamsCount();

        // Assert
        Assert.AreEqual(1, paramsCount);
    }

    [TestMethod]
    public void GetParamsCount_ReturnsNullForNonEnum()
    {
        // Arrange
        var nonEnumValue = 5;

        // Act
        var paramsCount = nonEnumValue.GetParamsCount();

        // Assert
        Assert.IsNull(paramsCount);
    }

    [TestMethod]
    public void GetParamsCount_ReturnsZeroWhenNoParamsCountAttribute()
    {
        // Arrange
        var enumValue = ParamsCountEnum.NoParamsCountValue;

        // Act
        var paramsCount = enumValue.GetParamsCount();

        // Assert
        Assert.AreEqual(0, paramsCount);
    }
}