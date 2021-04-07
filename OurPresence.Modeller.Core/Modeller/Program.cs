using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DotNetConfig;
using McMaster.Extensions.CommandLineUtils;

namespace Modeller
{
    public class Program
    {
        public static void Main(string[] args)
        {
            DebugHelper.HandleDebugSwitch(ref args);
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) => { services.AddHostedService<Worker>(); });
    }
}