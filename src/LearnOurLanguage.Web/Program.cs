using System.IO;
using Microsoft.AspNetCore.Hosting;
using Model.Helpers;

namespace LearnOurLanguage.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // todo: przeniesienie do DI?
            MapperHelper.InitializeMaps();

            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}
