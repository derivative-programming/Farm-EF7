using System;
using System.Configuration;
using System.Xml;
namespace FS.Farm.Providers
{
    /// <summary>
    /// Summary description for ContentConfigurationHandler.
    /// </summary>
    internal class DynaFlowProviderConfigurationHandler : IConfigurationSectionHandler
    {
        public virtual object Create(Object parent, Object context, XmlNode node)
        {
            DynaFlowProviderConfiguration config = new DynaFlowProviderConfiguration();
            config.LoadValuesFromConfigurationXml(node);
            return config;
        }
    }
}