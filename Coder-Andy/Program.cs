using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace CoderAndy
{
    public class Program
    {
        public static void Main(string[] a_args)
        {
            CreateWebHostBuilder(a_args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
