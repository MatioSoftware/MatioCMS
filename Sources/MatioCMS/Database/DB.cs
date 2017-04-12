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
        public DB(DbContextOptions<DB> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("matiocms");

            // VIEWS
            modelBuilder.Ignore<PublishedPage>();
            modelBuilder.Ignore<PublishedPost>();

            /* CONFIGURATION */

            // Config
            modelBuilder.Entity<Config>().Property("Name").IsUnicode(false);

            // Admins
            modelBuilder.Entity<Includes.Models.Admin>().Property("Username").IsUnicode(false);

            // Plugins
            modelBuilder.Entity<Plugin>().Property("Name").IsUnicode(false);

            // Themes
            modelBuilder.Entity<Theme>().Property("Name").IsUnicode(false);

            /* CONTENT */

            // Tags
            modelBuilder.Entity<Tag>().HasIndex("Name");
            modelBuilder.Entity<Tag>().Property("Name").IsUnicode(false);

            // Categories
            modelBuilder.Entity<Category>().HasIndex("Name");
            modelBuilder.Entity<Category>().Property("Name").IsUnicode(false);
            modelBuilder.Entity<Category>().HasOne(item => item.Parent).WithMany(item => item.Children).HasForeignKey(item => item.ParentID);

            // Widgets
            modelBuilder.Entity<Widget>().Property("WidgetName").IsUnicode(false);
            modelBuilder.Entity<Widget>().Property("AreaName").IsUnicode(false);

            // Menus
            modelBuilder.Entity<Menu>().Property("Name").IsUnicode(false);
            modelBuilder.Entity<Menu>().Property("AreaName").IsUnicode(false);

            // Links
            modelBuilder.Entity<Link>().Property("URL").IsUnicode(false);

            // Snippets
            modelBuilder.Entity<Snippet>().Property("Platform").IsUnicode(false);
            modelBuilder.Entity<Snippet>().Property("ObjectType").IsUnicode(false);

            // Gallery
            modelBuilder.Entity<Gallery>().Property("Path").IsUnicode(false);
            modelBuilder.Entity<Gallery>().Property("Extension").IsUnicode(false);
            modelBuilder.Entity<Gallery>().HasOne(item => item.Author).WithMany(item => item.AddedGalleryItems).HasForeignKey(item => item.AuthorUsername);
        }

        #region Tables
            #region Configuration
                public IQueryable<Includes.Models.Admin> Admins { get; set; }
                public IQueryable<Config> Config { get; set; }
                public IQueryable<Plugin> Plugins { get; set; }
                public IQueryable<Theme> Themes { get; set; }
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
