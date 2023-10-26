using System;
using System.Configuration;
using System.Xml;
namespace FS.Farm.Providers
{
    /// <summary>
    /// Summary description for ContentConfigurationHandler.
    /// </summary>
    internal class DateGreaterThanFilterProviderConfigurationHandler : IConfigurationSectionHandler
    {
        public virtual object Create(Object parent, Object context, XmlNode node)
        {
            DateGreaterThanFilterProviderConfiguration config = new DateGreaterThanFilterProviderConfiguration();
            config.LoadValuesFromConfigurationXml(node);
            return config;
        }
    }
}