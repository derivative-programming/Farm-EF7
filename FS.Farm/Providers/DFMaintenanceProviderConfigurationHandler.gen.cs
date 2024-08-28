using System;
using System.Configuration;
using System.Xml;
namespace FS.Farm.Providers
{
    /// <summary>
    /// Summary description for ContentConfigurationHandler.
    /// </summary>
    internal class DFMaintenanceProviderConfigurationHandler : IConfigurationSectionHandler
    {
        public virtual object Create(Object parent, Object context, XmlNode node)
        {
            DFMaintenanceProviderConfiguration config = new DFMaintenanceProviderConfiguration();
            config.LoadValuesFromConfigurationXml(node);
            return config;
        }
    }
}