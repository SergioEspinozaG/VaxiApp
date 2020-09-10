using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dominio;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Persistencia;

namespace WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var hostServer = CreateHostBuilder(args).Build();
                using(var ambiente = hostServer.Services.CreateScope()){
                    var service = ambiente.ServiceProvider;
                    
                    try
                    {
                        var userManager = service.GetRequiredService<UserManager<Usuario>>();
                        var context = service.GetRequiredService<CursosOnlineContext>();
                        context.Database.Migrate();
                        DataPrueba.InsertData(context, userManager).Wait();
                    }
                    catch (Exception e)
                    {
                        
                        var logging = service.GetRequiredService<ILogger<Program>>();
                        logging.LogError(e, "Ocurrio un error en la migracion");
                    }
                    hostServer.Run();
                }

        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
