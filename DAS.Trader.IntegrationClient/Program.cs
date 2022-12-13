using DAS.Trader.IntegrationClient.Client;
using DAS.Trader.IntegrationClient.Commands.Interfaces;
using DAS.Trader.IntegrationClient.Commands.TcpCommands;
using DAS.Trader.IntegrationClient.Enums;
using DAS.Trader.IntegrationClient.Response;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

using var host = Host.CreateDefaultBuilder(args).Build();
IConfiguration config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddJsonFile("appsettings.local.json", true, true)
    .AddEnvironmentVariables()
    .Build();

var cmdApiPort = config.GetValue("DAS:Trader:CmdApiPort", 0);
var cmdApiIpString = config.GetValue("DAS:Trader:CmdApiIp", string.Empty);
var login = config.GetValue("DAS:Trader:Login", string.Empty);
var password = config.GetValue("DAS:Trader:Password", string.Empty);
var account = config.GetValue("DAS:Trader:Account", string.Empty);

var tasks = new[]
{
    ReadAsync()
};

Task.WaitAll(tasks);

Console.WriteLine("");
Console.WriteLine("Press Enter key to stop the program");
Console.ReadLine();

async Task ReadAsync()
{
    using var client = new TraderClient(cmdApiIpString, cmdApiPort);
    await client.ConnectAsync();
    client.PriceInquiry += EventNotification;
    client.LoginResponse += EventNotification;
    client.SlOrder += EventNotification;
    client.SlOrderBegin += EventNotification;
    client.SlOrderEnd += EventNotification;
    client.Order += EventNotification;
    client.OrderBegin += EventNotification;
    client.OrderEnd += EventNotification;
    client.Position += EventNotification;
    client.PositionBegin += EventNotification;
    client.PositionEnd += EventNotification;
    client.Trade += EventNotification;
    client.TradeBegin += EventNotification;
    client.TradeEnd += EventNotification;
    client.OrderAction += EventNotification;
    client.BuyingPower += EventNotification;
    client.ClientCount += EventNotification;
    client.ShortInfo += EventNotification;
    client.Quote += EventNotification;
    client.LimitPrice += EventNotification;
    client.IOrder += EventNotification;
    client.IPosition += EventNotification;
    client.ITrade += EventNotification;
    client.TimeSalesQuote += EventNotification;
    client.Level2Quote += EventNotification;
    client.ChartData += EventNotification;

    var commandList = new List<ITcpCommand>
    {
        new LoginCommand(login, password, account),

        ClientCommand.Instance,
        EchoCommand.Instance,
        GetBuyingPowerCommand.Instance,
        new SlPriceInquireCommand("SL"),
        new SlPriceInquireCommand("SL", 200, "TESTSL"),
        new SlPriceInquireCommand("AACIW"),
        new SlNewOrderCommand("AACIW", 100, "TESTSL"),
        new SlNewOrderCommand("SL", 400, "TESTSL"),

        new GetShortInfoCommand("SL"),
        new GetShortInfoCommand("AACIW"),
        new GetShortInfoCommand("MSFT"),
        new SubscribeLevelCommand("MSFT", TraderLevels.Level1)
    };

    ICommandResult? result;
    foreach (var command in commandList)
    {
        Thread.Sleep(TimeSpan.FromMilliseconds(100));
        result = await client.SendCommandAsync(command);
        if (!result.Success) return;
    }

    Console.WriteLine("");
    Console.WriteLine("Press Enter key to use QUIT command");
    Console.ReadLine();

    client.SendCommandAsync(new UnSubscribeLevel1Command("MSFT"));
    client.SendCommandAsync(QuitCommand.Instance);
}

void EventNotification(object? sender, ResponseEventArgs args)
{
    Console.WriteLine(
        $"|<-| {args.CorrelationId.ToString().ToUpper()} |<<| Event detected: {args.CommandType} |<<| {args.Parameters.Length} params |<<| {string.Join(", ", args.Parameters)}");
}