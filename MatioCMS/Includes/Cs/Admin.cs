using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MatioCMS.Includes
{
    [Table("Admins")]
    public class Admin
    {
        [Key,Required,ConcurrencyCheck]
        public string Username { get; set; }
        [Required]
        public string Fullname { get; set; }
        public string Description { get; set; }
        public DateTime DateAdded { get; set; } = DateTime.UtcNow;
    }
}
