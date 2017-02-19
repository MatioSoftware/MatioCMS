using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace MatioCMS.Includes
{
    /// <summary>
    /// Code library model
    /// </summary>
    public class Library
    {
        /// <param name="RootPath">App root path</param>
        public Library(string RootPath)
        {
            if (string.IsNullOrWhiteSpace(RootPath))
                throw new ArgumentException();
            if()
        }

        private List<Bundle> styles;
        private List<Bundle> scripts;

        public Bundle[] Styles => this.styles.ToArray();

        public Bundle[] Scripts => this.scripts.ToArray();

        private class jsonobject
        {
            public Bundle[] Styles { get; set; }
            public Bundle[] Scripts { get; set; }
        }

        /// <summary>
        /// Style/script set
        /// </summary>
        public class Bundle
        {
            /// <summary>
            /// Unique name of the style/script bundle
            /// </summary>
            public string Name { get; set; }

            public List<Uri> Paths { get; set; }
        }
    }
}
