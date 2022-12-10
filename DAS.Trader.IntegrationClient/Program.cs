using DAS.Trader.IntegrationClient.Client;
using DAS.Trader.IntegrationClient.Commands.Interfaces;
using DAS.Trader.IntegrationClient.Commands.TcpCommands;

const int cmdApiPort = 9910;
const string cmdApiIpString = "192.168.56.101";
const string login = "INSERT VALID LOGIN";
const string password = "INSERT VALID PASSWORD";

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

    var commandList = new List<ITcpCommand>
    {
        new LoginCommand(login, password),
        OrderServerConnectionStatusCommand.Instance,
        OrderServerLogOnStatusCommand.Instance,
        QuoteServerConnectionStatusCommand.Instance,
        QuoteServerLogOnStatusCommand.Instance,
        ClientCommand.Instance,
        EchoCommand.Instance,
        GetBuyingPowerCommand.Instance,
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