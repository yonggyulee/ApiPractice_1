using System.Reflection;
using System.Text.Json.Serialization;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mirero.DAQ.Test.Custom.Yglee.ApiService.Context;
using Mirero.DAQ.Test.Custom.Yglee.ApiService.Models;
using Mirero.DAQ.Test.Custom.Yglee.ApiService.Services;
using Mirero.DAQ.Test.Custom.Yglee.ApiService.Services.Utils;

namespace Mirero.DAQ.Test.Custom.Yglee.ApiService
{
    public class StartUp
    {
        public StartUp(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        
        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddControllers()
            //    .AddJsonOptions(options =>
            //        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve);

            services.AddControllers()
                .AddFluentValidation(fv =>
                {
                    fv.ImplicitlyValidateChildProperties = true;
                    fv.ImplicitlyValidateRootCollectionElements = true;

                    fv.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
                });

            services.AddDbContext<MainDbContext>(options =>
            {
                options
                    .UseNpgsql(Configuration.GetConnectionString("MainDatabase"));
            });

            services.AddTransient<IDatasetManagementService, DatasetManagementService>();
            services.AddTransient<IDirectoryManager, DirectoryManager>();
            services.AddTransient<IDatasetDbContextFactory, DatasetDbContextFactory>();
            services.AddAutoMapper(typeof(MapProfile));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGet("main", async context =>
                {
                    // context.Response.ContentType = "text/plain; charset=utf-8";
                    await context.Response.WriteAsync("DAQ Api Service Main View.");
                });
            });
        }
    }
}