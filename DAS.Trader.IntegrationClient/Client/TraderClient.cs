using System.Net;
using System.Net.Sockets;
using DAS.Trader.IntegrationClient.Commands;
using DAS.Trader.IntegrationClient.Commands.Interfaces;

namespace DAS.Trader.IntegrationClient.Client;

internal class TraderClient : IDisposable
{
    private const int DefaultTimeOutInSeconds = 3;
    private readonly IPEndPoint _ipEndPoint;
    private readonly ResponseProcessor _responseProcessor;
    private readonly TcpClient _tcpClient;
    private readonly TimeSpan _timeOut;
    private readonly CancellationToken _cancellationToken;
    private NetworkStream? _currentStream;

    public TraderClient(string ipAddress, int port, TimeSpan? timeOut = null)
    {
        _timeOut = timeOut ?? TimeSpan.FromSeconds(DefaultTimeOutInSeconds);
        _ipEndPoint = new IPEndPoint(IPAddress.Parse(ipAddress), port);
        _tcpClient = new TcpClient();
        _cancellationToken = new CancellationToken();
        _responseProcessor = new ResponseProcessor(_cancellationToken);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public async Task ConnectAsync()
    {
        await _tcpClient.ConnectAsync(_ipEndPoint);
        Task.Factory.StartNew(item => _responseProcessor.ListenAsync(GetStream()),
            TaskCreationOptions.LongRunning, _cancellationToken);
    }

    public NetworkStream GetStream()
    {
        return _currentStream ??= _tcpClient.GetStream();
    }

    public async Task<ICommandResult> SendCommandAsync(ITcpCommand command)
    {
        ICommandResult? result;

        var commandText = command.ToString();
        var buffer = command.ToByteArray(commandText);

        Console.Write($"|>>>|    {commandText}");

        try
        {
            var starTime = DateTime.UtcNow;
            if (command.WaitForResult) command.Subscribe(_responseProcessor);

            await GetStream().WriteAsync(buffer, 0, buffer.Length);

            if (command.WaitForResult)
            {
                while (!command.HasResult)
                    if (DateTime.UtcNow - starTime > _timeOut)
                        throw new TimeoutException($"Timeout occurred. Command {command.Name}");

                result = new CommandResult { Message = command.Result?.ToString() ?? string.Empty, Success = true };
            }
            else
            {
                result = CommandResult.SuccessResult;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            result = new CommandResult { Message = e.Message };
        }
        finally
        {
            command.Unsubscribe(_responseProcessor);
        }

        return result;
    }

    private void ReleaseUnmanagedResources()
    {
        // TODO release unmanaged resources here
    }

    protected virtual void Dispose(bool disposing)
    {
        ReleaseUnmanagedResources();
        if (!disposing) return;

        _currentStream?.Close();
        _tcpClient.Close();
    }

    ~TraderClient()
    {
        Dispose(false);
    }
}