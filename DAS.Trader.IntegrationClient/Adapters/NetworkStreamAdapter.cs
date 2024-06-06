using System.Net.Sockets;
using DAS.Trader.IntegrationClient.Interfaces;

namespace DAS.Trader.IntegrationClient.Adapters;

public class NetworkStreamAdapter : INetworkStream
{
    private readonly NetworkStream _networkStream;

    public NetworkStreamAdapter(NetworkStream networkStream)
    {
        _networkStream = networkStream;
    }
    
    // Explicitly implement IDisposable
    void IDisposable.Dispose()
    {
        _networkStream.Dispose();
    }

    // Explicitly implement INetworkStream methods
    Task<int> INetworkStream.ReadAsync(byte[] buffer)
    {
        return _networkStream.ReadAsync(buffer, 0, buffer.Length);
    }

    bool INetworkStream.CanRead
    {
        get { return _networkStream.CanRead; }
        set { /* Not applicable, as CanRead is read-only */ }
    }

    bool INetworkStream.DataAvailable
    {
        get { return _networkStream.DataAvailable; }
        set { /* Not applicable, as DataAvailable is read-only */ }
    }

    public async Task WriteAsync(byte[] buffer, int offset, int count)
    {
        await _networkStream.WriteAsync(buffer, offset, count);
    }

    public void Close()
    {
        _networkStream.Close();
    }

    // You can add additional methods and properties here
    // that are specific to your wrapper class
}