using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using Fosc.Dolphin.Common.ConfigCompenent;

namespace Fosc.Dolphin.Common.Db.Sqlite
{
    public class SqliteService
    {
        #region Attribute

        /// <summary>
        /// 
        /// </summary>
        private const string providerInvariantName = "System.Data.SQLite";
        
        #endregion
        

        public static string ProviderInvariantName
        {
            get { return providerInvariantName; }
        }

        #region Function

        #region Get Table
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static DataTable GetDataTable(string tableName)
        {
            var dbProviderFactory = DbProviderFactories.GetFactory(providerInvariantName);
            var sqliteOperator = new SqliteOperateHelper(dbProviderFactory, ConfigReader.GetAppConfigValue("sqliteConn"));
            var sqliteTable = sqliteOperator.SelectTable(tableName);
            return sqliteTable;
        }
        #endregion

        #endregion

    }
}
