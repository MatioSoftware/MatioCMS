using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MatioCMS.Includes
{
    public class Plugin
    {
        public string Name { get; set; }
        public bool Enabled { get; set; }
        public DateTime DateAdded { get; set; }
        public string Settings { get; set; }
    }
}
