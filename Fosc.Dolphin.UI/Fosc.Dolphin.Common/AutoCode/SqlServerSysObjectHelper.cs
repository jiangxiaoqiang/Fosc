using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Maticsoft.DBUtility;

namespace Fosc.Dolphin.Common.AutoCode
{
    class SqlServerSysObjectHelper
    {
        /// <summary>
        /// 获取数据库所有表名
        /// </summary>
        /// <returns></returns>
        public static DataTable GetDataTableName()
        {
            const string sql = "select name from SysObjects where XType='U' order by name";
            var tableNameDataSet = DbHelperSQL.Query(sql);
            return tableNameDataSet.Tables[0];
        }

        /// <summary>
        /// 返回指定数据表的所有字段和字段数据类型
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static DataTable GetDataTableColumn(string tableName)
        {
            var sql = "SELECT COLUMN_NAME,DATA_TYPE,TABLE_NAME,CHARACTER_MAXIMUM_LENGTH FROM information_schema.columns WHERE table_name ='" + tableName + "'";
            var dataTableColumn = DbHelperSQL.Query(sql);
            return dataTableColumn.Tables[0];
        }

        /// <summary>
        /// 返回指定数据表的主键名称
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static string GetDataTableColumnKeyName(string tableName)
        {
            string Sql = "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE WHERE TABLE_NAME='" + tableName + "'";
            DataSet ds = DbHelperSQL.Query(Sql);
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                    return ds.Tables[0].Rows[0][0].ToString();
            }
            return null;
        }

        /// <summary>
        /// 返回指定数据表的主键的数据类型
        /// </summary>
        /// <param name="TableName"></param>
        /// <returns></returns>
        public static string GetDataTableColumnKeyType(string TableName)
        {
            string KeyName = GetDataTableColumnKeyName(TableName);
            if (!string.IsNullOrEmpty(KeyName))
            {
                string Sql = "SELECT DATA_TYPE FROM information_schema.columns WHERE table_name ='" + TableName + "' and column_name='" + KeyName + "'";
                DataSet ds = DbHelperSQL.Query(Sql);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                        return ds.Tables[0].Rows[0][0].ToString();
                }
            }
            return null;
        }

    }
}
