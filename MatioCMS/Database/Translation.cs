using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace MatioCMS.Database
{
    public sealed class Translation
    {
        /// <param name="RootPath">Webpage root path</param>
        public Translation(string RootPath, LanguageCode CurrentLanguage)
        {
            if (string.IsNullOrWhiteSpace(RootPath))
                throw new ArgumentException();
            if (!Directory.Exists(RootPath))
                throw new FileNotFoundException();
            FileStream json = new FileStream(Path.Combine(RootPath, "/Database/Translation.json"), FileMode.Open, FileAccess.Read);
            jsonobject output = JsonConvert.DeserializeObject<jsonobject>(new StreamReader(json).ReadToEnd());
            this.Titles = output.Titles;
            this.Messages = output.Messages;
            this.lang = CurrentLanguage;
        }

        private LanguageCode lang;

        public string GetTitle(string ZoneName, uint Index)
        {
            if (string.IsNullOrWhiteSpace(ZoneName))
                throw new ArgumentNullException("ZoneName");
            var list1 = this.Titles.Where(item => item.Zone == ZoneName);
            if (list1.Count() != 1)
                throw new TranslationException($"Cannot find zone: \"{ZoneName}\".");
            if (list1.First().Items.Count <= Index)
                throw new IndexOutOfRangeException();
            var list2 = list1.First().Items[(int)Index];
            if (list2.Where(item => item.Language == this.lang).Count() != 1)
            {
                if (list2.Where(item => item.Language == LanguageCode.en).Count() != 1)
                    throw new TranslationException("No translation in English and selected language.");
                else
                    return list2.Where(item => item.Language == LanguageCode.en).First().Value;
            }
            else
                return list2.Where(item => item.Language == this.lang).First().Value;
        }

        public string GetMessage(string ZoneName, uint Index)
        {
            if (string.IsNullOrWhiteSpace(ZoneName))
                throw new ArgumentNullException("ZoneName");
            var list1 = this.Messages.Where(item => item.Zone == ZoneName);
            if (list1.Count() != 1)
                throw new TranslationException($"Cannot find zone: \"{ZoneName}\".");
            if (list1.First().Items.Count <= Index)
                throw new IndexOutOfRangeException();
            var list2 = list1.First().Items[(int)Index];
            if (list2.Where(item => item.Language == this.lang).Count() != 1)
            {
                if (list2.Where(item => item.Language == LanguageCode.en).Count() != 1)
                    throw new TranslationException("No translation in English and selected language.");
                else
                    return list2.Where(item => item.Language == LanguageCode.en).First().Value;
            }
            else
                return list2.Where(item => item.Language == this.lang).First().Value;
        }

        public List<TranslationZone> Titles { get; private set; }

        public List<TranslationZone> Messages { get; private set; }

        private class jsonobject
        {
            public List<TranslationZone> Titles { get; private set; }
            public List<TranslationZone> Messages { get; private set; }
        }

        public class TranslationZone
        {
            public string Zone { get; set; }
            public List<List<TranslatedValue>> Items { get; set; }
        }

        public class TranslatedValue
        {
            public LanguageCode Language { get; set; }
            public string Value { get; set; }
        }

        public enum LanguageCode
        {
            en = 0,
            pl = 1
        }

        public class TranslationException : ApplicationException
        {
            public TranslationException() : base() { }
            public TranslationException(string message) : base(message) { }
            public TranslationException(string message, Exception innerException) : base(message, innerException) { }
        }
    }
}
