using DAS.Trader.IntegrationClient.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DAS.Trader.IntegrationClient.Tests.Common;

[TestClass]
public class ParametersCountAttributeTests
{
    [TestMethod]
    public void Attribute_Initialization()
    {
        // Arrange
        var expectedParamsCount = 5;

        // Act
        var attribute = new ParametersCountAttribute(expectedParamsCount);

        // Assert
        Assert.AreEqual(expectedParamsCount, attribute.ParamsCount);
    }

    [TestMethod]
    public void Attribute_Applied_To_Method()
    {
        // Arrange
        var methodInfo = typeof(TestClass).GetMethod(nameof(TestClass.MethodWithAttribute));

        // Act
        var attributes = methodInfo.GetCustomAttributes(typeof(ParametersCountAttribute), false);
        var attribute = attributes.Length > 0 ? attributes[0] as ParametersCountAttribute : null;

        // Assert
        Assert.IsNotNull(attribute);
        Assert.AreEqual(3, attribute.ParamsCount);
    }

    [TestMethod]
    public void Attribute_Applied_To_Class()
    {
        // Arrange
        var typeInfo = typeof(ClassWithAttribute);

        // Act
        var attributes = typeInfo.GetCustomAttributes(typeof(ParametersCountAttribute), false);
        var attribute = attributes.Length > 0 ? attributes[0] as ParametersCountAttribute : null;

        // Assert
        Assert.IsNotNull(attribute);
        Assert.AreEqual(2, attribute.ParamsCount);
    }
}

[ParametersCount(2)]
public class ClassWithAttribute
{
}

public class TestClass
{
    [ParametersCount(3)]
    public void MethodWithAttribute(int param1, int param2, int param3)
    {
    }
}