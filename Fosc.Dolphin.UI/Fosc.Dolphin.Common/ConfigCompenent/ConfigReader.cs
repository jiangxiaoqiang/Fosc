using System.Configuration;

namespace Fosc.Dolphin.Common.ConfigCompenent
{
    public static class ConfigReader
    {
        private static string _codeGenerateConn;

        public static string CodeGenerateConn
        {
            get
            {
                var value = ConfigurationManager.ConnectionStrings["code-generate"].ConnectionString;
                return value;
            }
            set { _codeGenerateConn = value; }
        }
    }
}