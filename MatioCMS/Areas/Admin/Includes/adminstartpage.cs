using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MatioCMS.Areas.Admin.Includes
{
    [Table("adminstartpages")]
    public class adminstartpage
    {
        [Key,ConcurrencyCheck,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [Required]
        public string Username { get; set; }
        [DefaultValue("Untitled")]
        public string Title { get; set; }
        [Required]
        public string WidgetName { get; set; }
        [DefaultValue(1), Range(1, 20)]
        public short Column { get; set; }
        [DefaultValue(1), Range(1, 20)]
        public short Row { get; set; }
        public string Settings { get; set; }

        [ForeignKey("Username")]
        public Admin Admin { get; set; }
    }
}
