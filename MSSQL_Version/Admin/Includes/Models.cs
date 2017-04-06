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

    /* Models only for admins */

    namespace Models
    {
        #region Configuration
            [Table("Admins")]
            public class Admin
            {
                [Key, Required, StringLength(35, MinimumLength = 5)]
                public string Username { get; set; }
                [Required, RegularExpression("^[0-9A-f]{64}$"), MaxLength(64)]
                public string Passkey { get; set; }
                [Required, RegularExpression(@"^\w+.*"), MaxLength(128)]
                public string Fullname { get; set; }
                public string Description { get; set; }
                public DateTime DateAdded { get; set; } = DateTime.UtcNow;
                [Required]
                public AdminRole Role { get; set; }
                public string StartPage { get; set; }

                public virtual ICollection<Log> Logs { get; set; }
                public virtual ICollection<Gallery> AddedGalleryItems { get; set; }
                public virtual ICollection<Page> CreatedPages { get; set; }
                public virtual ICollection<PageChange> PageChanges { get; set; }
                public virtual ICollection<Post> CreatedPosts { get; set; }
                public virtual ICollection<PostChanges> PostChanges { get; set; }
            }

            [Table("Sessions")]
            public class sessionmodel
        {
            [Key, Required]
            public Guid ID { get; set; }
            public DateTime TimeAdded { get; set; } = DateTime.UtcNow;
            public string Data { get; set; }
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
                [Required, EnumDataType(typeof(Actions))]
                public string Action { get; set; }
                public string Description { get; set; }

                public virtual Admin User { get; set; }
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

                public virtual Admin CreatedBy { get; set; }
                public virtual ICollection<PageChange> Changes { get; set; }
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

                public virtual Admin EditedBy { get; set; }
                public virtual Page Page { get; set; }
            }

            [Table("Post")]
            public class Post
            {
                [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
                public long ID { get; set; }
                [Required, StringLength(50, MinimumLength = 3)]
                public string Name { get; set; }
                [Required]
                public string CreatedBy_Username { get; set; }
                public string Categories { get; set; }
                public string Tags { get; set; }
                public DateTime DateAdded { get; set; } = DateTime.UtcNow;
                [DefaultValue(false)]
                public bool Pinned { get; set; }
                [Range(0, long.MaxValue), DefaultValue(0)]
                public long Views { get; set; }

                public virtual Admin CreatedBy { get; set; }
                public virtual ICollection<PostChanges> Changes { get; set; }
            }

            [Table("PostChanges")]
            public class PostChanges
            { 
                [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
                public long ID { get; set; }
                [Required]
                public long PostID { get; set; }
                [Required, StringLength(256, MinimumLength = 3)]
                public string Title { get; set; }
                [Required]
                public string EditedBy_Username { get; set; }
                public string Content { get; set; }
                public string TextContent { get; set; }
                public DateTime DateModified { get; set; } = DateTime.UtcNow;
                [DefaultValue(false)]
                public bool IsPublished { get; set; }
                public DateTime PublishDate { get; set; } = DateTime.UtcNow;

                public virtual Admin EditedBy { get; set; }
                public virtual Post Post { get; set; }
            }
        #endregion
    }

}
