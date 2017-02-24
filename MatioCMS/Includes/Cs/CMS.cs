using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using MatioCMS.Includes;
using MatioCMS.Database;

namespace MatioCMS
{
    /// <summary>
    /// Main class in the Matio CMS
    /// </summary>
    public class CMS
    {
        public CMS(DB Database)
        {
            if (Database == null)
                throw new ArgumentNullException();
            this.DB = Database;

            // Loading plugins
        }

        #region Fields
            private DB DB;
            private Library library;
            private memory mem;
        #endregion

        #region Classes
            private class memory
            {
                public List<Func<string, string>> headfilters { get; set; } = new List<Func<string, string>>();

                public List<Func<CMS, Microsoft.AspNetCore.Mvc.ContentResult>> extensions = new List<Func<CMS, Microsoft.AspNetCore.Mvc.ContentResult>>();

                public List<Func<string, string>> contentfilters = new List<Func<string, string>>();

                
            }
        #endregion
    }

    public class Config
    {
        [Key, Required, MaxLength(35)]
        public string Name { get; set; }
        public string Value { get; set; }
    }


}
