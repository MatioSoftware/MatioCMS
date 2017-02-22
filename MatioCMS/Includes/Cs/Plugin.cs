using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MatioCMS.Includes
{
    [Table("Plugins")]
    public class Plugin
    {
        [Key,Required,RegularExpression(@"A-z0-9_.-"),ConcurrencyCheck]
        public string Name { get; set; }
        [DefaultValue(false)]
        public bool Enabled { get; set; }
        public DateTime DateAdded { get; set; } = DateTime.UtcNow;
        public string Settings { get; set; }
    }
}
