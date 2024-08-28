using System;
using System.Configuration;
using System.Xml;
namespace FS.Farm.Providers
{
    /// <summary>
    /// Summary description for ContentConfigurationHandler.
    /// </summary>
    internal class DynaFlowTypeProviderConfigurationHandler : IConfigurationSectionHandler
    {
        public virtual object Create(Object parent, Object context, XmlNode node)
        {
            DynaFlowTypeProviderConfiguration config = new DynaFlowTypeProviderConfiguration();
            config.LoadValuesFromConfigurationXml(node);
            return config;
        }
    }
}