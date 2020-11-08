using System;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Todo.API.Connections;
using Todo.Extensions.Swaggers;

namespace Todo.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options
                    .ConfigureWarnings(warnings =>
                        warnings.Throw(RelationalEventId.QueryPossibleExceptionWithAggregateOperatorWarning))
                    .UseSqlServer(Configuration.GetValue<string>("DefaultConnection"), sqlOptions =>
                    {
                        sqlOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(30), null!);
                        sqlOptions.MigrationsAssembly(
                            typeof(ApplicationDbContext).GetTypeInfo().Assembly.GetName().Name);
                    });
            });

            // สำหรับเรียกใช้ Options ต่างๆ 
            services.AddOptions();
            // สำหรับเรียกใช้ IHttpContextAccessor
            services.AddHttpContextAccessor();
            // สำหรับให้ API เรารองรับหลาย Version
            services.AddApiVersioning();
            // สำหรับสร้าง Swagger Document
            services.AddSwaggerDocuments("Todo.API");

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            // เปิดใช้งาน Swagger
            app.UseSwaggerDocuments();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
