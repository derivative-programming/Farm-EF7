using System;
using System.Configuration;
using System.Xml;
using System.Threading.Tasks;
namespace FS.Farm.Reports.Providers
{
    /// <summary>
    /// Summary description for ContentConfigurationHandler.
    /// </summary>
    internal partial class FarmReportProviderConfigurationHandler : IConfigurationSectionHandler
    {
        public virtual object Create(Object parent, Object context, XmlNode node)
        {
            FarmReportProviderConfiguration config = new FarmReportProviderConfiguration();
            config.LoadValuesFromConfigurationXml(node);
            return config;
        }
    }
}