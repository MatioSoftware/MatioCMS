using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MatioCMS.Includes.Models
{
    #region Required_for_runtime
        /// <summary>
        /// Public admin-user model
        /// </summary>
        [Table("Admins")]
        public class Admin
        {
            [Key, Required, ConcurrencyCheck, MaxLength(35)]
            public string Username { get; set; }
            [Required]
            public string Fullname { get; set; }
            public string Description { get; set; }
            public DateTime DateAdded { get; set; } = DateTime.UtcNow;
        }

        [Table("Plugins")]
        public class Plugin
        {
            [Key, Required, ConcurrencyCheck]
            public string Name { get; set; }
            [DefaultValue(false)]
            public bool Enabled { get; set; }
            [DefaultValue(false)]
            public bool Locked { get; set; }
            public DateTime DateAdded { get; set; } = DateTime.UtcNow;
            public string Settings { get; set; }
        }

        [Table("Themes")]
        public class Theme
        {
            [Key,Required]
            public string Name { get; set; }
            [Required]
            public string Title { get; set; }
            public DateTime DateAdded { get; set; } = DateTime.UtcNow;
            public DateTime DateModified { get; set; } = DateTime.UtcNow;
            public string Settings { get; set; }
    }

        [Table("Errors")]
        public class Error
        {
            [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int ID { get; set; }
            [Required]
            public string ExceptionName { get; set; }
            [Required, DataType(DataType.Url)]
            public string Filename { get; set; }
            [DefaultValue(0)]
            public ushort Line { get; set; }
            public string Message { get; set; }
            [Required]
            public string StackTrace { get; set; }
        }

        [Table("Statistics")]
        public class Stat
        {
            [Key, DataType(DataType.Date), ConcurrencyCheck]
            public DateTime Date { get; set; } = DateTime.UtcNow.Date;
            [Range(0, int.MaxValue), DefaultValue(0)]
            public int Views { get; set; }
        }
    #endregion

    #region Content
    [Table("Categories")]
        public class Category
        {
            [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int ID { get; set; }
            [Required, MaxLength(35)]
            public string Name { get; set; }
            [Required, MaxLength(128)]
            public string Title { get; set; }
            public DateTime DateAdded { get; set; } = DateTime.UtcNow;
            public DateTime DateModified { get; set; } = DateTime.UtcNow;
            [DefaultValue(null)]
            public int ParentID { get; set; }
            public string Description { get; set; }
            [Range(0, long.MaxValue), DefaultValue(0)]
            public long Views { get; set; }

            public Category Parent { get; set; }
            public IEnumerable<Category> Children { get; set; }
        }

    [Table("Tags")]
    public class Tag
    {
        [Key, ConcurrencyCheck, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Title { get; set; }
        public DateTime DateAdded { get; set; } = DateTime.UtcNow;
        [Range(0, long.MaxValue), DefaultValue(0)]
        public long Views { get; set; }
    }

    [Table("Widgets")]
    public class Widget
    {

    }
    #endregion
}
