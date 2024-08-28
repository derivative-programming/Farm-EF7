using System;
using System.Configuration;
using System.Xml;
namespace FS.Farm.Providers
{
    /// <summary>
    /// Summary description for ContentConfigurationHandler.
    /// </summary>
    internal class DynaFlowTaskTypeProviderConfigurationHandler : IConfigurationSectionHandler
    {
        public virtual object Create(Object parent, Object context, XmlNode node)
        {
            DynaFlowTaskTypeProviderConfiguration config = new DynaFlowTaskTypeProviderConfiguration();
            config.LoadValuesFromConfigurationXml(node);
            return config;
        }
    }
}