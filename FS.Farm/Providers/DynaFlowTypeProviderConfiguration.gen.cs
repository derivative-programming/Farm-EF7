using System;
using System.Configuration;
using System.Collections;
using System.Xml;
using System.Collections.Specialized;
namespace FS.Farm.Providers
{
    internal class DynaFlowTypeProviderConfiguration
    {
        string defaultProvider;
        bool cacheAll = false;
        bool cacheIndividual = false;
        int cacheLifetimeInMinutes = 60;
        int maxCacheCount = 100;
        bool lazyInsert = false;
        bool lazyUpdate = false;
        bool lazyDelete = false;
        bool raiseEventOnInsert = false;
        bool raiseEventOnUpdate = false;
        bool raiseEventOnDelete = false;
        bool isMirrorActive = false;
        Hashtable providers = new Hashtable();
        public static DynaFlowTypeProviderConfiguration GetConfig()
        {
            string sectionName = "FS.Farm" + "." + "DynaFlowTypeProvider";
            DynaFlowTypeProviderConfiguration result = null;
            if(ConfigurationManager.GetSection(sectionName) != null)
                result = (DynaFlowTypeProviderConfiguration)ConfigurationManager.GetSection(sectionName);
            string overrideConfig = FS.Common.IO.Directory.GetBinDirectory() + "FS.Farm.config";
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
                    IConfigurationSectionHandler sectionHandler = new DynaFlowTypeProviderConfigurationHandler();
                    var xmlDocument = new XmlDocument();
                    xmlDocument.LoadXml(xml);
                    result = (DynaFlowTypeProviderConfiguration)sectionHandler.Create(null, null, xmlDocument.DocumentElement);
                }
            }
            return result;
        }
        public void LoadValuesFromConfigurationXml(XmlNode node)
        {
            XmlAttributeCollection attributeCollection = node.Attributes;
            // Get the default provider
            defaultProvider = attributeCollection["defaultProvider"].Value;
            if (attributeCollection["cacheAll"] != null)
            {
                string cacheAllStr = attributeCollection["cacheAll"].Value;
                bool.TryParse(cacheAllStr, out cacheAll);
            }
            if (attributeCollection["cacheIndividual"] != null)
            {
                string cacheIndividualStr = attributeCollection["cacheIndividual"].Value;
                bool.TryParse(cacheIndividualStr, out cacheIndividual);
            }
            if (attributeCollection["cacheLifetimeInMinutes"] != null)
            {
                string cacheLifetimeInMinutesStr = attributeCollection["cacheLifetimeInMinutes"].Value;
                int.TryParse(cacheLifetimeInMinutesStr, out cacheLifetimeInMinutes);
            }
            if (attributeCollection["maxCacheCount"] != null)
            {
                string maxCacheCountStr = attributeCollection["maxCacheCount"].Value;
                int.TryParse(maxCacheCountStr, out maxCacheCount);
            }
            if (attributeCollection["lazyInsert"] != null)
            {
                string lazyInsertStr = attributeCollection["lazyInsert"].Value;
                bool.TryParse(lazyInsertStr, out lazyInsert);
            }
            if (attributeCollection["lazyUpdate"] != null)
            {
                string lazyUpdateStr = attributeCollection["lazyUpdate"].Value;
                bool.TryParse(lazyUpdateStr, out lazyUpdate);
            }
            if (attributeCollection["lazyDelete"] != null)
            {
                string lazyDeleteStr = attributeCollection["lazyDelete"].Value;
                bool.TryParse(lazyDeleteStr, out lazyDelete);
            }
            if (attributeCollection["raiseEventOnInsert"] != null)
            {
                string raiseEventOnInsertStr = attributeCollection["raiseEventOnInsert"].Value;
                bool.TryParse(raiseEventOnInsertStr, out raiseEventOnInsert);
            }
            if (attributeCollection["raiseEventOnUpdate"] != null)
            {
                string raiseEventOnUpdateStr = attributeCollection["raiseEventOnUpdate"].Value;
                bool.TryParse(raiseEventOnUpdateStr, out raiseEventOnUpdate);
            }
            if (attributeCollection["raiseEventOnDelete"] != null)
            {
                string raiseEventOnDeleteStr = attributeCollection["raiseEventOnDelete"].Value;
                bool.TryParse(raiseEventOnDeleteStr, out raiseEventOnDelete);
            }
            if (attributeCollection["isMirrorActive"] != null)
            {
                string isMirrorActiveStr = attributeCollection["isMirrorActive"].Value;
                bool.TryParse(isMirrorActiveStr, out isMirrorActive);
            }
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
        public bool CacheAll { get { return cacheAll; } }
        public bool CacheIndividual { get { return cacheIndividual; } }
        public int CacheLifetimeInMinutes { get { return cacheLifetimeInMinutes; } }
        public int MaxCacheCount { get { return maxCacheCount; } }
        public bool LazyInsert { get { return lazyInsert; } }
        public bool LazyUpdate { get { return lazyUpdate; } }
        public bool LazyDelete { get { return lazyDelete; } }
        public bool RaiseEventOnInsert { get { return raiseEventOnInsert; } }
        public bool RaiseEventOnUpdate { get { return raiseEventOnUpdate; } }
        public bool RaiseEventOnDelete { get { return raiseEventOnDelete; } }
        public bool IsMirrorActive { get { return isMirrorActive; } }
        public Hashtable Providers { get { return providers; } }
    }
}
