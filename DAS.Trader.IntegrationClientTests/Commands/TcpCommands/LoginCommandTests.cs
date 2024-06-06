using System.Reflection;
using DAS.Trader.IntegrationClient.Commands.TcpCommands;
using DAS.Trader.IntegrationClient.Enums;
using DAS.Trader.IntegrationClient.Interfaces;
using DAS.Trader.IntegrationClient.Response;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DAS.Trader.IntegrationClient.Tests.Commands.TcpCommands;

[TestClass]
public class LoginCommandTests
{
    private const string Login = "testLogin";
    private const string Password = "testPassword";
    private const string Account = "testAccount";
    private LoginCommand _loginCommand;

    [TestInitialize]
    public void Setup()
    {
        _loginCommand = new LoginCommand(Login, Password, Account);
    }

    [TestMethod]
    public void Constructor_ShouldInitializeProperties()
    {
        // Arrange & Act
        var command = new LoginCommand(Login, Password, Account);

        // Assert
        Assert.AreEqual(TraderCommandType.LOGIN_COMMAND, command.Type);
        Assert.AreEqual("LOGIN", command.Name);
        CollectionAssert.AreEqual(new[] { Login, Password, Account }, command.Params);
        Assert.IsTrue(command.WaitForResult);
        Assert.IsFalse(command.HasResult);
    }

    [TestMethod]
    public void Subscribe_ShouldAddEventHandler()
    {
        // Arrange
        var mockResponseProcessor = new Mock<IResponseProcessor>();

        // Act
        _loginCommand.Subscribe(mockResponseProcessor.Object);

        // Assert
        mockResponseProcessor.VerifyAdd(rp => rp.LoginResponse += It.IsAny<EventHandler<ResponseEventArgs>>(),
            Times.Once);
    }

    [TestMethod]
    public void Unsubscribe_ShouldRemoveEventHandler()
    {
        // Arrange
        var mockResponseProcessor = new Mock<IResponseProcessor>();

        // Act
        _loginCommand.Unsubscribe(mockResponseProcessor.Object);

        // Assert
        mockResponseProcessor.VerifyRemove(rp => rp.LoginResponse -= It.IsAny<EventHandler<ResponseEventArgs>>(),
            Times.Once);
    }

    [TestMethod]
    public void ResponseProcessor_LoginResponse_ShouldSetResultAndHasResult()
    {
        // Arrange
        var responseEventArgs =
            new ResponseEventArgs(Guid.NewGuid(), TraderCommandType.LOGIN_COMMAND, null, "result");

        // Act
        typeof(LoginCommand).GetMethod("ResponseProcessor_LoginResponse",
                BindingFlags.NonPublic | BindingFlags.Instance)
            ?.Invoke(_loginCommand, new object[] { null, responseEventArgs });

        // Assert
        Assert.AreEqual("result", _loginCommand.Result);
        Assert.IsTrue((bool)typeof(LoginCommand)
            .GetProperty("HasResult", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
            ?.GetValue(_loginCommand));
    }

    [TestMethod]
    public void ResponseProcessor_LoginResponse_ShouldSetResultToMessageIfParametersNull()
    {
        // Arrange
        var responseEventArgs = new ResponseEventArgs(Guid.NewGuid(), TraderCommandType.LOGIN_COMMAND, "message");

        // Act
        typeof(LoginCommand).GetMethod("ResponseProcessor_LoginResponse",
                BindingFlags.NonPublic | BindingFlags.Instance)
            ?.Invoke(_loginCommand, new object[] { null, responseEventArgs });

        // Assert
        Assert.AreEqual("message", _loginCommand.Result);
        Assert.IsTrue((bool)typeof(LoginCommand)
            .GetProperty("HasResult", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
            ?.GetValue(_loginCommand));
    }

    [TestMethod]
    public void ResponseProcessor_LoginResponse_ShouldSetResultToEmptyStringIfParametersAreInvalid()
    {
        // Arrange
        var responseEventArgs = new ResponseEventArgs(Guid.NewGuid(), TraderCommandType.LOGIN_COMMAND);

        // Act
        typeof(LoginCommand).GetMethod("ResponseProcessor_LoginResponse",
                BindingFlags.NonPublic | BindingFlags.Instance)
            ?.Invoke(_loginCommand, new object[] { null, responseEventArgs });

        // Assert
        Assert.AreEqual(string.Empty, _loginCommand.Result);
        Assert.IsTrue((bool)typeof(LoginCommand)
            .GetProperty("HasResult", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
            ?.GetValue(_loginCommand));
    }
}