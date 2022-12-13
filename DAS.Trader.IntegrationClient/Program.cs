using DAS.Trader.IntegrationClient.Client;
using DAS.Trader.IntegrationClient.Commands.Interfaces;
using DAS.Trader.IntegrationClient.Commands.TcpCommands;
using DAS.Trader.IntegrationClient.Response;

const int cmdApiPort = 9910;
const string cmdApiIpString = "192.168.56.101";
const string login = "USE_CORRECT_VALUE_FOR_TEST";
const string password = "USE_CORRECT_VALUE_FOR_TEST";
const string account = "USE_CORRECT_VALUE_FOR_TEST";

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

    var commandList = new List<ITcpCommand>
    {
        new LoginCommand(login, password, account),
        //OrderServerConnectionStatusCommand.Instance,
        //OrderServerLogOnStatusCommand.Instance,
        //QuoteServerConnectionStatusCommand.Instance,
        //QuoteServerLogOnStatusCommand.Instance,
        ClientCommand.Instance,
        EchoCommand.Instance,
        GetBuyingPowerCommand.Instance,
        //new SlPriceInquireCommand("SL", 100),
        //new SlPriceInquireCommand("SL", 200, "TESTSL"),
        //new SlPriceInquireCommand("MSFT", 100),
        //new SlPriceInquireCommand("AACIW", 100),
        //new SlNewOrderCommand("AACIW", 100, "TESTSL"),
        //new SlNewOrderCommand("SL", 400, "TESTSL"),
        //new SlAvailQueryCommand(account, "SL"),
        //new SlAvailQueryCommand(account, "AACIW"),
        QuitCommand.Instance
    };

    ICommandResult? result;
    foreach (var command in commandList)
    {
        Thread.Sleep(TimeSpan.FromMilliseconds(500));
        result = await client.SendCommandAsync(command);
        if (!result.Success) return;
    }
}

void EventNotification(object? sender, ResponseEventArgs args)
{
    Console.WriteLine(
        $"|<-| {args.CorrelationId.ToString().ToUpper()} |<<| Event detected: {args.CommandType} |<<| {args.Parameters.Length} params |<<| {string.Join(", ", args.Parameters)}");
}