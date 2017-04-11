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

namespace AspCoreDemo.Middleware.ShortCircuit
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
            // Add framework services.
            services.AddMvc();
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

            //Middleware component 2
            app.Use(async (context, next) =>
            {
                await context.Response.WriteAsync("Middleware Component 2<br/>");
                await next.Invoke();
                await context.Response.WriteAsync("Middleware Component 2<br/>");
            });

            /*
            * ---------------------------------------------------------------------------------
            * Short Circuit! because call Run() earlier in the pipeline
            * ---------------------------------------------------------------------------------
            */
            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Running... Middleware Component 3 never runs<br/>");
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
                await context.Response.WriteAsync("Running...<br/>");
            });
        }
    }
}
