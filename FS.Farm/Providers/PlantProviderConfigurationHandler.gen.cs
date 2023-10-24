using System;
using System.Configuration;
using System.Xml;
namespace FS.Farm.Providers
{
    /// <summary>
    /// Summary description for ContentConfigurationHandler.
    /// </summary>
    internal class PlantProviderConfigurationHandler : IConfigurationSectionHandler
    {
        public virtual object Create(Object parent, Object context, XmlNode node)
        {
            PlantProviderConfiguration config = new PlantProviderConfiguration();
            config.LoadValuesFromConfigurationXml(node);
            return config;
        }
    }
}