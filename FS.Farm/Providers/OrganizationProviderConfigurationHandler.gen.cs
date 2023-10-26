using System;
using System.Configuration;
using System.Xml;
namespace FS.Farm.Providers
{
    /// <summary>
    /// Summary description for ContentConfigurationHandler.
    /// </summary>
    internal class OrganizationProviderConfigurationHandler : IConfigurationSectionHandler
    {
        public virtual object Create(Object parent, Object context, XmlNode node)
        {
            OrganizationProviderConfiguration config = new OrganizationProviderConfiguration();
            config.LoadValuesFromConfigurationXml(node);
            return config;
        }
    }
}