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
            FileStream json = new FileStream(Path.Combine(rootpath, "/Database/ConnectionString.json"), FileMode.Open, FileAccess.Read);
            optionsBuilder.UseSqlServer(JsonConvert.DeserializeObject<string>(new StreamReader(json).ReadToEnd()));
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
        }

        #region Properties
            public IEnumerable<Admin> Admins { get; private set; }
            public IEnumerable<Config> Config { get; private set; }
            public DbSet<Stat> Statistics { get; set; }
            public IEnumerable<Plugin> Plugins { get; private set; }
            public IEnumerable<Theme> Themes { get; private set; }
            public DbSet<Error> Errors { get; set; }
            public IEnumerable<Category> Categories { get; private set; }
            public IEnumerable<Tag> Tags { get; private set; }
        #endregion
    }
}
