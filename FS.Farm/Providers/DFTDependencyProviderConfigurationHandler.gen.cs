using System;
using System.Configuration;
using System.Xml;
namespace FS.Farm.Providers
{
    /// <summary>
    /// Summary description for ContentConfigurationHandler.
    /// </summary>
    internal class DFTDependencyProviderConfigurationHandler : IConfigurationSectionHandler
    {
        public virtual object Create(Object parent, Object context, XmlNode node)
        {
            DFTDependencyProviderConfiguration config = new DFTDependencyProviderConfiguration();
            config.LoadValuesFromConfigurationXml(node);
            return config;
        }
    }
}