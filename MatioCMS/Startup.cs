using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
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
                app.UseExceptionHandler("/Error/InternalServer");
            }
            app.UseStatusCodePagesWithReExecute("/Error/?status={0}");
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = env.ContentRootFileProvider,
                RequestPath = new Microsoft.AspNetCore.Http.PathString("/static")
            });

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
                    template: "page/{*arguments}", // example: page/name1/name2/name3/1
                    defaults: new { controller = "Content", action = "Page" });

                // Post
                routes.MapRoute(
                    name: "Post",
                    template: "news/{name}/{pagenumber?}",
                    defaults: new { controller = "Content", action = "Post" });

                // Archive
                routes.MapRoute(
                    name: "Archive-y",
                    template: @"archive/{year:regex([12]\d{3})}",
                    defaults: new { controller = "Content", action = "Archive" }
                    );
                routes.MapRoute(
                    name: "Archive-y-pagenumber",
                    template: @"archive/{year:regex([12]\d{3})}/page/{pagenumber:ushort}",
                    defaults: new { controller = "Content", action = "Archive" }
                    );
                routes.MapRoute(
                    name: "Archive-ym",
                    template: @"archive/{year:regex([12]\d{3})}/{month:regex([01]\d)}",
                    defaults: new { controller = "Content", action = "Archive" }
                    );
                routes.MapRoute(
                    name: "Archive-ym-pagenumber",
                    template: @"archive/{year:regex([12]\d{3})}/{month:regex([01]\d)}/page/{pagenumber:ushort}",
                    defaults: new { controller = "Content", action = "Archive" }
                    );
                routes.MapRoute(
                    name: "Archive-ymd",
                    template: @"archive/{year:regex([12]\d{3})}/{month:regex([01]\d)}/{day:regex([0-3]\d)}",
                    defaults: new { controller = "Content", action = "Archive" }
                    );
                routes.MapRoute(
                    name: "Archive-ymd-pagenumber",
                    template: @"archive/{year:regex([12]\d{3})}/{month:regex([01]\d)}/{day:regex([0-3]\d)}/page/{pagenumber:ushort}",
                    defaults: new { controller = "Content", action = "Archive" }
                    );

                // Category
                routes.MapRoute(
                    name: "Category",
                    template: "category/{name}/{pagenumber?}",
                    defaults: new { controller = "Content", action = "Category" }
                    );

                // Tag
                routes.MapRoute(
                    name: "Tag",
                    template: "tag/{name}/{pagenumber?}",
                    defaults: new { controller = "Content", action = "Tag" }
                    );

                // Taxonomy
                routes.MapRoute(
                    name: "Taxonomy",
                    template: "tax-{taxonomy}/",
                    defaults: new { controller = "Content", action = "Taxonomy" }
                    );

                // Term
                routes.MapRoute(
                    name: "Term",
                    template: "tax-{taxonomy}/{term}/{pagenumber?}",
                    defaults: new { controller = "Content", action = "Term" }
                    );

                // Extension
                routes.MapRoute(
                    name: "Extension",
                    template: "ext/{name}",
                    defaults: new { controller = "Content", action = "Extension" }
                    );

                // Error: Not Found
                routes.MapRoute(
                    name: "Error-NotFound",
                    template: "error/notfound",
                    defaults: new { controller = "Error", action = "NotFound" }
                    );

                // Error: Internal Server Error
                routes.MapRoute(
                    name: "Error-InternalServer",
                    template: "error/internalserver",
                    defaults: new { controller = "Error", action = "InternalServer" }
                    );
            });
        }
    }
}
