
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace API.Extensions
{
    public static class SwaggerServiceExtensions
    {
        private static string _apiVersion;
        private static string _apiName;

        // TODO: Clarify config copy and disposal
        public static IServiceCollection AddSwaggerDocumentation(
            this IServiceCollection services,
            IConfiguration config
        )
        {
            SetApiDocumentationProperties(config);
            services.AddSwaggerGen(o => {
                o.SwaggerDoc(_apiVersion, new OpenApiInfo{
                    Title = _apiName,
                    Version = _apiVersion
                });
            });

            return services;
        }

        public static IApplicationBuilder UseSwaggerDocumentation(
            this IApplicationBuilder app,
            IConfiguration config
        )
        {
            SetApiDocumentationProperties(config);
            app.UseSwagger();
            app.UseSwaggerUI(o => {
                o.SwaggerEndpoint(
                    $"/swagger/{_apiVersion}/swagger.json", _apiName);
            });

            return app;
        }

        private static void SetApiDocumentationProperties(IConfiguration config) { 
            _apiName = config.GetValue<string>("ApiDocumentation:Name");
            _apiVersion = config.GetValue<string>("ApiDocumentation:Version");
        }
    }
}