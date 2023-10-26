using System;
using System.Configuration;
using System.Xml;
namespace FS.Farm.Providers
{
    /// <summary>
    /// Summary description for ContentConfigurationHandler.
    /// </summary>
    internal class CustomerProviderConfigurationHandler : IConfigurationSectionHandler
    {
        public virtual object Create(Object parent, Object context, XmlNode node)
        {
            CustomerProviderConfiguration config = new CustomerProviderConfiguration();
            config.LoadValuesFromConfigurationXml(node);
            return config;
        }
    }
}