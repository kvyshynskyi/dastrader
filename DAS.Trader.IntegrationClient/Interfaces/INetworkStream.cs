namespace DAS.Trader.IntegrationClient.Interfaces;

public interface INetworkStream: IDisposable
{
    Task<int> ReadAsync(byte[] buffer);
    bool CanRead { get; set; }
    bool DataAvailable { get; set; }
    Task WriteAsync(byte[] buffer, int offset, int count);
    void Close();
}