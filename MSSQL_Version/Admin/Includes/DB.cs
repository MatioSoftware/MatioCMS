using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MatioCMS.Database;
using MatioCMS.Includes.Models;
using MatioCMS.Admin.Includes.Models;

namespace MatioCMS.Admin.Includes
{
    public class DB : Database.DB
    {
        public DB(string Username, string Passkey, string RootPath, Translation translation) : base(RootPath)
        {
            if (this.Admins.Where(user => user.Username == Username && user.Passkey == Passkey).Count() != 1)
                throw new AdminLoginException(translation.GetMessage("Admin", 0));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("matiocms");

            // Config
            modelBuilder.Entity<Config>().Property("Name").IsUnicode(false);
            modelBuilder.Entity<Config>().Property("Value").IsUnicode(false);
            // Admins
            modelBuilder.Entity<Models.Admin>().Property("Username").IsUnicode(false);
            modelBuilder.Entity<Models.Admin>().Property("Passkey").IsUnicode(false);

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

            // Logs
            modelBuilder.Entity<Log>().Property("Username").IsUnicode(false);
            modelBuilder.Entity<Log>().Property("ObjectType").IsUnicode(false);
            modelBuilder.Entity<Log>().Property("Action").IsUnicode(false);
            modelBuilder.Entity<Log>().HasOne(item => item.User).WithMany(item => item.Logs).HasForeignKey(item => item.Username);

            // Gallery
            modelBuilder.Entity<Gallery>().Property("Extension").IsUnicode(false);
            modelBuilder.Entity<Gallery>().HasOne(item => item.Author).WithMany(item => item.AddedGalleryItems).HasForeignKey(item=> item.AuthorUsername);

            // Page
            modelBuilder.Entity<Page>().Property("Name").IsUnicode(false);
            modelBuilder.Entity<Page>().HasOne(item => item.CreatedBy).WithMany(item => item.CreatedPages).HasForeignKey(item=> item.CreatedBy_Username);

            // Page Changes
            modelBuilder.Entity<PageChange>().HasOne(item => item.EditedBy).WithMany(item => item.PageChanges).HasForeignKey(item=> item.EditedBy_Username);
            modelBuilder.Entity<PageChange>().HasOne(item => item.Page).WithMany(item => item.Changes).HasForeignKey(item=> item.PageID);

            
        }

        #region Tables
            #region Configuration
                public new DbSet<Models.Admin> Admins { get; set; }
                public DbSet<sessionmodel> Sessions { get; set; }
                public new DbSet<Config> Config { get; set; }
                public new DbSet<Plugin> Plugins { get; set; }
                public new DbSet<Theme> Themes { get; set; }
                public DbSet<Log> Logs { get; set; }
                public new DbSet<Stat>
            #endregion

            #region Content
                public new DbSet<Category> Categories { get; set; }
                public new DbSet<Tag> Tags { get; set; }
                public new DbSet<Widget> Widgets { get; set; }
                public new DbSet<Gallery> Gallery { get; set; }

                public DbSet<Page> Pages { get; set; }
                public DbSet<PageChange> PageChanges { get; set; }


            #endregion
        #endregion
    }
}
