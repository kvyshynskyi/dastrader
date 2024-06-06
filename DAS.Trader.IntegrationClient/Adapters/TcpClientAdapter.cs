using System.Net;
using System.Net.Sockets;
using DAS.Trader.IntegrationClient.Interfaces;

namespace DAS.Trader.IntegrationClient.Adapters;

public class TcpClientAdapter : ITcpClient
{
    private readonly TcpClient _tcpClient;

    public TcpClientAdapter(): this(new TcpClient())
    {
    }

    public TcpClientAdapter(TcpClient tcpClient)
    {
        _tcpClient = tcpClient;
    }

    public async Task ConnectAsync(IPAddress address, int port)
    {
        await _tcpClient.ConnectAsync(address, port);
    }

    public INetworkStream GetStream()
    {
        return new NetworkStreamAdapter(_tcpClient.GetStream());
    }

    public void Close()
    {
        _tcpClient.Close();
    }

    public void Dispose()
    {
        _tcpClient.Dispose();
    }
}