using System;
using System.Configuration;
using System.Xml;
namespace FS.Farm.Providers
{
    /// <summary>
    /// Summary description for ContentConfigurationHandler.
    /// </summary>
    internal class FlavorProviderConfigurationHandler : IConfigurationSectionHandler
    {
        public virtual object Create(Object parent, Object context, XmlNode node)
        {
            FlavorProviderConfiguration config = new FlavorProviderConfiguration();
            config.LoadValuesFromConfigurationXml(node);
            return config;
        }
    }
}