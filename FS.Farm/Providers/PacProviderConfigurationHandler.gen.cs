using System;
using System.Configuration;
using System.Xml;
namespace FS.Farm.Providers
{
    /// <summary>
    /// Summary description for ContentConfigurationHandler.
    /// </summary>
    internal class PacProviderConfigurationHandler : IConfigurationSectionHandler
    {
        public virtual object Create(Object parent, Object context, XmlNode node)
        {
            PacProviderConfiguration config = new PacProviderConfiguration();
            config.LoadValuesFromConfigurationXml(node);
            return config;
        }
    }
}