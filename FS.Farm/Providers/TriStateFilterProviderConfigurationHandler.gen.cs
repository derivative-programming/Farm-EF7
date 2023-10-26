using System;
using System.Configuration;
using System.Xml;
namespace FS.Farm.Providers
{
    /// <summary>
    /// Summary description for ContentConfigurationHandler.
    /// </summary>
    internal class TriStateFilterProviderConfigurationHandler : IConfigurationSectionHandler
    {
        public virtual object Create(Object parent, Object context, XmlNode node)
        {
            TriStateFilterProviderConfiguration config = new TriStateFilterProviderConfiguration();
            config.LoadValuesFromConfigurationXml(node);
            return config;
        }
    }
}