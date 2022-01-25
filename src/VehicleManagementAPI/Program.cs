using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore;
using Serilog;
using System;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Extensions;
using Microsoft.OpenApi;

namespace Pitstop.Application.VehicleManagement
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args)
                .Run();
        }

        private static IWebHost BuildWebHost(string[] args)
        {
            var webHost = WebHost.CreateDefaultBuilder(args)
                .UseSerilog()
                .UseHealthChecks("/hc")
                .UseStartup<Startup>()
                .Build();

            // If args, then it's for generating swagger and exit
            // otherwise run web server
            if (args.Length > 0)
            {
                Console.WriteLine(GenerateSwagger(webHost, args[0]));
                Environment.Exit(0);
            }

            return webHost;
        }

        private static string GenerateSwagger(IWebHost host, string docName)
        {
            ISwaggerProvider sw = (ISwaggerProvider)host.Services.GetService(typeof(ISwaggerProvider));
            OpenApiDocument doc = sw.GetSwagger(docName);
            return doc.SerializeAsJson(OpenApiSpecVersion.OpenApi3_0);
        }
    }
}
