using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vivirseguros.CorreoCentralizado.Utilities
{
    public static class UtilHelper
    {
        public static string obtainConfig(string value)
        {
            ExeConfigurationFileMap customConfigFileMap = new ExeConfigurationFileMap();
            customConfigFileMap.ExeConfigFilename = AppDomain.CurrentDomain.BaseDirectory + "customAppSettings.xml";
            System.Configuration.Configuration customConfig = ConfigurationManager.OpenMappedExeConfiguration(customConfigFileMap, ConfigurationUserLevel.None);
            AppSettingsSection appSettings = (customConfig.GetSection("appSettings") as AppSettingsSection);
            return appSettings.Settings[value].Value;
        }
    }
}
