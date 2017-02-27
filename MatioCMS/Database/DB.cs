using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using MatioCMS.Includes.Models;

namespace MatioCMS.Database
{
    /// <summary>
    /// Database Connection for anonymous use (readonly)
    /// </summary>
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
            FileStream file = new FileStream(Path.Combine(rootpath, "/Database/ConnectionString.json"), FileMode.Open, FileAccess.Read);
            string config = new StreamReader(file).ReadToEnd();
            if(string.IsNullOrWhiteSpace(config))
            optionsBuilder.UseSqlServer(config);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("matiocms");

            // Config
            modelBuilder.Entity<Config>().Property("Name").IsUnicode(false);
            modelBuilder.Entity<Config>().Property("Value").IsUnicode(false);

            // Admins
            modelBuilder.Entity<Admin>().Property("Username").IsUnicode(false);

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
            modelBuilder.Entity<>()
        }

        #region Properties
            public IEnumerable<Admin> Admins { get; set; }
            public IEnumerable<Config> Config { get; set; }
            public IEnumerable<Plugin> Plugins { get; set; }
            public IEnumerable<Theme> Themes { get; set; }
            public DbSet<Error> Errors { get; set; }
            public IEnumerable<Category> Categories { get; set; }
            public IEnumerable<Tag> Tags { get; set; }
            public IEnumerable<Widget> Widgets { get; set; }
            public IEnumerable<Menu> Menus { get; set; }
            public IEnumerable<Link> Links { get; set; }
            public IEnumerable<Gallery> Gallery { get; set; }
        #endregion

        #region Methods
            public void UpdateStatistics()
            { this.Database.ExecuteSqlCommand("[matiocms].UpdateStatistics"); }
        #endregion
    }
}
