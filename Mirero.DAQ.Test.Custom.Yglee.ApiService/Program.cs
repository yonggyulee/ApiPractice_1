using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mirero.DAQ.Test.Custom.Yglee.ApiService.Context;

namespace Mirero.DAQ.Test.Custom.Yglee.ApiService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //var config = new ConfigurationBuilder()
            //        .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            //        .AddJsonFile("appsettings.json")
            //        .Build();

            //Console.WriteLine($"::::: {config.GetConnectionString("Default")}");

            CreateHostBuilder(args).Build().Run();
        }
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<StartUp>();
                });
    }
}

// C#10 without StartUp.cs
// var builder = WebApplication.CreateBuilder(args);
//
// // AddAsync services to the container.
//
// builder.Services.AddControllers()
//     .AddJsonOptions(options => 
//         options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
// builder.Services.AddDbContext<MainDbContext>(options =>
//     {
//         options
//             .UseNpgsql(builder.Configuration.GetConnectionString("MainDatabase"));
//     }
// );
// // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// builder.Services.AddEndpointsApiExplorer();
//
// var app = builder.Build();
//
// // Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     
// }
//
// app.UseHttpsRedirection();
//
// app.UseAuthorization();
//
// app.MapControllers();
//
// app.MapGet("main", (IHostEnvironment env) => new {Environment = env.EnvironmentName});
//
// app.Run();