using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MatioCMS.Areas.Admin.Includes
{
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

        [ForeignKey("Username")]
        public Admin Admin { get; set; }
    }
    public enum ObjectTypes { Admin, Page, Post, Category, Tag, Snippet, Link, Menu, Theme, Plugin, Gallery }
    public enum Action { Default, Add, Delete, Edit, Personalize, Publish, Install }
}
