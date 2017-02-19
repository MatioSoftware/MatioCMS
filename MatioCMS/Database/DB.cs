﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace MatioCMS.Database
{
    /// <summary>
    /// Database Connection for anonymous use (readonly)
    /// </summary>
    public sealed class DB : DbContext
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

        #region Properties

            

        #endregion
    }
}