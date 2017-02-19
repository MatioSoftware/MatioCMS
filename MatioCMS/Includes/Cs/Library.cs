using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace MatioCMS.Database
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
            if (!Directory.Exists(RootPath))
                throw new ArgumentException();

            string path = Path.Combine(RootPath, "/Database/Library.json");
            if (!File.Exists(path))
                throw new ConfigFileNotFoundException("Library.json");

            FileStream json = new FileStream(path, FileMode.Open, FileAccess.Read);
            jsonobject output = JsonConvert.DeserializeObject<jsonobject>(new StreamReader(json).ReadToEnd());
            this.styles = output.Styles.ToList();
            this.scripts = output.Scripts.ToList();
        }

        private List<Bundle> styles;
        private List<Bundle> scripts;

        public bool AddStyle(string name, Uri[] paths)
        {
            if (this.styles.Where(bundle => bundle.Name == name).Count() > 0)
                return false;

            styles.Add(new Bundle { Name = name, Paths = paths.ToList() });
            return true;
        }

        public bool AddScript(string name, Uri[] paths)
        {
            if (this.scripts.Where(bundle => bundle.Name == name).Count() > 0)
                return false;

            scripts.Add(new Bundle { Name = name, Paths = paths.ToList() });
            return true;
        }

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
