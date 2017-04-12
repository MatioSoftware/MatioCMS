using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;

namespace MatioCMS
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseSetting("detailedErrors", "true")
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}
