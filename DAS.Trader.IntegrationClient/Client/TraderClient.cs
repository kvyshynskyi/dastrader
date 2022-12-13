using System.Net;
using System.Net.Sockets;
using DAS.Trader.IntegrationClient.Commands;
using DAS.Trader.IntegrationClient.Commands.Interfaces;
using DAS.Trader.IntegrationClient.Response;

namespace DAS.Trader.IntegrationClient.Client;

internal class TraderClient : IDisposable
{
    private const int DefaultTimeOutInSeconds = 3;
    private readonly CancellationToken _cancellationToken;
    private readonly IPEndPoint _ipEndPoint;
    private readonly ResponseProcessor _responseProcessor;
    private readonly TcpClient _tcpClient;
    private readonly TimeSpan _timeOut;
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

    public event EventHandler<ResponseEventArgs> PriceInquiry
    {
        add => _responseProcessor.PriceInquiry += value;
        remove => _responseProcessor.PriceInquiry -= value;
    }

    public event EventHandler<ResponseEventArgs> LoginResponse
    {
        add => _responseProcessor.LoginResponse += value;
        remove => _responseProcessor.LoginResponse -= value;
    }

    public event EventHandler<ResponseEventArgs> SlOrder
    {
        add => _responseProcessor.SlOrder += value;
        remove => _responseProcessor.SlOrder -= value;
    }

    public event EventHandler<ResponseEventArgs> SlOrderBegin
    {
        add => _responseProcessor.SlOrderBegin += value;
        remove => _responseProcessor.SlOrderBegin -= value;
    }

    public event EventHandler<ResponseEventArgs> SlOrderEnd
    {
        add => _responseProcessor.SlOrderEnd += value;
        remove => _responseProcessor.SlOrderEnd -= value;
    }


    public async Task<ICommandResult> SendCommandAsync(ITcpCommand command)
    {
        ICommandResult? result;

        var commandText = command.ToString();
        var buffer = command.ToByteArray(commandText);

        Console.Write($"{new string(' ', 42)}|>>| {commandText}");

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