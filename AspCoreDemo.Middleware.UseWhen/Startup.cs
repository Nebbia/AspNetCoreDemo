using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AspCoreDemo.Middleware.UseWhen
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            //Middleware component 1
            app.Use(async (context, next) =>
            {
                await context.Response.WriteAsync("Middleware Component 1<br/>");
                await next.Invoke();
                await context.Response.WriteAsync("Middleware Component 1<br/>");
            });

            app.UseWhen(x => x.Request.Path.Value.Contains("/branched-middleware"), appBuilder =>
            {
                appBuilder.Use(async (context, next) =>
                {
                    await context.Response.WriteAsync("Branch! Middleware Component 2<br/>");
                    await next.Invoke();
                    await context.Response.WriteAsync("Branch! Middleware Component 1<br/>");
                });

                //Don't short circuit
            });

            //App Run
            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Running... This gets called even from /branched-middleware<br/>");
            });
        }
    }
}
