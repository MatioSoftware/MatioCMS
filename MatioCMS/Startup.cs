using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.StaticFiles;
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
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            // Logging
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
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "/includes/lib")),
                RequestPath = new Microsoft.AspNetCore.Http.PathString("/static/includes"),
                ContentTypeProvider = new StaticContentTypeProvider(),
                ServeUnknownFileTypes = false
            }).UseStaticFiles(new StaticFileOptions
            { // static/schemas/
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "/includes/schemas")),
                RequestPath = new Microsoft.AspNetCore.Http.PathString("/static/schemas"),
                ContentTypeProvider = new StaticContentTypeProvider(),
                ServeUnknownFileTypes = false
            }).UseStaticFiles(new StaticFileOptions
            { // static/uploads/
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "/content/uploads")),
                RequestPath = new Microsoft.AspNetCore.Http.PathString("/static/uploads"),
                ContentTypeProvider = new StaticContentTypeProvider(),
                ServeUnknownFileTypes = false
            }).UseStaticFiles(new StaticFileOptions
            { // static/plugins/
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "/content/plugins")),
                RequestPath = new Microsoft.AspNetCore.Http.PathString("/static/plugins"),
                ContentTypeProvider = new StaticContentTypeProvider(),
                ServeUnknownFileTypes = false
            }).UseStaticFiles(new StaticFileOptions
            { // static/themes/
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "/content/themes")),
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
    public class StaticContentTypeProvider : FileExtensionContentTypeProvider
    {
        public StaticContentTypeProvider()
        {
            // List of known file types (unknown types have HTTP Error)
            /* Text files */
            Mappings.Add(".txt", "text/plain");
            Mappings.Add(".appcache", "text/plain");

            /* HTML, XML */
            Mappings.Add(".htm", "text/html");
            Mappings.Add(".html", "text/html");
            Mappings.Add(".xhtml", "application/xhtml+xml");
            Mappings.Add(".xml", "text/xml");
            Mappings.Add(".xsd", "text/xml");

            /* Style Sheets */
            Mappings.Add(".css", "text/css");
            Mappings.Add(".less", "text/css");

            /* JavaScript */
            Mappings.Add(".js", "text/javascript");
            Mappings.Add(".json", "application/json");

            /* Documents */
            Mappings.Add(".pdf", "application/pdf");
            Mappings.Add(".doc", "application/msword");
            Mappings.Add(".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document");
            Mappings.Add(".ppt", "application/vnd.ms-powerpoint");
            Mappings.Add(".pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation");
            Mappings.Add(".xls", "application/vnd.ms-excel");
            Mappings.Add(".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            Mappings.Add(".mdb", "application/vnd.ms-access");
            Mappings.Add(".odt", "application/vnd.oasis.opendocument.text");
            Mappings.Add(".odp", "application/vnd.oasis.opendocument.presentation");
            Mappings.Add(".ods", "application/vnd.oasis.opendocument.spreadsheet");
            Mappings.Add(".odb", "application/vnd.oasis.opendocument.database");

            /* Images */
            Mappings.Add(".bmp", "image/bmp");
            Mappings.Add(".ico", "image/x-icon");
            Mappings.Add(".gif", "image/gif");
            Mappings.Add(".jpg", "image/jpeg");
            Mappings.Add(".jpeg", "image/jpeg");
            Mappings.Add(".png", "image/png");
            Mappings.Add(".psd", "image/vnd.adobe.photoshop");
            Mappings.Add(".svg", "image/svg+xml");
            Mappings.Add(".tif", "image/tiff");
            Mappings.Add(".tiff", "image/tiff");

            /* Audio */
            Mappings.Add(".aif", "audio/aiff");
            Mappings.Add(".aiff", "audio/aiff");
            Mappings.Add(".wav", "audio/wave");
            Mappings.Add(".mp3", "audio/mpeg");
            Mappings.Add(".wma", "audio/x-ms-wma");
            Mappings.Add(".mid", "audio/midi");
            Mappings.Add(".midi", "audio/midi");
            Mappings.Add(".kar", "audio/midi");
            Mappings.Add(".kar", "audio/midi");
            Mappings.Add(".kar", "audio/midi");
            Mappings.Add(".ogg", "audio/ogg");

            /* Video */
            Mappings.Add(".avi", "video/avi");
            Mappings.Add(".mp4", "video/mp4");
            Mappings.Add(".mpg", "video/mpeg");
            Mappings.Add(".mpeg", "video/mpeg");
            Mappings.Add(".mov", "video/quicktime");
            Mappings.Add(".wmv", "video/x-ms-wmv");
            Mappings.Add(".webm", "video/webm");

            /* Fonts */
            Mappings.Add(".ttf", "font/ttf");
            Mappings.Add(".otf", "font/otf");
            Mappings.Add(".woff", "application/font-woff");

            /* Archives */
            Mappings.Add(".zip", "application/zip");
            Mappings.Add(".rar", "application/vnd.rar");
            Mappings.Add(".gz", "application/gzip");
            Mappings.Add(".7z", "application/x-7z-compressed");

            /* Binary */
            Mappings.Add(".class", "application/octet-stream");
            Mappings.Add(".exe", "application/octet-stream");
            Mappings.Add(".dll", "application/octet-stream");
            Mappings.Add(".apk", "application/vnd.android.package-archive");
            Mappings.Add(".dmg", "application/x-apple-diskimage");
            Mappings.Add(".jar", "application/java-archive");
            Mappings.Add(".iso", "application/x-iso9660-image");
        }
    }
}
