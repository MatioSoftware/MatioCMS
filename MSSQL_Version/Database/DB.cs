using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.IO;
using MatioCMS.Includes.Models;
using System.Data.SqlClient;

namespace MatioCMS.Database
{
    public class DB : DbContext
    {
        /// <param name="RootPath">Webpage root path</param>
        public DB(string RootPath)
        {
            if (string.IsNullOrWhiteSpace(RootPath))
                throw new ArgumentException();
            if (!Directory.Exists(RootPath))
                throw new FileNotFoundException();
            rootpath = RootPath;
        }

        private string rootpath;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            FileStream file = new FileStream(Path.Combine(rootpath, "/Database/ConnectionString.config"), FileMode.Open, FileAccess.Read);
            string config = new StreamReader(file).ReadToEnd();
            if (string.IsNullOrWhiteSpace(config))
                optionsBuilder.UseSqlServer(config);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("matiocms");

            // Readonly data
            modelBuilder.Ignore<PublishedPage>();
            modelBuilder.Ignore<PublishedPost>();

            // Config
            modelBuilder.Entity<Config>().Property("Name").IsUnicode(false);
            modelBuilder.Entity<Config>().Property("Value").IsUnicode(false);

            // Admins
            modelBuilder.Entity<Includes.Models.Admin>().Property("Username").IsUnicode(false);

            // Errors
            modelBuilder.Entity<Error>().Property("ExceptionName").IsUnicode(false);
            modelBuilder.Entity<Error>().Property("Filename").IsUnicode(false);

            // Tags
            modelBuilder.Entity<Tag>().HasIndex("Name");
            modelBuilder.Entity<Tag>().Property("Name").IsUnicode(false);

            // Categories
            modelBuilder.Entity<Category>().HasIndex("Name");
            modelBuilder.Entity<Category>().Property("Name").IsUnicode(false);
            modelBuilder.Entity<Category>().HasOne(item => item.Parent).WithMany(item => item.Children).HasForeignKey(item => item.ParentID);

            // Plugins
            modelBuilder.Entity<Plugin>().Property("Name").IsUnicode(false);

            // Themes
            modelBuilder.Entity<Theme>().Property("Name").IsUnicode(false);

            // Widgets
            modelBuilder.Entity<Widget>().Property("WidgetName").IsUnicode(false);
            modelBuilder.Entity<Widget>().Property("AreaName").IsUnicode(false);

            // Menus
            modelBuilder.Entity<Menu>().Property("Name").IsUnicode(false);
            modelBuilder.Entity<Menu>().Property("AreaName").IsUnicode(false);

            // Links
            modelBuilder.Entity<Link>().Property("URL").IsUnicode(false);

            // Gallery
            modelBuilder.Entity<Gallery>().Property("Extension").IsUnicode(false);
            modelBuilder.Entity<Gallery>().Property("AuthorUsername").IsUnicode(false);
            modelBuilder.Entity<Gallery>().HasOne(item => item.Author).WithMany(item => item.AddedGalleryItems).HasForeignKey(item => item.AuthorUsername);
        }

        #region Tables
            #region Configuration
                public IEnumerable<Includes.Models.Admin> Admins { get; set; }
                public IEnumerable<Config> Config { get; set; }
                public IEnumerable<Plugin> Plugins { get; set; }
                public IEnumerable<Theme> Themes { get; set; }
            #endregion

            #region Content
                public IQueryable<Category> Categories { get; set; }
                public IQueryable<Tag> Tags { get; set; }
                public IQueryable<Widget> Widgets { get; set; }
                public IQueryable<Menu> Menus { get; set; }
                public IQueryable<Snippet> Snippets { get; set; }
                public IQueryable<Link> Links { get; set; }
                public IQueryable<Gallery> Gallery { get; set; }
                private DbSet<PublishedPage> publishedpages { get; set; }
                public IQueryable<PublishedPage> PublishedPages => this.publishedpages.FromSql(@"SELECT * FROM [GetPublishedPages]() ORDER BY [DatePublished] DESC");
                private DbSet<PublishedPost> publishedposts { get; set; }
                public IQueryable<PublishedPost> PublishedPosts => this.publishedposts.FromSql(@"SELECT * FROM [GetPublishedPosts]() ORDER BY [DatePublished] DESC");
            #endregion
        #endregion

        #region Methods
            public void UpdateStatistics()
            { this.Database.ExecuteSqlCommandAsync("exec [matiocms].UpdateStatistics"); }
            public void AddError(Error error)
            {
                this.Database.ExecuteSqlCommand("exec [matiocms].AddError @exceptionname, @filename, @line, @message, @stacktrace, @dateadded",
                    new SqlParameter("@exceptionname", error.ExceptionName),
                    new SqlParameter("@filename", error.Filename),
                    new SqlParameter("@line", error.Line),
                    new SqlParameter("@message", error.Message),
                    new SqlParameter("@stacktrace", error.StackTrace),
                    new SqlParameter("@dateadded", error.DateAdded));
            }
        #endregion
    }
}
