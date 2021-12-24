using System.Reflection;
using FluentValidation.AspNetCore;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mirero.DAQ.Test.Custom.Yglee.ApiService.Common.Interfaces;
using Mirero.DAQ.Test.Custom.Yglee.ApiService.Common.Utils;
using Mirero.DAQ.Test.Custom.Yglee.ApiService.Context;
using Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.DTO.Main;
using Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.DTO.Storage;
using Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.Entity.Main;
using Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.Entity.Storage;
using Mirero.DAQ.Test.Custom.Yglee.ApiService.Services;
using IMapper = MapsterMapper.IMapper;

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
                    .UseNpgsql(Configuration.GetConnectionString("MainDatabase"))
                    .UseSnakeCaseNamingConvention();
            });

            services.AddTransient<IDatasetManagementService, DatasetManagementService>();
            services.AddTransient<IImageManagementService, ImageManagementService>();
            services.AddTransient<IDirectoryManager, DirectoryManager>();
            services.AddTransient<IFileManager, FileManager>();
            services.AddTransient<IDatasetDbContextFactory, DatasetDbContextFactory>();

            // services.AddAutoMapper(typeof(MapProfile));

            //services.AddSingleton(GetConfiguredMappingConfig());
            //services.AddScoped<IMapper, ServiceMapper>();
        }

        // Model과 DTO 간 변환에 특정 동작이나 설정이 필요할 경우 사용하여 매핑시킬 수 있다.
        //private static TypeAdapterConfig GetConfiguredMappingConfig()
        //{
        //    var config = new TypeAdapterConfig();
        //    // {
        //    //     Compiler = exp => exp.ComileWithDebugInfo(new ExpressionCompilationOptions
        //    //         {EmitFile = true, ThrowOnFailedCompilation = true})
        //    // };

        //    // Main
        //    config.NewConfig<Artifact, ArtifactDTO>();
        //    config.NewConfig<Auth, AuthDTO>();
        //    config.NewConfig<BatchJob, BatchJobDTO>();
        //    config.NewConfig<ClassCode, ClassCodeDTO>();
        //    config.NewConfig<ClassCodeSet, ClassCodeSetDTO>();
        //    config.NewConfig<Dataset, DatasetDTO>();
        //    config.NewConfig<Job, JobDTO>();
        //    config.NewConfig<UserAuthMap, UserAuthMapDTO>();
        //    config.NewConfig<User, UserDTO>();

        //    // Storage
        //    config.NewConfig<Sample, SampleDTO>();
        //    config.NewConfig<Image, ImageDTO>();
        //    config.NewConfig<LabelSet, LabelSetDTO>();
        //    config.NewConfig<ClassificationLabel, ClassificationLabelDTO>();
        //    config.NewConfig<ObjectDetectionLabel, ObjectDetectionLabelDTO>();
        //    config.NewConfig<SegmentationLabel, SegmentationLabelDTO>();

        //    return config;
        //}
        
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