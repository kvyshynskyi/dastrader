using System.Diagnostics;
using System.Net;
using DAS.Trader.IntegrationClient.Adapters;
using DAS.Trader.IntegrationClient.Commands;
using DAS.Trader.IntegrationClient.Interfaces;
using DAS.Trader.IntegrationClient.Response;

namespace DAS.Trader.IntegrationClient.Client;

public class TraderClient : ITraderClient
{
    private const int DefaultTimeOutInSeconds = 3;
    private readonly CancellationToken _cancellationToken;
    private readonly IPEndPoint _ipEndPoint;
    private readonly IResponseProcessor _responseProcessor;
    private readonly ITcpClient _tcpClient;
    private readonly TimeSpan _timeOut;
    private INetworkStream? _currentStream;

    public TraderClient(string ipAddress, int port, TimeSpan? timeOut = null, ITcpClient? tcpClient = null, IResponseProcessor? responseProcessor = null)
    {
        _timeOut = timeOut ?? TimeSpan.FromSeconds(DefaultTimeOutInSeconds);
        _ipEndPoint = new IPEndPoint(IPAddress.Parse(ipAddress), port);
        _tcpClient = tcpClient ?? new TcpClientAdapter();
        _cancellationToken = new CancellationToken();
        _responseProcessor = responseProcessor ?? new ResponseProcessor(_cancellationToken);
    }

    public async Task ConnectAsync()
    {
        await _tcpClient.ConnectAsync(_ipEndPoint.Address, _ipEndPoint.Port);
#pragma warning disable CS4014
        Task.Factory.StartNew(item => _responseProcessor.ListenAsync(GetStream()),
            TaskCreationOptions.LongRunning, _cancellationToken);
#pragma warning restore CS4014
    }

    public INetworkStream GetStream()
    {
        return _currentStream ??= _tcpClient.GetStream();
    }

    public async Task<ICommandResult> SendCommandAsync(ITcpCommand command)
    {
        ICommandResult? result;

        var commandText = command.ToString();
        var buffer = command.ToByteArray(commandText);

        Debug.Write($"{new string(' ', 42)}|>>| {commandText}");

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
            Debug.WriteLine(e);
            result = new CommandResult { Message = e.Message };
        }
        finally
        {
            command.Unsubscribe(_responseProcessor);
        }

        return result;
    }

    #region Events

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

    public event EventHandler<ResponseEventArgs> OrderBegin
    {
        add => _responseProcessor.OrderBegin += value;
        remove => _responseProcessor.OrderBegin -= value;
    }

    public event EventHandler<ResponseEventArgs> OrderEnd
    {
        add => _responseProcessor.OrderEnd += value;
        remove => _responseProcessor.OrderEnd -= value;
    }

    public event EventHandler<ResponseEventArgs> Order
    {
        add => _responseProcessor.Order += value;
        remove => _responseProcessor.Order -= value;
    }

    public event EventHandler<ResponseEventArgs> PositionBegin
    {
        add => _responseProcessor.PositionBegin += value;
        remove => _responseProcessor.PositionBegin -= value;
    }

    public event EventHandler<ResponseEventArgs> PositionEnd
    {
        add => _responseProcessor.PositionEnd += value;
        remove => _responseProcessor.PositionEnd -= value;
    }

    public event EventHandler<ResponseEventArgs> Position
    {
        add => _responseProcessor.Position += value;
        remove => _responseProcessor.Position -= value;
    }

    public event EventHandler<ResponseEventArgs> TradeBegin
    {
        add => _responseProcessor.TradeBegin += value;
        remove => _responseProcessor.TradeBegin -= value;
    }

    public event EventHandler<ResponseEventArgs> TradeEnd
    {
        add => _responseProcessor.TradeEnd += value;
        remove => _responseProcessor.TradeEnd -= value;
    }

    public event EventHandler<ResponseEventArgs> Trade
    {
        add => _responseProcessor.Trade += value;
        remove => _responseProcessor.Trade -= value;
    }

    public event EventHandler<ResponseEventArgs> OrderAction
    {
        add => _responseProcessor.OrderAction += value;
        remove => _responseProcessor.OrderAction -= value;
    }

    public event EventHandler<ResponseEventArgs> BuyingPower
    {
        add => _responseProcessor.BuyingPower += value;
        remove => _responseProcessor.BuyingPower -= value;
    }

    public event EventHandler<ResponseEventArgs> ClientCount
    {
        add => _responseProcessor.ClientCount += value;
        remove => _responseProcessor.ClientCount -= value;
    }

    public event EventHandler<ResponseEventArgs> ShortInfo
    {
        add => _responseProcessor.ShortInfo += value;
        remove => _responseProcessor.ShortInfo -= value;
    }

    public event EventHandler<ResponseEventArgs> Quote
    {
        add => _responseProcessor.Quote += value;
        remove => _responseProcessor.Quote -= value;
    }

    public event EventHandler<ResponseEventArgs> LimitPrice
    {
        add => _responseProcessor.LimitPrice += value;
        remove => _responseProcessor.LimitPrice -= value;
    }

    public event EventHandler<ResponseEventArgs> IPosition
    {
        add => _responseProcessor.IPosition += value;
        remove => _responseProcessor.IPosition -= value;
    }

    public event EventHandler<ResponseEventArgs> IOrder
    {
        add => _responseProcessor.IOrder += value;
        remove => _responseProcessor.IOrder -= value;
    }

    public event EventHandler<ResponseEventArgs> ITrade
    {
        add => _responseProcessor.ITrade += value;
        remove => _responseProcessor.ITrade -= value;
    }

    public event EventHandler<ResponseEventArgs> TimeSalesQuote
    {
        add => _responseProcessor.TimeSalesQuote += value;
        remove => _responseProcessor.TimeSalesQuote -= value;
    }

    public event EventHandler<ResponseEventArgs> Level2Quote
    {
        add => _responseProcessor.Level2Quote += value;
        remove => _responseProcessor.Level2Quote -= value;
    }
    public event EventHandler<ResponseEventArgs> ChartData
    {
        add => _responseProcessor.ChartData += value;
        remove => _responseProcessor.ChartData -= value;
    }

    #endregion

    #region IDisposable

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
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

    #endregion
}