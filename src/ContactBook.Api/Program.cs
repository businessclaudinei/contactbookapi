using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ContactBook.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
    {
        var port = Environment.GetEnvironmentVariable("PORT");
        port = string.IsNullOrEmpty(port) ? "5000" : port;
        return WebHost.CreateDefaultBuilder(args)
            .UseStartup<Startup>()
            .UseUrls("http://*:"+port);
    }
    }
}
