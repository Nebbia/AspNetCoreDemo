using System;
using System.Collections.Generic;
using System.Linq;
using AspCoreDemo.CustomMiddleware;
using AspCoreDemo.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AspCoreDemo
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
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            //Middleware component 1
            app.Use(async (context, next) =>
            {
                await context.Response.WriteAsync("Middleware Component 1<br/>");
                await next.Invoke();
                await context.Response.WriteAsync("Middleware Component 1<br/>");
            });

            //Middleware component 2
            app.Use(async (context, next) =>
            {
                await context.Response.WriteAsync("Middleware Component 2<br/>");
                await next.Invoke();
                await context.Response.WriteAsync("Middleware Component 2<br/>");
            });

            //Middleware component 3
            app.Use(async (context, next) =>
            {
                await context.Response.WriteAsync("Middleware Component 3<br/>");
                await next.Invoke();
                await context.Response.WriteAsync("Middleware Component 3<br/>");
            });

            //App Run
            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Running...");
            });
        }
        
        // For Staging Environment
        public void ConfigureStaging(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            // Short Circuits the Pipeline
            app.Run(async context =>
            {
                await context.Response.WriteAsync("Completely different pipeline for staging!<br/>");
            });
        }
    }
}
