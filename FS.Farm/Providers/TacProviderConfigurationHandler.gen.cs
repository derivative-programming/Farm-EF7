using System;
using System.Configuration;
using System.Xml;
namespace FS.Farm.Providers
{
    /// <summary>
    /// Summary description for ContentConfigurationHandler.
    /// </summary>
    internal class TacProviderConfigurationHandler : IConfigurationSectionHandler
    {
        public virtual object Create(Object parent, Object context, XmlNode node)
        {
            TacProviderConfiguration config = new TacProviderConfiguration();
            config.LoadValuesFromConfigurationXml(node);
            return config;
        }
    }
}