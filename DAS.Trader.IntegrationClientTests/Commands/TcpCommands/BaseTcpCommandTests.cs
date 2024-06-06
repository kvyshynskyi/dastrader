using System.Reflection;
using System.Text;
using DAS.Trader.IntegrationClient.Commands.TcpCommands;
using DAS.Trader.IntegrationClient.Enums;
using DAS.Trader.IntegrationClient.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DAS.Trader.IntegrationClient.Tests.Commands.TcpCommands;

[TestClass]
public class BaseTcpCommandTests
{
    private const TraderCommandType CommandType = TraderCommandType.LOGIN_RESPONSE;
    private const string CommandName = "#LOGIN";
    private TestTcpCommand _testTcpCommand;
    private readonly string[] commandParams = { "param1", "param2" };

    [TestInitialize]
    public void Setup()
    {
        _testTcpCommand = new TestTcpCommand(CommandType, true, true, commandParams);
    }

    [TestMethod]
    public void Constructor_ShouldInitializeProperties()
    {
        // Arrange
        var command = new TestTcpCommand(CommandType, true, true, commandParams);

        // Assert
        Assert.AreEqual(CommandType, command.Type);
        Assert.AreEqual(CommandName, command.Name);
        CollectionAssert.AreEqual(commandParams, command.Params);
        Assert.IsTrue(command.WaitForResult);
        Assert.IsTrue(command.HasResult);
    }

    [TestMethod]
    public void ToByteArray_ShouldReturnCorrectByteArray()
    {
        // Arrange
        var commandString = _testTcpCommand.ToString();

        // Act
        var result = _testTcpCommand.ToByteArray(commandString);

        // Assert
        var expected = Encoding.ASCII.GetBytes(commandString);
        CollectionAssert.AreEqual(expected, result);
    }

    [TestMethod]
    public void ToByteArray_ShouldHandleNullCommand()
    {
        // Arrange
        var commandString = _testTcpCommand.ToString();

        // Act
        var result = _testTcpCommand.ToByteArray(null);

        // Assert
        var expected = Encoding.ASCII.GetBytes(commandString);
        CollectionAssert.AreEqual(expected, result);
    }

    [TestMethod]
    public void ToString_ShouldReturnCorrectString()
    {
        // Act
        var result = _testTcpCommand.ToString();

        // Assert
        var expected = $"{CommandName} param1 param2\r\n";
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void ToString_ShouldHandleEmptyParams()
    {
        // Arrange
        var command = new TestTcpCommand(CommandType);

        // Act
        var result = command.ToString();

        // Assert
        var expected = $"{CommandName}\r\n";
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void Subscribe_ShouldNotThrowException()
    {
        // Arrange
        var mockResponseProcessor = new Mock<IResponseProcessor>();

        // Act & Assert
        _testTcpCommand.Subscribe(mockResponseProcessor.Object);
    }

    [TestMethod]
    public void Unsubscribe_ShouldNotThrowException()
    {
        // Arrange
        var mockResponseProcessor = new Mock<IResponseProcessor>();

        // Act & Assert
        _testTcpCommand.Unsubscribe(mockResponseProcessor.Object);
    }

    [TestMethod]
    public void Result_ShouldReturnNullInitially()
    {
        // Assert
        Assert.IsNull(_testTcpCommand.Result);
    }

    [TestMethod]
    public void HasResult_ShouldBeSettableUsingReflection()
    {
        // Arrange
        var propertyInfo = typeof(BaseTcpCommand).GetProperty("HasResult",
            BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

        // Act
        Assert.IsNotNull(propertyInfo, nameof(propertyInfo) + " is null");

        propertyInfo.SetValue(_testTcpCommand, false);

        // Assert
        var result = (bool)propertyInfo.GetValue(_testTcpCommand);
        Assert.IsFalse(result);
    }

    public class TestTcpCommand : BaseTcpCommand
    {
        public TestTcpCommand(TraderCommandType type, bool waitForResult = false, bool hasResult = false,
            params string[] @params)
            : base(type, waitForResult, hasResult, @params)
        {
        }
    }
}