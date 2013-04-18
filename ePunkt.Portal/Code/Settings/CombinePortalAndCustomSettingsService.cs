using System;
using System.Collections.Generic;
using System.Linq;
using ePunkt.Api;
using ePunkt.Utilities;

namespace ePunkt.Portal
{
    public class CombinePortalAndCustomSettingsService
    {
        private readonly IEnumerable<Type> _enumTypes = new[] {typeof (PortalSettings.ApplicationOnSignUpType), typeof (PortalSettings.DisplayOnPageType)};

        public void UpdatePortalSettingsWithCustomSettings(PortalSettings portalSettings, CustomSettings customSettings)
        {
            foreach (var property in portalSettings.GetType().GetProperties())
            {
                if (customSettings.AdditionalSettings.ContainsKey(property.Name))
                {
                    if (property.PropertyType == typeof (bool))
                        property.SetValue(portalSettings, customSettings.AdditionalSettings[property.Name].GetBool(), null);
                    else if (property.PropertyType == typeof (int))
                        property.SetValue(portalSettings, customSettings.AdditionalSettings[property.Name].GetInt(), null);
                    else if (property.PropertyType == typeof (string))
                        property.SetValue(portalSettings, customSettings.AdditionalSettings[property.Name], null);
                    else if (_enumTypes.Any(x => x == property.PropertyType))
                    {
                        var valueAsString = customSettings.AdditionalSettings[property.Name];
                        var valueAsEnum = Enum.Parse(property.PropertyType, valueAsString);
                        property.SetValue(portalSettings, valueAsEnum, null);
                    }
                }
            }
        }
    }
}