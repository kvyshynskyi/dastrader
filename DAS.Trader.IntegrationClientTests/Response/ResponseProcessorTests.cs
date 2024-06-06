using System.Collections.Concurrent;
using System.ComponentModel;
using System.Reflection;
using System.Text;
using DAS.Trader.IntegrationClient.Common;
using DAS.Trader.IntegrationClient.Enums;
using DAS.Trader.IntegrationClient.Interfaces;
using DAS.Trader.IntegrationClient.Response;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DAS.Trader.IntegrationClient.Tests.Response;

[TestClass]
public class ResponseProcessorTests
{
    private static readonly Dictionary<string, TraderCommandType> EventEnumDictionary = new()
    {
        { "LoginResponse", TraderCommandType.LOGIN_RESPONSE },
        { "PriceInquiry", TraderCommandType.SLRET_RESPONSE },
        { "SlOrderBegin", TraderCommandType.SLORDER_BEGIN_RESPONSE },
        { "SlOrderEnd", TraderCommandType.SLORDER_END_RESPONSE },
        { "SlOrder", TraderCommandType.SLORDER_RESPONSE },
        { "OrderBegin", TraderCommandType.ORDER_BEGIN_RESPONSE },
        { "OrderEnd", TraderCommandType.ORDER_END_RESPONSE },
        { "Order", TraderCommandType.ORDER_RESPONSE },
        { "PositionBegin", TraderCommandType.POS_BEGIN_RESPONSE },
        { "PositionEnd", TraderCommandType.POS_END_RESPONSE },
        { "Position", TraderCommandType.POS_RESPONSE },
        { "TradeBegin", TraderCommandType.TRADE_BEGIN_RESPONSE },
        { "TradeEnd", TraderCommandType.TRADE_END_RESPONSE },
        { "Trade", TraderCommandType.TRADE_RESPONSE },
        { "OrderAction", TraderCommandType.ORDER_ACTION_MESSAGE_RESPONSE },
        { "BuyingPower", TraderCommandType.BUYING_POWER_RESPONSE },
        { "ClientCount", TraderCommandType.CLIENT_RESPONSE },
        { "ShortInfo", TraderCommandType.SHORTINFO_RESPONSE },
        { "Quote", TraderCommandType.QUOTE_RESPONSE },
        { "LimitPrice", TraderCommandType.LDLU_RESPONSE },
        { "IPosition", TraderCommandType.IPOS_RESPONSE },
        { "IOrder", TraderCommandType.IORDER_RESPONSE },
        { "ITrade", TraderCommandType.ITRADE_RESPONSE },
        { "TimeSalesQuote", TraderCommandType.TS_RESPONSE },
        { "Level2Quote", TraderCommandType.LV2_RESPONSE },
        { "ChartData", TraderCommandType.BAR_RESPONSE }
    };

    private CancellationTokenSource _cancellationTokenSource;
    private Mock<INetworkStream> _mockStream;
    private ResponseProcessor _responseProcessor;

    [TestInitialize]
    public void Setup()
    {
        _cancellationTokenSource = new CancellationTokenSource();
        _responseProcessor = new ResponseProcessor(_cancellationTokenSource.Token);
        _mockStream = new Mock<INetworkStream>();
    }


    [TestMethod]
    public void WrapResponse_ShouldRaiseCorrectEvent()
    {
        // Arrange
        var raisedEvent = false;
        _responseProcessor.LoginResponse += (sender, args) =>
        {
            raisedEvent = true;
            Assert.AreEqual(TraderCommandType.LOGIN_RESPONSE, args.CommandType);
        };

        var stringBuilder = new StringBuilder($"{TraderCommandType.LOGIN_RESPONSE.GetDescription()} Description\r\n");
        var wrapArgsType = typeof(ResponseProcessor).GetNestedType("WrapResponseEventArgs", BindingFlags.NonPublic);
        var constructor = wrapArgsType?.GetConstructors(BindingFlags.Public | BindingFlags.Instance)
            .FirstOrDefault(ctor => ctor.GetParameters().Length == 2);

        if (constructor == null) Assert.Fail("WrapResponseEventArgs constructor not found.");

        var wrapArgsInstance = constructor?.Invoke(new object[] { stringBuilder, Guid.NewGuid() });

        var onWrapResponseMethod =
            typeof(ResponseProcessor).GetMethod("OnWrapResponse", BindingFlags.NonPublic | BindingFlags.Instance);

        if (onWrapResponseMethod == null) Assert.Fail("OnWrapResponse method not found.");

        // Act
        onWrapResponseMethod.Invoke(_responseProcessor, new[] { wrapArgsInstance });

        // Assert
        Assert.IsTrue(raisedEvent);
    }

    [TestMethod]
    public void WrapResponse_ShouldRaiseAllEvent()
    {
        var eventRaised = new Dictionary<string, bool>();
        var eventHandlers = new Dictionary<string, Delegate>();

        // Subscribe to events
        foreach (var eventName in EventEnumDictionary)
        {
            eventRaised[eventName.Key] = false;
            var eventInfo = typeof(ResponseProcessor).GetEvent(eventName.Key);
            if (eventInfo != null)
            {
                // Subscribe to the event
                var handler = new EventHandler<ResponseEventArgs>((sender, args) =>
                {
                    var keyValue = EventEnumDictionary.FirstOrDefault(i => i.Value == args.CommandType);
                    eventRaised[keyValue.Key] = true;
                });

                eventInfo.AddEventHandler(_responseProcessor, handler);

                // Store the event handler
                eventHandlers[eventName.Key] = handler;

                // Verify that subscription occurs once
                var eventHandlersCount = GetEventHandlersCount(eventName.Value, _responseProcessor);
                Assert.AreEqual(1, eventHandlersCount,
                    $"Subscription to {eventName.Key} event occurred more than once.");
            }
        }

        var wrapArgsType = typeof(ResponseProcessor).GetNestedType("WrapResponseEventArgs", BindingFlags.NonPublic);
        var constructor = wrapArgsType?.GetConstructors(BindingFlags.Public | BindingFlags.Instance)
            .FirstOrDefault(ctor => ctor.GetParameters().Length == 2);

        if (constructor == null) Assert.Fail("WrapResponseEventArgs constructor not found.");

        var onWrapResponseMethod =
            typeof(ResponseProcessor).GetMethod("OnWrapResponse", BindingFlags.NonPublic | BindingFlags.Instance);

        if (onWrapResponseMethod == null) Assert.Fail("OnWrapResponse method not found.");

        // Act
        foreach (var commandType in EventEnumDictionary)
        {
            var stringBuilder = new StringBuilder($"{commandType.Value.GetDescription()} Description\r\n");
            var wrapArgsInstance = constructor?.Invoke(new object[] { stringBuilder, Guid.NewGuid() });
            onWrapResponseMethod.Invoke(_responseProcessor, new[] { wrapArgsInstance });
        }

        // Unsubscribe from events
        foreach (var eventName in EventEnumDictionary)
        {
            var eventInfo = typeof(ResponseProcessor).GetEvent(eventName.Key);
            if (eventInfo != null)
            {
                // Remove the event handler
                if (eventHandlers.TryGetValue(eventName.Key, out var handler))
                    eventInfo.RemoveEventHandler(_responseProcessor, handler);

                // Verify that unsubscription occurs once
                var subscribersCount = GetEventHandlersCount(eventName.Value, _responseProcessor);
                Assert.AreEqual(0, subscribersCount,
                    $"Unsubscription from {eventName.Key} event occurred more than once.");
            }
        }

        // Assert
        foreach (var eventName in EventEnumDictionary)
        {
            Assert.IsTrue(eventRaised[eventName.Key], $"{eventName.Key} event not raised.");
            Console.WriteLine($"{eventName.Key} event raised.");
        }
    }

    [TestMethod]
    public async Task ListenAsync_ShouldReadFromStreamUntilCancelled()
    {
        // Arrange
        _mockStream.Setup(s => s.CanRead).Returns(true);
        _mockStream.Setup(s => s.ReadAsync(It.IsAny<byte[]>())).ReturnsAsync(0);

        // Act
        var listenTask = _responseProcessor.ListenAsync(_mockStream.Object);
        _cancellationTokenSource.Cancel(); // Cancel the token to stop the loop

        await listenTask; // Wait for the task to complete

        // Assert
        _mockStream.Verify(s => s.ReadAsync(It.IsAny<byte[]>()), Times.AtLeastOnce);
    }


    private int GetEventHandlersCount(TraderCommandType commandType, IResponseProcessor responseProcessor)
    {
        // Use reflection to access the private _eventKeys field
        var eventKeysField =
            typeof(ResponseProcessor).GetField("_eventKeys", BindingFlags.NonPublic | BindingFlags.Instance);
        var eventKeys = (ConcurrentDictionary<TraderCommandType, object>)eventKeysField.GetValue(responseProcessor);

        // Retrieve the event key for the specified command type
        var eventKey = eventKeys.GetOrAdd(commandType, key => new object());

        // Use reflection to access the private ListEventDelegates field
        var listEventDelegatesField =
            typeof(ResponseProcessor).GetField("ListEventDelegates", BindingFlags.NonPublic | BindingFlags.Instance);
        var listEventDelegates = (EventHandlerList)listEventDelegatesField.GetValue(responseProcessor);

        // Retrieve the event handlers for the specified event key
        var eventHandlers = listEventDelegates[eventKey];

        // If event handlers exist, return the count; otherwise, return 0
        return eventHandlers?.GetInvocationList().Length ?? 0;
    }
}
