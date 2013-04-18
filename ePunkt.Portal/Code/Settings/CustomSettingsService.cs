using System.Collections.Generic;
using ePunkt.Utilities;
using JetBrains.Annotations;
using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace ePunkt.Portal
{
    public class CustomSettingsService
    {
        [CanBeNull]
        public CustomSettings LoadCustomSettings(HttpContext context)
        {
            var customFolder = Settings.Get("CustomFolder_" + context.Request.Url.Host, "");

            //use the current hostname for custom folder, if none specified in the settings
            if (customFolder.IsNoE())
                customFolder = context.Request.Url.Host;

            CustomSettings customSettings = null;

            if (customFolder.HasValue())
            {
                var path = Path.Combine(context.Server.MapPath("~/Custom"), customFolder, "Settings.config");
                if (File.Exists(path))
                {
                    var xml = XDocument.Load(path);
                    var root = xml.Root;

                    if (root != null)
                    {
                        customSettings = new CustomSettings
                            {
                                CustomFolder = "~/Custom/" + customFolder,

                                ApiEndpoint = FindValue(root, "ApiEndpoint", Settings.Get("ApiEndpoint", "")),
                                ApiKey = FindValue(root, "ApiKey", Settings.Get("ApiKey", "")),
                                MandatorId = FindValue(root, "MandatorId", Settings.Get("MandatorId", "")).GetInt(0),
                                AdditionalSettings = root.Elements("add").ToDictionary(x => x.Attribute("key").Value, x => x.Attribute("value").Value)
                            };
                        customSettings.AdditionalSettings.Remove("ApiEndpoint");
                        customSettings.AdditionalSettings.Remove("ApiKey");
                        customSettings.AdditionalSettings.Remove("MandatorId");
                    }
                }
            }

            if (customSettings == null)
                customSettings = new CustomSettings
                    {
                        ApiEndpoint = Settings.Get("ApiEndpoint", ""),
                        ApiKey = Settings.Get("ApiKey", ""),
                        MandatorId = Settings.Get("MandatorId", "").GetInt(0),
                        AdditionalSettings = new Dictionary<string, string>()
                    };

            //normalize the ApiEndpoint URL, we always need a / at the end
            customSettings.ApiEndpoint = customSettings.ApiEndpoint.TrimEnd('/') + "/";

            if (customSettings.ApiEndpoint.IsNoE())
                throw new ApplicationException("No API endpoint URL found in configuration.");
            if (customSettings.ApiKey.IsNoE())
                throw new ApplicationException("No API key found in configuration.");
            if (customSettings.MandatorId == 0)
                throw new ApplicationException("No mandator ID found in configuration.");

            return customSettings;
        }

        private static string FindValue(XElement root, string key, string defaultValue)
        {
            var element = root.Elements("add").FirstOrDefault(x => x.Attribute("key").Value == key);
            return element != null ? element.Attribute("value").Value : defaultValue;
        }
    }
}