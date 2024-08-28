using System;
using System.Configuration;
using System.Xml;
namespace FS.Farm.Providers
{
    /// <summary>
    /// Summary description for ContentConfigurationHandler.
    /// </summary>
    internal class DynaFlowTaskProviderConfigurationHandler : IConfigurationSectionHandler
    {
        public virtual object Create(Object parent, Object context, XmlNode node)
        {
            DynaFlowTaskProviderConfiguration config = new DynaFlowTaskProviderConfiguration();
            config.LoadValuesFromConfigurationXml(node);
            return config;
        }
    }
}