using DAS.Trader.IntegrationClient.Commands.TcpCommands;
using DAS.Trader.IntegrationClient.Common;
using DAS.Trader.IntegrationClient.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DAS.Trader.IntegrationClient.Tests.Commands.TcpCommands;

[TestClass]
public class CommandTests
{
    private const string Token = "testToken";
    private const string Symbol = "testSymbol";
    private const string Route = "testRoute";
    private const string Share = "testShare";
    private const string Price = "100.50";
    private const uint Display = 1;

    [TestMethod]
    public void HiddenOrderCommand_ShouldInitializePropertiesCorrectly()
    {
        // Arrange & Act
        var command = new HiddenOrderCommand(Token, OrderAction.Buy, Symbol, Route, Share, Price, Display);

        // Assert
        Assert.AreEqual(TraderCommandType.NEWORDER_COMMAND, command.Type);
        Assert.AreEqual("NEWORDER", command.Name);
        Assert.AreEqual(8, command.Params.Length); // Check if all parameters are included
        // Add more assertions to validate individual parameters if needed
    }

    [TestMethod]
    public void LimitOrderCommand_ShouldInitializePropertiesCorrectly()
    {
        // Arrange & Act
        var command = new LimitOrderCommand(Token, OrderAction.Buy, Symbol, Route, Share, Price);

        // Assert
        Assert.AreEqual(TraderCommandType.NEWORDER_COMMAND, command.Type);
        Assert.AreEqual("NEWORDER", command.Name);
        Assert.AreEqual(7, command.Params.Length); // Check if all parameters are included
        // Add more assertions to validate individual parameters if needed
    }

    [TestMethod]
    public void MarketOrderCommand_ShouldInitializePropertiesCorrectly()
    {
        // Arrange & Act
        var command = new MarketOrderCommand(Token, OrderAction.Buy, Symbol, Route, Share);

        // Assert
        Assert.AreEqual(TraderCommandType.NEWORDER_COMMAND, command.Type);
        Assert.AreEqual("NEWORDER", command.Name);
        Assert.AreEqual(7, command.Params.Length); // Check if all parameters are included
        // Add more assertions to validate individual parameters if needed
    }

    [TestMethod]
    public void CancelCommand_ShouldInitializePropertiesCorrectly()
    {
        // Arrange & Act
        var command = new CancelCommand(123);

        // Assert
        Assert.AreEqual(TraderCommandType.CANCEL_COMMAND, command.Type);
        Assert.AreEqual("CANCEL", command.Name);
        Assert.AreEqual(1, command.Params.Length); // Check if all parameters are included
        // Add more assertions to validate individual parameters if needed
    }

    [TestMethod]
    public void ClientCommand_ShouldInitializePropertiesCorrectly()
    {
        // Arrange & Act
        var command = ClientCommand.Instance;

        // Assert
        Assert.AreEqual(TraderCommandType.CLIENT_COMMAND, command.Type);
        Assert.AreEqual("CLIENT", command.Name);
        Assert.AreEqual(0, command.Params.Length); // Check if all parameters are included
        // Add more assertions to validate individual parameters if needed
    }

    [TestMethod]
    public void EchoCommand_ShouldInitializePropertiesCorrectly()
    {
        // Arrange & Act
        var command = EchoCommand.Instance;

        // Assert
        Assert.AreEqual(TraderCommandType.ECHO_COMMAND, command.Type);
        Assert.AreEqual("ECHO", command.Name);
        Assert.AreEqual(0, command.Params.Length); // Check if all parameters are included
        // Add more assertions to validate individual parameters if needed
    }

    [TestMethod]
    public void GetBuyingPowerCommand_ShouldInitializePropertiesCorrectly()
    {
        // Arrange & Act
        var command = GetBuyingPowerCommand.Instance;

        // Assert
        Assert.AreEqual(TraderCommandType.GET_BUYING_POWER_COMMAND, command.Type);
        Assert.AreEqual("GET BP", command.Name);
        Assert.AreEqual(0, command.Params.Length); // Check if all parameters are included
        // Add more assertions to validate individual parameters if needed
    }

    [TestMethod]
    public void GetShortInfoCommand_ShouldInitializePropertiesCorrectly()
    {
        // Arrange & Act
        var command = new GetShortInfoCommand(Symbol);

        // Assert
        Assert.AreEqual(TraderCommandType.GET_SHORTINFO_COMMAND, command.Type);
        Assert.AreEqual("GET SHORTINFO", command.Name);
        Assert.AreEqual(1, command.Params.Length); // Check if all parameters are included
        // Add more assertions to validate individual parameters if needed
    }

    [TestMethod]
    public void QuitCommand_ShouldInitializePropertiesCorrectly()
    {
        // Arrange & Act
        var command = QuitCommand.Instance;

        // Assert
        Assert.AreEqual(TraderCommandType.QUIT_COMMAND, command.Type);
        Assert.AreEqual("QUIT", command.Name);
        Assert.AreEqual(0, command.Params.Length); // Check if all parameters are included
        // Add more assertions to validate individual parameters if needed
    }

    [TestMethod]
    public void PosrefreshCommand_ToString_ShouldReturnCorrectString()
    {
        // Arrange & Act
        var command = PosrefreshCommand.Instance;

        // Assert
        Assert.AreEqual(TraderCommandType.POSREFRESH_COMMAND, command.Type);
        Assert.AreEqual("POSREFRESH", command.Name);
        Assert.AreEqual(0, command.Params.Length); // Check if all parameters are included
        // Add more assertions to validate individual parameters if needed
    }

    [TestMethod]
    public void UnSubscribeTimeSalesCommand_Instance_ShouldReturnCorrectValues()
    {
        // Arrange & Act
        var command = new UnSubscribeTimeSalesCommand("AAPL");

        // Assert
        Assert.AreEqual(TraderCommandType.UNSB_COMMAND, command.Type);
        Assert.AreEqual("UNSB", command.Name);
        CollectionAssert.AreEqual(new[] { "AAPL", "tms" }, command.Params);
        // Add more assertions to validate individual parameters if needed
    }

    [TestMethod]
    public void UnSubscribeMinChartCommand_Instance_ShouldReturnCorrectValues()
    {
        // Arrange & Act
        var command = new UnSubscribeMinChartCommand("AAPL");

        // Assert
        Assert.AreEqual(TraderCommandType.UNSB_COMMAND, command.Type);
        Assert.AreEqual("UNSB", command.Name);
        CollectionAssert.AreEqual(new[] { "AAPL", "MINCHART" }, command.Params);
        // Add more assertions to validate individual parameters if needed
    }

    [TestMethod]
    public void UnSubscribeLevel2Command_Instance_ShouldReturnCorrectValues()
    {
        // Arrange & Act
        var command = new UnSubscribeLevel2Command("AAPL");

        // Assert
        Assert.AreEqual(TraderCommandType.UNSB_COMMAND, command.Type);
        Assert.AreEqual("UNSB", command.Name);
        CollectionAssert.AreEqual(new[] { "AAPL", "Lv2" }, command.Params);
        // Add more assertions to validate individual parameters if needed
    }

    [TestMethod]
    public void UnSubscribeLevel1Command_Instance_ShouldReturnCorrectValues()
    {
        // Arrange & Act
        var command = new UnSubscribeLevel1Command("AAPL");

        // Assert
        Assert.AreEqual(TraderCommandType.UNSB_COMMAND, command.Type);
        Assert.AreEqual("UNSB", command.Name);
        CollectionAssert.AreEqual(new[] { "AAPL", "Lv1" }, command.Params);
        // Add more assertions to validate individual parameters if needed
    }

    [TestMethod]
    public void UnSubscribeDayChartCommand_Instance_ShouldReturnCorrectValues()
    {
        // Arrange & Act
        var command = new UnSubscribeDayChartCommand("AAPL");

        // Assert
        Assert.AreEqual(TraderCommandType.UNSB_COMMAND, command.Type);
        Assert.AreEqual("UNSB", command.Name);
        CollectionAssert.AreEqual(new[] { "AAPL", "DAYCHART" }, command.Params);
        // Add more assertions to validate individual parameters if needed
    }

    [TestMethod]
    public void SubscribeTimeSalesCommand_Instance_ShouldReturnCorrectValues()
    {
        // Arrange & Act
        var command = new SubscribeTimeSalesCommand("AAPL");

        // Assert
        Assert.AreEqual(TraderCommandType.SB_COMMAND, command.Type);
        Assert.AreEqual("SB", command.Name);
        CollectionAssert.AreEqual(new[] { "AAPL", "tms" }, command.Params);
        // Add more assertions to validate individual parameters if needed
    }

    [TestMethod]
    public void SubscribeMinChartCommand_Instance_ShouldReturnCorrectValues()
    {
        // Arrange
        var symbol = "AAPL";
        var startDate = new DateTime(2024, 6, 6, 9, 30, 0);
        var endDate = new DateTime(2024, 6, 6, 16, 0, 0);
        var minType = 5;

        // Act
        var command = new SubscribeMinChartCommand(symbol, startDate, endDate, minType);

        // Assert
        Assert.AreEqual(TraderCommandType.SB_COMMAND, command.Type);
        Assert.AreEqual("SB", command.Name);
        CollectionAssert.AreEqual(new[]
        {
            "AAPL",
            "MINCHART",
            "2024/06/06-09:30",
            "2024/06/06-16:00",
            "5"
        }, command.Params);
    }

    [TestMethod]
    public void SubscribeLevelCommand_Instance_ShouldReturnCorrectValues()
    {
        // Arrange
        var symbol = "AAPL";
        var level = TraderLevels.REGIONAL_LEVEL2;

        // Act
        var command = new SubscribeLevelCommand(symbol, level);

        // Assert
        Assert.AreEqual(TraderCommandType.SB_COMMAND, command.Type);
        Assert.AreEqual("SB", command.Name);
        CollectionAssert.AreEqual(new[]
        {
            "AAPL",
            level.GetDescription()
        }, command.Params);
    }

    [TestMethod]
    public void SubscribeDayChartCommand_Instance_ShouldReturnCorrectValues()
    {
        // Arrange
        var symbol = "AAPL";
        var startDate = new DateTime(2024, 6, 1);
        var endDate = new DateTime(2024, 6, 5);

        // Act
        var command = new SubscribeDayChartCommand(symbol, startDate, endDate);

        // Assert
        Assert.AreEqual(TraderCommandType.SB_COMMAND, command.Type);
        Assert.AreEqual("SB", command.Name);
        CollectionAssert.AreEqual(new[]
        {
            "AAPL",
            "DAYCHART",
            startDate.ToString("yyyy/MM/dd"),
            endDate.ToString("yyyy/MM/dd")
        }, command.Params);
    }

    [TestMethod]
    public void StopTrailingOrderCommand_Instance_ShouldReturnCorrectValues()
    {
        // Arrange
        var token = "123456";
        var action = OrderAction.Sell;
        var symbol = "AAPL";
        var route = "SMAT";
        var share = "100";
        var trailPrice = "0.2";

        // Act
        var command = new StopTrailingOrderCommand(token, action, symbol, route, share, trailPrice);

        // Assert
        Assert.AreEqual(TraderCommandType.NEWORDER_COMMAND, command.Type);
        Assert.AreEqual("NEWORDER", command.Name);
        CollectionAssert.AreEqual(new[]
        {
            token,
            action.GetDescription(),
            symbol,
            route,
            share,
            "STOPTRAILING",
            trailPrice
        }, command.Params);
    }

    [TestMethod]
    public void StopRangeOrderCommand_Instance_ShouldReturnCorrectValues()
    {
        // Arrange
        var token = "123456";
        var action = OrderAction.Buy;
        var symbol = "AAPL";
        var route = "SMAT";
        var share = "100";
        var isStopRangeMarket = false;
        var lowPrice = "210.2";
        var highPrice = "210.6";

        // Act
        var command =
            new StopRangeOrderCommand(token, action, symbol, route, share, isStopRangeMarket, lowPrice, highPrice);

        // Assert
        Assert.AreEqual(TraderCommandType.NEWORDER_COMMAND, command.Type);
        Assert.AreEqual("NEWORDER", command.Name);
        CollectionAssert.AreEqual(new[]
        {
            token,
            action.GetDescription(),
            symbol,
            route,
            share,
            "STOPRANGE",
            lowPrice,
            highPrice
        }, command.Params);
    }

    [TestMethod]
    public void StopRangeOrderCommand_Instance_WithStopRangeMarket_ShouldReturnCorrectValues()
    {
        // Arrange
        var token = "123456";
        var action = OrderAction.Buy;
        var symbol = "AAPL";
        var route = "SMAT";
        var share = "100";
        var isStopRangeMarket = true;
        var lowPrice = "210.2";
        var highPrice = "210.6";

        // Act
        var command =
            new StopRangeOrderCommand(token, action, symbol, route, share, isStopRangeMarket, lowPrice, highPrice);

        // Assert
        Assert.AreEqual(TraderCommandType.NEWORDER_COMMAND, command.Type);
        Assert.AreEqual("NEWORDER", command.Name);
        CollectionAssert.AreEqual(new[]
        {
            token,
            action.GetDescription(),
            symbol,
            route,
            share,
            "STOPRANGEMKT",
            lowPrice,
            highPrice
        }, command.Params);
    }

    [TestMethod]
    public void StopMarketOrderCommand_Instance_ShouldReturnCorrectValues()
    {
        // Arrange
        var token = "123456";
        var action = OrderAction.Sell;
        var symbol = "AAPL";
        var route = "SMAT";
        var share = "100";
        var stopPrice = "210.5";
        var timeInForce = TimeInForce.Day;

        // Act
        var command = new StopMarketOrderCommand(token, action, symbol, route, share, stopPrice, timeInForce);

        // Assert
        Assert.AreEqual(TraderCommandType.NEWORDER_COMMAND, command.Type);
        Assert.AreEqual("NEWORDER", command.Name);
        CollectionAssert.AreEqual(new[]
        {
            token,
            action.GetDescription(),
            symbol,
            route,
            share,
            "STOPMKT",
            stopPrice,
            timeInForce.GetDescription()
        }, command.Params);
    }

    [TestMethod]
    public void StopLimitOrderCommand_Instance_ShouldReturnCorrectValues()
    {
        // Arrange
        var token = "123456";
        var action = OrderAction.Buy;
        var symbol = "AAPL";
        var route = "SMAT";
        var share = "100";
        var stopPrice = "210.5";
        var price = "210.8";
        var timeInForce = TimeInForce.Day;

        // Act
        var command = new StopLimitOrderCommand(token, action, symbol, route, share, stopPrice, price, timeInForce);

        // Assert
        Assert.AreEqual(TraderCommandType.NEWORDER_COMMAND, command.Type);
        Assert.AreEqual("NEWORDER", command.Name);
        CollectionAssert.AreEqual(new[]
        {
            token,
            action.GetDescription(),
            symbol,
            route,
            share,
            "STOPLMT",
            stopPrice,
            price,
            timeInForce.GetDescription()
        }, command.Params);
    }

    [TestMethod]
    public void SlPriceInquireCommand_Instance_ShouldReturnCorrectValues()
    {
        // Arrange
        var symbol = "AAPL";
        var locateShares = 100u;
        var route = "ALLROUTE";

        // Act
        var command = new SlPriceInquireCommand(symbol, locateShares, route);

        // Assert
        Assert.AreEqual(TraderCommandType.SLPRICEINQUIRE_COMMAND, command.Type);
        Assert.AreEqual("SLPRICEINQUIRE", command.Name);
        CollectionAssert.AreEqual(new[] { symbol, locateShares.ToString(), route }, command.Params);
    }

    [TestMethod]
    public void SlOfferOperationRejectCommand_Instance_ShouldReturnCorrectValues()
    {
        // Arrange
        var locateOrderId = "12345";

        // Act
        var command = new SlOfferOperationRejectCommand(locateOrderId);

        // Assert
        Assert.AreEqual(TraderCommandType.SLOFFEROPERATION_COMMAND, command.Type);
        Assert.AreEqual("SLOFFEROPERATION", command.Name);
        CollectionAssert.AreEqual(new[] { locateOrderId, "Reject" }, command.Params);
    }

    [TestMethod]
    public void SlOfferOperationAcceptCommand_Instance_ShouldReturnCorrectValues()
    {
        // Arrange
        var locateOrderId = "12345";

        // Act
        var command = new SlOfferOperationAcceptCommand(locateOrderId);

        // Assert
        Assert.AreEqual(TraderCommandType.SLOFFEROPERATION_COMMAND, command.Type);
        Assert.AreEqual("SLOFFEROPERATION", command.Name);
        CollectionAssert.AreEqual(new[] { locateOrderId, "Accept" }, command.Params);
    }

    [TestMethod]
    public void SlNewOrderCommand_Instance_ShouldReturnCorrectValues()
    {
        // Arrange
        var symbol = "AAPL";
        var locateShares = 100;
        var route = "ALLROUTE";

        // Act
        var command = new SlNewOrderCommand(symbol, (uint)locateShares, route);

        // Assert
        Assert.AreEqual(TraderCommandType.SLNEWORDER_COMMAND, command.Type);
        Assert.AreEqual("SLNEWORDER", command.Name);
        CollectionAssert.AreEqual(new[] { symbol, locateShares.ToString(), route }, command.Params);
    }

    [TestMethod]
    public void SlCancelOrderCommand_Instance_ShouldReturnCorrectValues()
    {
        // Arrange
        var locateOrderId = "12345";

        // Act
        var command = new SlCancelOrderCommand(locateOrderId);

        // Assert
        Assert.AreEqual(TraderCommandType.SLCANCELORDER_COMMAND, command.Type);
        Assert.AreEqual("SLCANCELORDER", command.Name);
        CollectionAssert.AreEqual(new[] { locateOrderId }, command.Params);
    }

    [TestMethod]
    public void PegOrderCommand_Instance_ShouldReturnCorrectValues()
    {
        // Arrange
        var token = "token";
        var action = OrderAction.Buy;
        var symbol = "MSFT";
        var route = "INET";
        var share = "100";
        var orderOptions = OrderOptions.MID;
        var price = "200.5";
        var timeInForce = TimeInForce.GTC;

        // Act
        var command = new PegOrderCommand(token, action, symbol, route, share, orderOptions, price, timeInForce);

        // Assert
        Assert.AreEqual(TraderCommandType.NEWORDER_COMMAND, command.Type);
        Assert.AreEqual("NEWORDER", command.Name);
        CollectionAssert.AreEqual(
            new[]
            {
                token, "B", symbol, route, share, "PEG", orderOptions.GetDescription(), price,
                timeInForce.GetDescription()
            }, command.Params);
    }
}