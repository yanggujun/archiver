using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace Archiver.Host
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseKestrel()
                .UseWebRoot("content")
                .UseStartup<Startup>()
                .Build();
            host.Run();
        }
    }
}
