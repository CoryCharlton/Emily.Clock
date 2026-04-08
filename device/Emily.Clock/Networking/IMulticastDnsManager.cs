namespace Emily.Clock.Networking;

public interface IMulticastDnsManager
{
    string HostName { get; }
    bool IsRunning { get; }

    void Start(string ipAddress);
    void Stop();
}
