using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MatioCMS
{
    /// <summary>
    /// Klasa startowa aplikacji ASP.NET Core
    /// </summary>
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
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
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                // Main page
                routes.MapRoute(
                    name: "Home",
                    template: string.Empty,
                    defaults: new { controller = "Content", action = "Index"});

                // Page
                routes.MapRoute(
                    name: "Page",
                    template: "page/{name}",
                    defaults: new { controller = "Content", action = "Page" });
                routes.MapRoute(
                    name: "Page-param1",
                    template: "page/{name}/{param1}",
                    defaults: new { controller = "Content", action = "Page" });
                routes.MapRoute(
                    name: "Page-param2",
                    template: "page/{name}/{param1}/{param2}",
                    defaults: new { controller = "Content", action = "Page" });
                routes.MapRoute(
                    name: "Page-param3",
                    template: "page/{name}/{param1}/{param2}/{param3}",
                    defaults: new { controller = "Content", action = "Page" });
                routes.MapRoute(
                    name: "Page-param4",
                    template: "page/{name}/{param1}/{param2}/{param3}/{param4}",
                    defaults: new { controller = "Content", action = "Page" });
                routes.MapRoute(
                    name: "Page-param5",
                    template: "page/{name}/{param1}/{param2}/{param3}/{param4}/{param5}",
                    defaults: new { controller = "Content", action = "Page" });

                // Post
                routes.MapRoute(
                    name: "Post",
                    template: "news/{name}",
                    defaults: new { controller = "Content", action = "Post" });
                routes.MapRoute(
                    name: "Post-param1",
                    template: "post/{name}/{pagenumber:uint}",
                    defaults: new { controller = "Content", action = "Post" });

                // Archive
                routes.MapRoute(
                    name: "Archive-y",
                    template: @"archive/{year:regex([12]\d{3})}",
                    defaults: new { controller = "Content", action = "Archive" }
                    );
                routes.MapRoute(
                    name: "Archive-ym",
                    template: @"archive/{year:regex([12]\d{3})}/{month:regex([01]\d)}",
                    defaults: new { controller = "Content", action = "Archive" }
                    );
                routes.MapRoute(
                    name: "Archive-ymd",
                    template: @"archive/{year:regex([12]\d{3})}/{month:regex([01]\d)}/{day:regex([0-3]\d)}",
                    defaults: new { controller = "Content", action = "Archive" }
                    );

                // Extension
                routes.MapRoute(
                    name: "Extension",
                    template: "ext/{name}",
                    defaults: new { controller = "Content", action = "Extension" }
                    );


            });
        }
    }
}
