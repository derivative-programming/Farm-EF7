using System;
using System.Configuration;
using System.Xml;
namespace FS.Farm.Providers
{
    /// <summary>
    /// Summary description for ContentConfigurationHandler.
    /// </summary>
    internal class OrgApiKeyProviderConfigurationHandler : IConfigurationSectionHandler
    {
        public virtual object Create(Object parent, Object context, XmlNode node)
        {
            OrgApiKeyProviderConfiguration config = new OrgApiKeyProviderConfiguration();
            config.LoadValuesFromConfigurationXml(node);
            return config;
        }
    }
}