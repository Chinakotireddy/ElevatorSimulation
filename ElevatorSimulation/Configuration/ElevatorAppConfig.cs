using System;
using System.Collections.Specialized;
using System.Configuration;

namespace ElevatorSimulation.Configuration
{
    public static class ElevatorAppConfig
    {
        static readonly NameValueCollection s_appSettings = ConfigurationManager.AppSettings;
        
        public static string DeploymentEnvironment { get; set; }

        public static string GetConfig(string keyToFind)
        {
            string valueToReturn;
            valueToReturn = Environment.GetEnvironmentVariable(keyToFind);
            valueToReturn ??= string.IsNullOrEmpty(s_appSettings[$"{keyToFind}_{DeploymentEnvironment}"]) ?
                s_appSettings[$"{keyToFind}"] : s_appSettings[$"{keyToFind}_{DeploymentEnvironment}"];
            return valueToReturn;
        }
    }
}
