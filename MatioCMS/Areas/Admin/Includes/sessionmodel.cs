using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MatioCMS.Areas.Admin.Includes
{
    public class sessionmodel
    {
        [Key,Required]
        public Guid ID { get; set; }
        public DateTime TimeAdded { get; set; }
        public string Data { get; set; }
    }
}
