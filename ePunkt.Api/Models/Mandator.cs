using System.Collections.Generic;

namespace ePunkt.Api.Models
{
    public class Mandator
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public PortalSettings Settings { get; set; }
        public IEnumerable<Translation> Translations { get; set; }
        public IEnumerable<string> JobProfiles { get; set; }
        public IEnumerable<Region> Regions { get; set; }
        public IEnumerable<string> Urls { get; set; }
    }

    public class Translation
    {
        public string Key { get; set; }
        public IEnumerable<TranslationText> Texts { get; set; }
    }

    public class TranslationText
    {
        public string Culture { get; set; }
        public string Text { get; set; }
    }

    public class Region
    {
        public string Name { get; set; }
        public IEnumerable<Region> Regions { get; set; }
    }
}
