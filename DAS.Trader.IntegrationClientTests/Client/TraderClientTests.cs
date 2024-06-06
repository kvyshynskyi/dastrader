using System.Net;
using DAS.Trader.IntegrationClient.Client;
using DAS.Trader.IntegrationClient.Interfaces;
using DAS.Trader.IntegrationClient.Response;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DAS.Trader.IntegrationClient.Tests.Client;

[TestClass]
public class TraderClientTests
{
    private Mock<IResponseProcessor> _mockResponseProcessor;
    private Mock<ITcpClient> _mockTcpClient;
    private TraderClient _traderClient;

    [TestInitialize]
    public void Setup()
    {
        _mockTcpClient = new Mock<ITcpClient>();
        _mockResponseProcessor = new Mock<IResponseProcessor>();
        _traderClient = new TraderClient("127.0.0.1", 8080, tcpClient: _mockTcpClient.Object,
            responseProcessor: _mockResponseProcessor.Object);
    }

    [TestMethod]
    public async Task ConnectAsyncTest()
    {
        // Arrange
        _mockTcpClient.Setup(client => client.ConnectAsync(It.IsAny<IPAddress>(), It.IsAny<int>()))
            .Returns(Task.CompletedTask);

        // Act
        await _traderClient.ConnectAsync();

        // Assert
        _mockTcpClient.Verify(client => client.ConnectAsync(It.IsAny<IPAddress>(), It.IsAny<int>()), Times.Once);

        // Add assertion to ensure the listening process is started
        await Task.Delay(100); // Adjust delay as needed
        _mockResponseProcessor.Verify(processor => processor.ListenAsync(It.IsAny<INetworkStream>()), Times.Once);
    }

    [TestMethod]
    public async Task SendCommandAsyncTest()
    {
        // Arrange
        var mockCommand = new Mock<ITcpCommand>();
        mockCommand.Setup(cmd => cmd.ToString()).Returns("TEST COMMAND");
        mockCommand.Setup(cmd => cmd.ToByteArray(It.IsAny<string>())).Returns(new byte[0]);
        mockCommand.Setup(cmd => cmd.WaitForResult).Returns(false);

        var mockStream = new Mock<INetworkStream>();

        _mockTcpClient.Setup(client => client.GetStream())
            .Returns(mockStream.Object);

        // Act
        var result = await _traderClient.SendCommandAsync(mockCommand.Object);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Success);
        _mockTcpClient.Verify(client => client.GetStream(), Times.Once);

        // Add assertion to ensure the listening process is started
        await Task.Delay(100); // Adjust delay as needed
        mockStream.Verify(stream => stream.WriteAsync(It.IsAny<byte[]>(), 0, It.IsAny<int>()), Times.Once);
    }

    [TestMethod]
    public async Task SendCommandAsyncTest_Results()
    {
        // Arrange
        var mockCommand = new Mock<ITcpCommand>();
        mockCommand.Setup(cmd => cmd.ToString()).Returns("TEST COMMAND");
        mockCommand.Setup(cmd => cmd.ToByteArray(It.IsAny<string>())).Returns(new byte[0]);
        mockCommand.Setup(cmd => cmd.WaitForResult).Returns(true);
        mockCommand.Setup(cmd => cmd.HasResult).Returns(true);

        var mockStream = new Mock<INetworkStream>();

        _mockTcpClient.Setup(client => client.GetStream())
            .Returns(mockStream.Object);

        // Act
        var result = await _traderClient.SendCommandAsync(mockCommand.Object);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Success);
        _mockTcpClient.Verify(client => client.GetStream(), Times.Once);

        // Add assertion to ensure the listening process is started
        await Task.Delay(100); // Adjust delay as needed
        mockStream.Verify(stream => stream.WriteAsync(It.IsAny<byte[]>(), 0, It.IsAny<int>()), Times.Once);
    }


    [TestMethod]
    public void DisposeTest()
    {
        // Arrange
        var mockStream = new Mock<INetworkStream>();

        _mockTcpClient.Setup(client => client.GetStream())
            .Returns(mockStream.Object);
        _traderClient.GetStream(); // This initializes the _currentStream

        // Act
        _traderClient.Dispose();

        // Assert
        _mockTcpClient.Verify(client => client.Close(), Times.Once);
        mockStream.Verify(stream => stream.Close(), Times.Once);
    }

    [TestMethod]
    public async Task ConnectAsync_Failure()
    {
        // Arrange
        _mockTcpClient.Setup(client => client.ConnectAsync(It.IsAny<IPAddress>(), It.IsAny<int>()))
            .ThrowsAsync(new Exception("Connection failed"));

        // Act & Assert
        await Assert.ThrowsExceptionAsync<Exception>(() => _traderClient.ConnectAsync());
    }


    [TestMethod]
    public async Task SendCommandAsync_Timeout()
    {
        // Arrange
        var expectedMessage = "Timeout occurred. Command TEST COMMAND";
        var mockCommand = new Mock<ITcpCommand>();
        mockCommand.Setup(cmd => cmd.Name).Returns("TEST COMMAND");
        mockCommand.Setup(cmd => cmd.ToByteArray(It.IsAny<string>())).Returns(new byte[0]);
        mockCommand.Setup(cmd => cmd.WaitForResult).Returns(true);
        mockCommand.Setup(cmd => cmd.HasResult).Returns(false);

        var mockStream = new Mock<INetworkStream>();

        _mockTcpClient.Setup(client => client.GetStream())
            .Returns(mockStream.Object);

        // Act
        var result = await _traderClient.SendCommandAsync(mockCommand.Object);

        // Assert
        Assert.IsFalse(result.Success);
        Assert.AreEqual(expectedMessage, result.Message);
    }

    [TestMethod]
    public void Events_Subscribed_Unsubscribed()
    {
        // Arrange
        var mockEventHandler = new Mock<EventHandler<ResponseEventArgs>>();

        // Act
        _traderClient.PriceInquiry += mockEventHandler.Object;
        _traderClient.PriceInquiry -= mockEventHandler.Object;

        _traderClient.LoginResponse += mockEventHandler.Object;
        _traderClient.LoginResponse -= mockEventHandler.Object;

        _traderClient.SlOrder += mockEventHandler.Object;
        _traderClient.SlOrder -= mockEventHandler.Object;

        _traderClient.BuyingPower += mockEventHandler.Object;
        _traderClient.BuyingPower -= mockEventHandler.Object;

        _traderClient.ChartData += mockEventHandler.Object;
        _traderClient.ChartData -= mockEventHandler.Object;

        _traderClient.ClientCount += mockEventHandler.Object;
        _traderClient.ClientCount -= mockEventHandler.Object;

        _traderClient.IOrder += mockEventHandler.Object;
        _traderClient.IOrder -= mockEventHandler.Object;

        _traderClient.IPosition += mockEventHandler.Object;
        _traderClient.IPosition -= mockEventHandler.Object;

        _traderClient.ITrade += mockEventHandler.Object;
        _traderClient.ITrade -= mockEventHandler.Object;

        _traderClient.Level2Quote += mockEventHandler.Object;
        _traderClient.Level2Quote -= mockEventHandler.Object;

        _traderClient.LimitPrice += mockEventHandler.Object;
        _traderClient.LimitPrice -= mockEventHandler.Object;

        _traderClient.Trade += mockEventHandler.Object;
        _traderClient.Trade -= mockEventHandler.Object;

        _traderClient.Order += mockEventHandler.Object;
        _traderClient.Order -= mockEventHandler.Object;

        _traderClient.OrderAction += mockEventHandler.Object;
        _traderClient.OrderAction -= mockEventHandler.Object;

        _traderClient.OrderBegin += mockEventHandler.Object;
        _traderClient.OrderBegin -= mockEventHandler.Object;

        _traderClient.OrderEnd += mockEventHandler.Object;
        _traderClient.OrderEnd -= mockEventHandler.Object;

        _traderClient.SlOrderEnd += mockEventHandler.Object;
        _traderClient.SlOrderEnd -= mockEventHandler.Object;

        _traderClient.SlOrderBegin += mockEventHandler.Object;
        _traderClient.SlOrderBegin -= mockEventHandler.Object;

        _traderClient.PositionBegin += mockEventHandler.Object;
        _traderClient.PositionBegin -= mockEventHandler.Object;

        _traderClient.PositionEnd += mockEventHandler.Object;
        _traderClient.PositionEnd -= mockEventHandler.Object;

        _traderClient.Position += mockEventHandler.Object;
        _traderClient.Position -= mockEventHandler.Object;

        _traderClient.TradeBegin += mockEventHandler.Object;
        _traderClient.TradeBegin -= mockEventHandler.Object;

        _traderClient.TradeEnd += mockEventHandler.Object;
        _traderClient.TradeEnd -= mockEventHandler.Object;

        _traderClient.ShortInfo += mockEventHandler.Object;
        _traderClient.ShortInfo -= mockEventHandler.Object;

        _traderClient.Quote += mockEventHandler.Object;
        _traderClient.Quote -= mockEventHandler.Object;

        _traderClient.TimeSalesQuote += mockEventHandler.Object;
        _traderClient.TimeSalesQuote -= mockEventHandler.Object;

        // Assert
        _mockResponseProcessor.VerifyAdd(
            processor => processor.PriceInquiry += It.IsAny<EventHandler<ResponseEventArgs>>(), Times.Once);
        _mockResponseProcessor.VerifyRemove(
            processor => processor.PriceInquiry -= It.IsAny<EventHandler<ResponseEventArgs>>(), Times.Once);

        _mockResponseProcessor.VerifyAdd(
            processor => processor.LoginResponse += It.IsAny<EventHandler<ResponseEventArgs>>(), Times.Once);
        _mockResponseProcessor.VerifyRemove(
            processor => processor.LoginResponse -= It.IsAny<EventHandler<ResponseEventArgs>>(), Times.Once);

        _mockResponseProcessor.VerifyAdd(processor => processor.SlOrder += It.IsAny<EventHandler<ResponseEventArgs>>(),
            Times.Once);
        _mockResponseProcessor.VerifyRemove(
            processor => processor.SlOrder -= It.IsAny<EventHandler<ResponseEventArgs>>(), Times.Once);

        _mockResponseProcessor.VerifyAdd(
            processor => processor.BuyingPower += It.IsAny<EventHandler<ResponseEventArgs>>(), Times.Once);
        _mockResponseProcessor.VerifyRemove(
            processor => processor.BuyingPower -= It.IsAny<EventHandler<ResponseEventArgs>>(), Times.Once);

        _mockResponseProcessor.VerifyAdd(
            processor => processor.ChartData += It.IsAny<EventHandler<ResponseEventArgs>>(), Times.Once);
        _mockResponseProcessor.VerifyRemove(
            processor => processor.ChartData -= It.IsAny<EventHandler<ResponseEventArgs>>(), Times.Once);

        _mockResponseProcessor.VerifyAdd(
            processor => processor.ClientCount += It.IsAny<EventHandler<ResponseEventArgs>>(), Times.Once);
        _mockResponseProcessor.VerifyRemove(
            processor => processor.ClientCount -= It.IsAny<EventHandler<ResponseEventArgs>>(), Times.Once);

        _mockResponseProcessor.VerifyAdd(processor => processor.IOrder += It.IsAny<EventHandler<ResponseEventArgs>>(),
            Times.Once);
        _mockResponseProcessor.VerifyRemove(
            processor => processor.IOrder -= It.IsAny<EventHandler<ResponseEventArgs>>(), Times.Once);

        _mockResponseProcessor.VerifyAdd(
            processor => processor.IPosition += It.IsAny<EventHandler<ResponseEventArgs>>(), Times.Once);
        _mockResponseProcessor.VerifyRemove(
            processor => processor.IPosition -= It.IsAny<EventHandler<ResponseEventArgs>>(), Times.Once);

        _mockResponseProcessor.VerifyAdd(processor => processor.ITrade += It.IsAny<EventHandler<ResponseEventArgs>>(),
            Times.Once);
        _mockResponseProcessor.VerifyRemove(
            processor => processor.ITrade -= It.IsAny<EventHandler<ResponseEventArgs>>(), Times.Once);

        _mockResponseProcessor.VerifyAdd(
            processor => processor.Level2Quote += It.IsAny<EventHandler<ResponseEventArgs>>(), Times.Once);
        _mockResponseProcessor.VerifyRemove(
            processor => processor.Level2Quote -= It.IsAny<EventHandler<ResponseEventArgs>>(), Times.Once);

        _mockResponseProcessor.VerifyAdd(
            processor => processor.LimitPrice += It.IsAny<EventHandler<ResponseEventArgs>>(), Times.Once);
        _mockResponseProcessor.VerifyRemove(
            processor => processor.LimitPrice -= It.IsAny<EventHandler<ResponseEventArgs>>(), Times.Once);

        _mockResponseProcessor.VerifyAdd(processor => processor.Trade += It.IsAny<EventHandler<ResponseEventArgs>>(),
            Times.Once);
        _mockResponseProcessor.VerifyRemove(processor => processor.Trade -= It.IsAny<EventHandler<ResponseEventArgs>>(),
            Times.Once);

        _mockResponseProcessor.VerifyAdd(processor => processor.Order += It.IsAny<EventHandler<ResponseEventArgs>>(),
            Times.Once);
        _mockResponseProcessor.VerifyRemove(processor => processor.Order -= It.IsAny<EventHandler<ResponseEventArgs>>(),
            Times.Once);

        _mockResponseProcessor.VerifyAdd(
            processor => processor.OrderAction += It.IsAny<EventHandler<ResponseEventArgs>>(), Times.Once);
        _mockResponseProcessor.VerifyRemove(
            processor => processor.OrderAction -= It.IsAny<EventHandler<ResponseEventArgs>>(), Times.Once);

        _mockResponseProcessor.VerifyAdd(
            processor => processor.OrderBegin += It.IsAny<EventHandler<ResponseEventArgs>>(), Times.Once);
        _mockResponseProcessor.VerifyRemove(
            processor => processor.OrderBegin -= It.IsAny<EventHandler<ResponseEventArgs>>(), Times.Once);

        _mockResponseProcessor.VerifyAdd(processor => processor.OrderEnd += It.IsAny<EventHandler<ResponseEventArgs>>(),
            Times.Once);
        _mockResponseProcessor.VerifyRemove(
            processor => processor.OrderEnd -= It.IsAny<EventHandler<ResponseEventArgs>>(), Times.Once);

        _mockResponseProcessor.VerifyAdd(
            processor => processor.SlOrderEnd += It.IsAny<EventHandler<ResponseEventArgs>>(), Times.Once);
        _mockResponseProcessor.VerifyRemove(
            processor => processor.SlOrderEnd -= It.IsAny<EventHandler<ResponseEventArgs>>(), Times.Once);

        _mockResponseProcessor.VerifyAdd(
            processor => processor.SlOrderBegin += It.IsAny<EventHandler<ResponseEventArgs>>(), Times.Once);
        _mockResponseProcessor.VerifyRemove(
            processor => processor.SlOrderBegin -= It.IsAny<EventHandler<ResponseEventArgs>>(), Times.Once);

        _mockResponseProcessor.VerifyAdd(processor => processor.Position += It.IsAny<EventHandler<ResponseEventArgs>>(),
            Times.Once);
        _mockResponseProcessor.VerifyRemove(
            processor => processor.Position -= It.IsAny<EventHandler<ResponseEventArgs>>(), Times.Once);

        _mockResponseProcessor.VerifyAdd(
            processor => processor.PositionBegin += It.IsAny<EventHandler<ResponseEventArgs>>(), Times.Once);
        _mockResponseProcessor.VerifyRemove(
            processor => processor.PositionBegin -= It.IsAny<EventHandler<ResponseEventArgs>>(), Times.Once);

        _mockResponseProcessor.VerifyAdd(
            processor => processor.PositionEnd += It.IsAny<EventHandler<ResponseEventArgs>>(), Times.Once);
        _mockResponseProcessor.VerifyRemove(
            processor => processor.PositionEnd -= It.IsAny<EventHandler<ResponseEventArgs>>(), Times.Once);

        _mockResponseProcessor.VerifyAdd(
            processor => processor.TradeBegin += It.IsAny<EventHandler<ResponseEventArgs>>(), Times.Once);
        _mockResponseProcessor.VerifyRemove(
            processor => processor.TradeBegin -= It.IsAny<EventHandler<ResponseEventArgs>>(), Times.Once);

        _mockResponseProcessor.VerifyAdd(processor => processor.TradeEnd += It.IsAny<EventHandler<ResponseEventArgs>>(),
            Times.Once);
        _mockResponseProcessor.VerifyRemove(
            processor => processor.TradeEnd -= It.IsAny<EventHandler<ResponseEventArgs>>(), Times.Once);

        _mockResponseProcessor.VerifyAdd(
            processor => processor.ShortInfo += It.IsAny<EventHandler<ResponseEventArgs>>(), Times.Once);
        _mockResponseProcessor.VerifyRemove(
            processor => processor.ShortInfo -= It.IsAny<EventHandler<ResponseEventArgs>>(), Times.Once);

        _mockResponseProcessor.VerifyAdd(processor => processor.Quote += It.IsAny<EventHandler<ResponseEventArgs>>(),
            Times.Once);
        _mockResponseProcessor.VerifyRemove(processor => processor.Quote -= It.IsAny<EventHandler<ResponseEventArgs>>(),
            Times.Once);

        _mockResponseProcessor.VerifyAdd(
            processor => processor.TimeSalesQuote += It.IsAny<EventHandler<ResponseEventArgs>>(), Times.Once);
        _mockResponseProcessor.VerifyRemove(
            processor => processor.TimeSalesQuote -= It.IsAny<EventHandler<ResponseEventArgs>>(), Times.Once);
    }
}