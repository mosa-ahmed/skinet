using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Data;
using Infrastructure.Data.SeedData;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            //Insted of using the command of dotnet ef database update, we will create our databse inside our main method here
            var host = CreateHostBuilder(args).Build();
            
            using(var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var loggerFactory = services.GetRequiredService<ILoggerFactory>();

                //Because we are out of StartUp class which means we are not gonna have any exception handling where we are now, so we will add try{} catch{}
                try
                {
                    var context = services.GetRequiredService<StoreContext>();
//This method MigrateAsync() is what we are looking for, because we 've dropped our databse and we 've got a migration(InitialCreate) and when we start our application then we want to create our database using any migrations that we have
                    await context.Database.MigrateAsync();
                    await StoreContextSeed.SeedAsync(context, loggerFactory);
                }
                catch(Exception ex)
                {
                    var logger = loggerFactory.CreateLogger<Program>();   //We specify the class that we want to log against
                    logger.LogError(ex, "An error occured during migration");
                }

            }
            host.Run();
        }

// our main method is executed and this CreateHostBuilder is called and we also provide it with some additional configration inside the StartUp class

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
