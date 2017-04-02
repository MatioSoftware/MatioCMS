using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MatioCMS.Includes.Models;
using MatioCMS.Admin.Includes.Models;

namespace MatioCMS.Admin.Includes
{
    public enum AdminRole
    {
        SuperAdmin = 0, // Root
        Administrator = 1, // Full Admin
        Editor = 2, // Editing and publishing all pages, posts, categories and tags
        Author = 3, // Editing and publishing their own pages and posts
        Contributor = 4 // Editing their own posts and save as draft
    }

    public enum ObjectTypes { Admin, Page, Post, Category, Tag, Snippet, Link, Menu, Theme, Plugin, Gallery }

    public enum Actions { Default, Add, Delete, Edit, Personalize, Publish, Install }

    namespace Models
    {
        [Table("Admins")]
        public class Admin
        {
            [Key, Required, StringLength(35, MinimumLength = 5)]
            public string Username { get; set; }
            [Required, RegularExpression("^[0-9A-f]{64}$")]
            public string Passkey { get; set; }
            [Required, RegularExpression(@"^\w+.*"), MaxLength(128)]
            public string Fullname { get; set; }
            public string Description { get; set; }
            public DateTime DateAdded { get; set; } = DateTime.UtcNow;
            [Required]
            public AdminRole Role { get; set; }
            public string StartPage { get; set; }

            public IEnumerable<Log> Logs { get; set; }
            public IEnumerable<Gallery> AddedGalleryItems { get; set; }
            public IEnumerable<Page> CreatedPages { get; set; }
            public IEnumerable<PageChange> PageChanges { get; set; }

        }

        [Table("Logs")]
        public class Log
        {
            [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int ID { get; set; }
            [Required]
            public string Username { get; set; }
            [DataType(DataType.DateTime)]
            public DateTime ActionDate { get; set; } = DateTime.UtcNow;
            [Required, EnumDataType(typeof(ObjectTypes))]
            public string ObjectType { get; set; }
            [Required, EnumDataType(typeof(Action))]
            public string Action { get; set; }
            public string Description { get; set; }

            public Admin Admin { get; set; }
        }      

        [Table("Pages")]
        public class Page
        {
            [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public long ID { get; set; }
            [Required, MaxLength(50)]
            public string Name { get; set; }
            [MaxLength(128)]
            public string Categories { get; set; }
            [MaxLength(128)]
            public string Tags { get; set; }
            [Required, MaxLength(35)]
            public string CreatedBy_Username { get; set; }
            public DateTime DateAdded { get; set; } = DateTime.UtcNow;
            [Range(0, long.MaxValue), DefaultValue(0)]
            public long Views { get; set; }

            public Admin CreatedBy { get; set; }
            public PageChange Changes { get; set; }
        }

        [Table("PageChanges")]
        public class PageChange
        {
            [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public long ID { get; set; }
            [Required]
            public long PageID { get; set; }
            [Required, MaxLength(256)]
            public string Title { get; set; }
            [Required, MaxLength(35)]
            public string EditedBy_Username { get; set; }
            public string Content { get; set; }
            public string TextContent { get; set; }
            public DateTime DateModified { get; set; } = DateTime.UtcNow;
            [DefaultValue(false)]
            public bool IsPublished { get; set; }
            public DateTime PublishDate { get; set; } = DateTime.UtcNow;

            public Admin EditedBy { get; set; }
            public Page Page { get; set; }
        }

        [Table("Sessions")]
        public class sessionmodel
        {
            [Key, Required]
            public Guid ID { get; set; }
            public DateTime TimeAdded { get; set; } = DateTime.UtcNow;
            public string Data { get; set; }
        }
    }

}
