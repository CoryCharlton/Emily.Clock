using System.Threading;
using MakoIoT.Device;

namespace Emily.Clock.App
{
    public class Program
    {
        public static void Main()
        {
            // TODO: Replace MakoIoT.Device with nanoFramework.Hosting / new DeviceHost / DeviceHostBuilder
            var builder = DeviceBuilder.Create()
                .ConfigureDependencyInjection()
                .AddCore();

            var device = builder.Build();

            device.Start();

            Thread.Sleep(Timeout.Infinite);
        }
    }
}
