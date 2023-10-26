using System;
using System.Configuration;
using System.Xml;
namespace FS.Farm.Providers
{
    /// <summary>
    /// Summary description for ContentConfigurationHandler.
    /// </summary>
    internal class OrgCustomerProviderConfigurationHandler : IConfigurationSectionHandler
    {
        public virtual object Create(Object parent, Object context, XmlNode node)
        {
            OrgCustomerProviderConfiguration config = new OrgCustomerProviderConfiguration();
            config.LoadValuesFromConfigurationXml(node);
            return config;
        }
    }
}