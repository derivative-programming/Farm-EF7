using System;
using System.Configuration;
using System.Xml;
namespace FS.Farm.Providers
{
    /// <summary>
    /// Summary description for ContentConfigurationHandler.
    /// </summary>
    internal class DynaFlowTypeScheduleProviderConfigurationHandler : IConfigurationSectionHandler
    {
        public virtual object Create(Object parent, Object context, XmlNode node)
        {
            DynaFlowTypeScheduleProviderConfiguration config = new DynaFlowTypeScheduleProviderConfiguration();
            config.LoadValuesFromConfigurationXml(node);
            return config;
        }
    }
}