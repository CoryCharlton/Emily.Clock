using System;
using System.Net.NetworkInformation;

namespace Emily.Clock.Networking
{
    public interface INetworkInterfaceProvider
    {
        NetworkInterface? GetInterface(NetworkInterfaceType interfaceType);
        NetworkInterface RequireInterface(NetworkInterfaceType interfaceType);
    }

    public class NetworkInterfaceProvider : INetworkInterfaceProvider
    {
        public NetworkInterface? GetInterface(NetworkInterfaceType interfaceType)
        {
            var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();

            foreach (var networkInterface in networkInterfaces)
            {
                if (networkInterface.NetworkInterfaceType == interfaceType)
                {
                    return networkInterface;
                }
            }

            return null;
        }

        public NetworkInterface RequireInterface(NetworkInterfaceType interfaceType)
        {
            return GetInterface(interfaceType) ?? throw new ArgumentException();
        }
    }
}
