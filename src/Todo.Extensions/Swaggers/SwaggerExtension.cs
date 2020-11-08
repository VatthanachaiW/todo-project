using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

namespace Todo.Extensions.Swaggers
{
    public static class SwaggerExtension
    {
        public static IServiceCollection AddSwaggerDocuments(this IServiceCollection services,
            params string[] assemblyNames)
        {
            SwaggerOption options;
            IHttpContextAccessor httpContextAccessor;
            using (var serviceProvider = services.BuildServiceProvider())
            {
                var configuration = serviceProvider.GetService<IConfiguration>();
                httpContextAccessor = serviceProvider.GetService<IHttpContextAccessor>();

                // ดึง Configuration มาใช้งาน
                services.Configure<SwaggerOption>(configuration.GetSection("swagger"));
                options = configuration.GetOptions<SwaggerOption>("swagger");
            }

            if (!options.Enabled)
            {
                return services;
            }

            return services.AddSwaggerGen(cfg =>
            {
                // Register API Document แต่ละ Version
                foreach (var version in options.Versions)
                {
                    cfg.SwaggerDoc(version.Version, new OpenApiInfo {Title = version.Title, Version = version.Version});
                }

                //เรียกใช้ Extensions
                cfg.OperationFilter<SecurityRequirementsOperationFilter>();
                cfg.OperationFilter<RemoveVersionFromParameter>();
                cfg.OperationFilter<SwaggerJsonIgnore>();
                cfg.DocumentFilter<ReplaceVersionWithExactValueInPath>();

                cfg.DocInclusionPredicate((s, description) =>
                    description.ApplyPredicate(s, httpContextAccessor.HttpContext));

                cfg.EnableAnnotations();

                //ดึงเอา xml summary ของ namespace ต่างๆ ที่ระบุเอาไว้มาใช้งาน
                if (assemblyNames.Length > 0)
                {
                    foreach (var name in assemblyNames)
                    {
                        cfg.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{name}.xml"));
                    }
                }

                //สำหรับเปิดใช้งาน Authentication ในกรณีที่ Method นั้นมีการเปิดใช้
                if (options.IncludeSecurity)
                {
                    cfg.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Description =
                            "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey
                    });
                }
            });
        }

        public static IApplicationBuilder UseSwaggerDocuments(this IApplicationBuilder builder)
        {
            var options = builder.ApplicationServices.GetService<IConfiguration>()
                .GetOptions<SwaggerOption>("swagger");
            if (!options.Enabled)
            {
                return builder;
            }

            builder.UseStaticFiles()
                .UseSwagger(c => c.RouteTemplate = options.RoutePrefix + "/{documentName}/swagger.json");

            //Register Endpoint ให้กับ Document ของ API แต่ละ Version
            return options.ReDocEnabled
                ? builder.UseReDoc(c =>
                {
                    c.RoutePrefix = options.RoutePrefix;
                    foreach (var version in options.Versions)
                    {
                        c.SpecUrl = $"{version.Version}/swagger.json";
                    }
                })
                : builder.UseSwaggerUI(c =>
                {
                    foreach (var version in options.Versions)
                    {
                        c.SwaggerEndpoint($"/{options.RoutePrefix}/{version.Version}/swagger.json", version.Title);
                    }

                    c.RoutePrefix = options.RoutePrefix;
                });
        }
    }
}