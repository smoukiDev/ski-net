using System.Collections.Generic;
using API.Extensions;
using API.Middleware;
using AutoMapper;
using Infrastructure.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace API
{
    public class Startup
    {
        private readonly IConfiguration _config;
        private readonly string _corsPolicy = "CorsPolicy";

        public Startup(IConfiguration config)
        {
            _config = config;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAppServices();
            services.AddDbContext<StoreContext>(b => 
                b.UseSqlite(_config.GetConnectionString("DefaultConnection")));
            services.AddSingleton<ConnectionMultiplexer>(c => {
                var configuration = 
                    ConfigurationOptions.Parse(_config.GetConnectionString("Redis"), true);
                return ConnectionMultiplexer.Connect(configuration);
            });

            services.AddControllers();
            services.ConfigureAppServices();

            services.AddAutoMapper(typeof(Helpers.MappingProfiles));
            services.AddSwaggerDocumentation(_config);
            // TODO: Extract to settings.json
            services.AddCors(o => 
                o.AddPolicy(_corsPolicy, policy =>
                    {
                        var origins = _config.GetSection("CorsAllowedOrigins").Get<List<string>>();
                        foreach(var origin in origins)
                        {
                            policy.AllowAnyHeader().AllowAnyMethod().WithOrigins(origin);
                        }
                    }
                )
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<ExceptionMiddleware>();

            // if (env.IsDevelopment()) {app.UseDeveloperExceptionPage();}

            app.UseStatusCodePagesWithReExecute("/error/{0}");

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseStaticFiles();

            app.UseCors(_corsPolicy);

            app.UseAuthorization();

            app.UseSwaggerDocumentation(_config);

            app.UseEndpoints(endpoints => {endpoints.MapControllers();} );
        }
    }
}
