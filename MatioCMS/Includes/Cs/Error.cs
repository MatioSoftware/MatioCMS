using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MatioCMS.Includes
{
    [Table("Errors")]
    public class Error
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [Required]
        public string ExceptionName { get; set; }
        [Required, DataType(DataType.Url)]
        public string FileName { get; set; }
        [DefaultValue(0)]
        public ushort Line { get; set; }
        public string Message { get; set; }
        [Required]
        public string StackTrace { get; set; }
    }
}
