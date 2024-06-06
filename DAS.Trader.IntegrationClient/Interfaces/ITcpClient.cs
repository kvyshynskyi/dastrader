using System.Net;
using System.Net.Sockets;

namespace DAS.Trader.IntegrationClient.Interfaces;

public interface ITcpClient : IDisposable
{
    Task ConnectAsync(IPAddress address, int port);
    INetworkStream GetStream();
    void Close();
}