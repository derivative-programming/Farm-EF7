using System;
using System.Configuration;
using System.Xml;
namespace FS.Farm.Providers
{
    /// <summary>
    /// Summary description for ContentConfigurationHandler.
    /// </summary>
    internal class CustomerRoleProviderConfigurationHandler : IConfigurationSectionHandler
    {
        public virtual object Create(Object parent, Object context, XmlNode node)
        {
            CustomerRoleProviderConfiguration config = new CustomerRoleProviderConfiguration();
            config.LoadValuesFromConfigurationXml(node);
            return config;
        }
    }
}