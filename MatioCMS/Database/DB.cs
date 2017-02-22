using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using MatioCMS.Includes;

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
        }

        #region Properties
            public IEnumerable<Admin> Admins { get; private set; }
            public IEnumerable<Config> Config { get; private set; }
            public DbSet<Stat> Statistics { get; set; }
            public IEnumerable<Plugin> Plugins { get; private set; }
            public 
            
        #endregion
    }
}
