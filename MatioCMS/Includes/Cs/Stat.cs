using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MatioCMS.Includes
{
    [Table("Statistics")]
    public class Stat
    {
        [Key, DataType(DataType.Date),ConcurrencyCheck]
        public DateTime Date { get; set; } = DateTime.UtcNow.Date;
        [DefaultValue(0)]
        public uint Views { get; set; }
    }
}
