using System;
using System.Configuration;
using System.IO;

namespace Fosc.Dolphin.Common.ConfigCompenent
{
    public class ConfigReader
    {
        #region Attribute

        private string _codeGeneratePath;

        public string CodeGeneratePath
        {
            get
            {
                _codeGeneratePath = Environment.CurrentDirectory + @"\CodeOutPath\";
                if (!Directory.Exists(_codeGeneratePath))
                {
                    Directory.CreateDirectory(_codeGeneratePath);
                }
                return _codeGeneratePath;
            }
        }

        #endregion

        public static string GetConfig(string databaseName)
        {
            return ConfigurationManager.ConnectionStrings[databaseName].ConnectionString;
        }

        public static string GetAppConfigValue(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }
}