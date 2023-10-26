using System;
using System.Configuration;
using System.Xml;
namespace FS.Farm.Providers
{
    /// <summary>
    /// Summary description for ContentConfigurationHandler.
    /// </summary>
    internal class ErrorLogProviderConfigurationHandler : IConfigurationSectionHandler
    {
        public virtual object Create(Object parent, Object context, XmlNode node)
        {
            ErrorLogProviderConfiguration config = new ErrorLogProviderConfiguration();
            config.LoadValuesFromConfigurationXml(node);
            return config;
        }
    }
}