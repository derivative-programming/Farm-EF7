using System;
using System.Configuration;
using System.Xml;
namespace FS.Farm.Providers
{
    /// <summary>
    /// Summary description for ContentConfigurationHandler.
    /// </summary>
    internal class LandProviderConfigurationHandler : IConfigurationSectionHandler
    {
        public virtual object Create(Object parent, Object context, XmlNode node)
        {
            LandProviderConfiguration config = new LandProviderConfiguration();
            config.LoadValuesFromConfigurationXml(node);
            return config;
        }
    }
}