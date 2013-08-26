using ePunkt.Api.Responses;
using ePunkt.Utilities;
using System.Globalization;
using System.Linq;

namespace ePunkt.Portal
{
    public class Translations
    {

        public static string TlT(MandatorResponse mandatorResponse, string originalText)
        {
            var allTranslations = mandatorResponse.Translations;
            var matchingEntry = allTranslations.FirstOrDefault(x => x.Texts.Any(y => y.Text.Is(originalText)));
            if (matchingEntry != null)
            {
                var currentCulture = CultureInfo.CurrentCulture.Name;

                //check for identical culture
                var matchingTranslation = matchingEntry.Texts.FirstOrDefault(x => x.Culture.Is(currentCulture));

                //check for similar culture
                matchingTranslation = matchingTranslation ?? matchingEntry.Texts.FirstOrDefault(x => x.Culture.Substring(0, 2).Is(currentCulture.Substring(0, 2)));

                //fallback to english
                matchingTranslation = matchingTranslation ?? matchingEntry.Texts.FirstOrDefault(x => x.Culture.Substring(0, 2).Is("en"));

                if (matchingTranslation != null)
                    return matchingTranslation.Text;
            }
            return originalText;
        }
    }
}