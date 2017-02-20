using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatioCMS.Includes
{
    public class Admin
    {
        public string Username { get; set; }
        public string Passkey { get; set; }
        public string Fullname { get; set; }
        public string Description { get; set; }
        public DateTime DateAdded { get; set; }
        public short Role { get; set; }
    }
}
