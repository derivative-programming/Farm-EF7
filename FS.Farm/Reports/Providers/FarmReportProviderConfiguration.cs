using System;
using System.Configuration;
using System.Collections;
using System.Xml;
using System.Collections.Specialized;
using System.Threading.Tasks;
namespace FS.Farm.Reports.Providers
{
    internal partial class FarmReportProviderConfiguration
    {
        string defaultProvider;
        Hashtable providers = new Hashtable();
        public static FarmReportProviderConfiguration GetConfig()
        {
            string sectionName = "FS.Farm.Reports.FarmReportProvider";
            FarmReportProviderConfiguration result = null;
            if (ConfigurationManager.GetSection(sectionName) != null)
                result = (FarmReportProviderConfiguration)ConfigurationManager.GetSection(sectionName);
            string overrideConfig = FS.Common.IO.Directory.GetBinDirectory()  + "FS.Farm.config";
            if (result == null && System.IO.File.Exists(overrideConfig))
            {
                if (!System.IO.File.Exists(overrideConfig.Replace(".config", "", StringComparison.OrdinalIgnoreCase)))
                {
                    System.IO.File.Create(overrideConfig.ToLower().Replace(".config", "", StringComparison.OrdinalIgnoreCase)).Close();
                }
                System.Configuration.Configuration systemConfiguration = System.Configuration.ConfigurationManager.OpenExeConfiguration(overrideConfig.Replace(".config", "", StringComparison.OrdinalIgnoreCase));
                if (systemConfiguration.GetSection(sectionName) != null)
                {
                    string xml = systemConfiguration.Sections[sectionName].SectionInformation.GetRawXml();
                    IConfigurationSectionHandler sectionHandler = new FarmReportProviderConfigurationHandler();
                    var xmlDocument = new XmlDocument();
                    xmlDocument.LoadXml(xml);
                    result = (FarmReportProviderConfiguration)sectionHandler.Create(null, null, xmlDocument.DocumentElement);
                }
            }
            return result;
        }
        public void LoadValuesFromConfigurationXml(XmlNode node)
        {
            XmlAttributeCollection attributeCollection = node.Attributes;
            // Get the default provider
            defaultProvider = attributeCollection["defaultProvider"].Value;
            // Read child nodes
            foreach (XmlNode child in node.ChildNodes)
            {
                if (child.Name == "providers")
                    GetProviders(child);
            }
        }
        void GetProviders(XmlNode node)
        {
            foreach (XmlNode provider in node.ChildNodes)
            {
                switch (provider.Name)
                {
                    case "add":
                        providers.Add(provider.Attributes["name"].Value, new FS.Common.Providers.Provider(provider.Attributes));
                        break;
                    case "remove":
                        providers.Remove(provider.Attributes["name"].Value);
                        break;
                    case "clear":
                        providers.Clear();
                        break;
                }
            }
        }
        // Properties
        //
        public string DefaultProvider { get { return defaultProvider; } }
        public Hashtable Providers { get { return providers; } }
    }
}
