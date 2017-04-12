using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;

namespace MatioCMS
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
            services.AddMvc();

            FileStream file = new FileStream(Path.Combine(Directory.GetCurrentDirectory(), "Database/ConnectionString.config"), FileMode.Open, FileAccess.Read);
            string config = new StreamReader(file).ReadToEnd();
            if (!string.IsNullOrWhiteSpace(config))
                services.AddDbContext<Database.DB>(options => options.UseSqlServer(config));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            // Exception Viewing
            if (env.IsDevelopment() ||
                File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "/Content/Themes/.debugmode")) ||
                File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "/Content/Plugins/.debugmode")))
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Error/InternalServer");
            }
            // While HTTP Error
            app.UseStatusCodePagesWithReExecute("/Error/?status={0}");

            // Access to static files on server
            app.UseStaticFiles(new StaticFileOptions
            { // static/includes/
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "includes/lib")),
                RequestPath = new Microsoft.AspNetCore.Http.PathString("/static/includes"),
                ContentTypeProvider = new StaticContentTypeProvider(),
                ServeUnknownFileTypes = false
            });
            app.UseStaticFiles(new StaticFileOptions
            { // static/schemas/
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "includes/schemas")),
                RequestPath = new Microsoft.AspNetCore.Http.PathString("/static/schemas"),
                ContentTypeProvider = new StaticContentTypeProvider(),
                ServeUnknownFileTypes = false
            });
            app.UseStaticFiles(new StaticFileOptions
            { // static/gallery/
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "content/gallery")),
                RequestPath = new Microsoft.AspNetCore.Http.PathString("/gallery"),
                ContentTypeProvider = new StaticContentTypeProvider(),
                ServeUnknownFileTypes = false
            });
            app.UseStaticFiles(new StaticFileOptions
            { // static/plugins/
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "content/plugins")),
                RequestPath = new Microsoft.AspNetCore.Http.PathString("/static/plugins"),
                ContentTypeProvider = new StaticContentTypeProvider(),
                ServeUnknownFileTypes = false
            });
            app.UseStaticFiles(new StaticFileOptions
            { // static/themes/
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "content/themes")),
                RequestPath = new Microsoft.AspNetCore.Http.PathString("/static/themes"),
                ContentTypeProvider = new StaticContentTypeProvider(),
                ServeUnknownFileTypes = false
            });

            // Routing to contents
            app.UseMvc(routes =>
            {
                // Main page
                routes.MapRoute(
                    name: "Home",
                    template: string.Empty,
                    defaults: new { controller = "Content", action = "Index" });

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
                    template: @"archive/{year:regex(^[12]\d\d\d$)}",
                    defaults: new { controller = "Content", action = "Archive" }
                    );
                routes.MapRoute(
                    name: "Archive-y-pagenumber",
                    template: @"archive/{year:regex(^[12]\d\d\d$)}/page/{pagenumber:regex(^\d+$)}",
                    defaults: new { controller = "Content", action = "Archive" }
                    );
                routes.MapRoute(
                    name: "Archive-ym",
                    template: @"archive/{year:regex(^[12]\d\d\d$)}/{month:regex(^[01]\d$)}",
                    defaults: new { controller = "Content", action = "Archive" }
                    );
                routes.MapRoute(
                    name: "Archive-ym-pagenumber",
                    template: @"archive/{year:regex(^[12]\d\d\d$)}/{month:regex(^[01]\d$)}/page/{pagenumber:regex(^\d+$)}",
                    defaults: new { controller = "Content", action = "Archive" }
                    );
                routes.MapRoute(
                    name: "Archive-ymd",
                    template: @"archive/{year:regex(^[12]\d\d\d$)}/{month:regex(^[01]\d$)}/{day:regex(^[0-3]\d$)}",
                    defaults: new { controller = "Content", action = "Archive" }
                    );
                routes.MapRoute(
                    name: "Archive-ymd-pagenumber",
                    template: @"archive/{year:regex(^[12]\d\d\d$)}/{month:regex(^[01]\d$)}/{day:regex(^[0-3]\d$)}/page/{pagenumber:regex(^\d+$)}",
                    defaults: new { controller = "Content", action = "Archive" }
                    );

                // Category
                routes.MapRoute(
                    name: "Category",
                    template: "category/{*arguments}",
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

        public class StaticContentTypeProvider : FileExtensionContentTypeProvider
        {
            public StaticContentTypeProvider()
            {
                // List of new known file types (server have default list of known file types)

                /* Style Sheets */
                Mappings.Add(".less", "text/css");
                
                /* Documents */
                Mappings.Add(".odt", "application/vnd.oasis.opendocument.text");
                Mappings.Add(".odp", "application/vnd.oasis.opendocument.presentation");
                Mappings.Remove(".ods");
                Mappings.Add(".ods", "application/vnd.oasis.opendocument.spreadsheet");
                Mappings.Add(".odb", "application/vnd.oasis.opendocument.database");

                /* Images */
                Mappings.Remove(".psd");
                Mappings.Add(".psd", "image/vnd.adobe.photoshop");
                
                /* Archives */
                Mappings.Add(".7z", "application/x-7z-compressed");

                /* Binary */
                Mappings.Add(".dll", "application/octet-stream");
                Mappings.Add(".apk", "application/vnd.android.package-archive");
                Mappings.Add(".dmg", "application/x-apple-diskimage");
                Mappings.Add(".iso", "application/x-iso9660-image");
            }
        }
    }
}
