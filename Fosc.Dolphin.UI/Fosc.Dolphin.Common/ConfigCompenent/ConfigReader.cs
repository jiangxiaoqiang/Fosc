using System.Configuration;

namespace Fosc.Dolphin.Common.ConfigCompenent
{
    public static class ConfigReader
    {
        public static string GetConfig(string databaseName)
        {
            return ConfigurationManager.ConnectionStrings[databaseName].ConnectionString;
        }
    }
}