using System;
using System.Configuration;
using System.Xml;
namespace FS.Farm.Providers
{
    /// <summary>
    /// Summary description for ContentConfigurationHandler.
    /// </summary>
    internal class RoleProviderConfigurationHandler : IConfigurationSectionHandler
    {
        public virtual object Create(Object parent, Object context, XmlNode node)
        {
            RoleProviderConfiguration config = new RoleProviderConfiguration();
            config.LoadValuesFromConfigurationXml(node);
            return config;
        }
    }
}