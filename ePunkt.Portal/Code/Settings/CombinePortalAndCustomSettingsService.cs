using ePunkt.Api.Responses;
using ePunkt.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ePunkt.Portal
{
    public class CombinePortalAndCustomSettingsService
    {
        private readonly IEnumerable<Type> _enumTypes = new[] {typeof (PortalSettingsResponse.ApplicationOnSignUpType), typeof (PortalSettingsResponse.DisplayOnPageType)};

        public void UpdatePortalSettingsWithCustomSettings(PortalSettingsResponse portalSettingsResponse, CustomSettings customSettings)
        {
            foreach (var property in portalSettingsResponse.GetType().GetProperties())
            {
                if (customSettings.AdditionalSettings.ContainsKey(property.Name))
                {
                    if (property.PropertyType == typeof (bool))
                        property.SetValue(portalSettingsResponse, customSettings.AdditionalSettings[property.Name].GetBool(), null);
                    else if (property.PropertyType == typeof (int))
                        property.SetValue(portalSettingsResponse, customSettings.AdditionalSettings[property.Name].GetInt(), null);
                    else if (property.PropertyType == typeof (string))
                        property.SetValue(portalSettingsResponse, customSettings.AdditionalSettings[property.Name], null);
                    else if (_enumTypes.Any(x => x == property.PropertyType))
                    {
                        var valueAsString = customSettings.AdditionalSettings[property.Name];
                        var valueAsEnum = Enum.Parse(property.PropertyType, valueAsString);
                        property.SetValue(portalSettingsResponse, valueAsEnum, null);
                    }
                }
            }
        }
    }
}