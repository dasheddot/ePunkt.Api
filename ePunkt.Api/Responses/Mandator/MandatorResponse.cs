using System.Collections.Generic;

namespace ePunkt.Api.Responses
{
    public class MandatorResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public PortalSettingsResponse PortalSettings { get; set; }
        public IEnumerable<TranslationResponse> Translations { get; set; }
        public IEnumerable<string> JobProfiles { get; set; }
        public IEnumerable<RegionResponse> Regions { get; set; }
        public IEnumerable<string> Urls { get; set; }
        public IEnumerable<string> ApplicantDocumentTypes { get; set; }
        public IEnumerable<TitleResponse> TitlesBeforeName { get; set; }
        public IEnumerable<TitleResponse> TitlesAfterName { get; set; }
        public IEnumerable<string> Countries { get; set; }
    }

    public class TranslationResponse
    {
        public string Key { get; set; }
        public IEnumerable<TranslationTextResponse> Texts { get; set; }
    }

    public class TranslationTextResponse
    {
        public string Culture { get; set; }
        public string Text { get; set; }
    }

    public class RegionResponse
    {
        public string Name { get; set; }
        public IEnumerable<RegionResponse> Regions { get; set; }
    }

    public class TitleResponse
    {
        public string Name { get; set; }
        public IEnumerable<string> CultureFilter { get; set; }
        public string Group { get; set; }
    }
}
