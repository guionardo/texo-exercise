using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using texo.data.DependencyInjection;
using texo.data.Interfaces;

namespace texo.api
{
    public class Startup
    {
        public Startup(IConfiguration _)
        {
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "texo.api", Version = "v1" });
            });

            services
                .AddLogging()
                .AddDataServices();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IDatabaseBootstrap databaseBootstrap,
            IAggregatorService aggregatorService, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "texo.api v1"));
                logger.LogInformation("Development mode: added swagger");
            }
            else
            {
                app.UseHttpsRedirection();
                logger.LogInformation("Production mode");
            }

            databaseBootstrap.Setup();
            aggregatorService.LoadDataFromCsv();


            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}