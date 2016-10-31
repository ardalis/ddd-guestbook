using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using CleanArchitecture.Infrastructure.Data;

namespace CleanArchitecture.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                //.ConfigureServices(services =>
                //{
                //    services.AddDbContext<AppDbContext>(options =>
                //        options.UseInMemoryDatabase(Guid.NewGuid().ToString()));
                //})
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}
