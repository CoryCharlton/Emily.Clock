using CCSWE.nanoFramework.Hosting;
using nanoFramework.Hosting;

namespace Emily.Clock.App
{
    public class Program
    {
        public static void Main()
        {
            var builder = DeviceHost.CreateDefaultBuilder()
                .ConfigureHardware()
                .AddCore();

            using var host = builder.Build();

            host.Run();
        }
    }
}
