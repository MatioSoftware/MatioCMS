using System;
using System.Collections.Generic;
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
        #endregion

        #region Classes
            private class 
        #endregion
    }

    public class Config
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
