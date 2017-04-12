using MatioCMS.Database;
using MatioCMS.Includes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MatioCMS
{
    public class CMS
    {
        public CMS(DB Database)
        {
            this.DB = Database ?? throw new ArgumentNullException();
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
}
