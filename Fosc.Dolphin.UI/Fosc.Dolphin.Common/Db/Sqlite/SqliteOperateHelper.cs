using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Fosc.Dolphin.Common.LogCompenent;
using Fosc.Dolphin.Common.Security;

namespace Fosc.Dolphin.Common.Db.Sqlite
{
    /// <summary>
    /// 
    /// </summary>
    public class SqliteOperateHelper
    {
        private readonly DbConnection _dbConnection;

        public SqliteOperateHelper(DbProviderFactory factory, string connectionString)
        {
            _dbConnection = factory.CreateConnection();
            if (_dbConnection == null) return;
            _dbConnection.ConnectionString = connectionString;
            var cnnstring = factory.CreateConnectionStringBuilder();
            if (cnnstring == null) return;
            cnnstring.ConnectionString = connectionString;
            _dbConnection.Open();
        }

        #region 根据表名称查询表
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        public DataTable SelectTable(string tableName)
        {
            var dataTable = new DataTable();
            using (var cmd = _dbConnection.CreateCommand())
            {
                cmd.CommandText = "select * from " + tableName + " limit 60";
                cmd.ExecuteNonQuery();
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    for (var i = 0; i < reader.FieldCount; i++)
                    {
                        var datat = reader.GetFieldType(i);
                        DataColumn col;
                        if (datat.Name == "Byte[]" && reader.GetName(i) == "password_value")
                        {
                            col = new DataColumn
                            {
                                ColumnName = "password_value",
                                DataType = reader.GetFieldType(i - 1)
                            };
                        }
                        else
                        {
                            col = new DataColumn
                            {
                                ColumnName = reader.GetName(i),
                                DataType = datat
                            };
                        }
                        dataTable.Columns.Add(col);
                    }

                    while (reader.Read())
                    {
                        var row = dataTable.NewRow();
                        for (var i = 0; i < reader.FieldCount; i++)
                        {
                            if (reader.GetName(i) == "password_value")
                            {
                                if (reader[i] != DBNull.Value)
                                {
                                    try
                                    {
                                        byte[] sAditionalEntropy = { };
                                        var passwordValueArray = (byte[])reader[i];
                                        //var decryptArray = DataProtection.UnprotectData(passwordValueArray);
                                        var decryptArray = ProtectedData.Unprotect(passwordValueArray, sAditionalEntropy, DataProtectionScope.LocalMachine);
                                        var decryptResult = Encoding.Default.GetString(decryptArray);
                                        row[i] = decryptResult;
                                    }
                                    catch (Exception e)
                                    {
                                        LogHelper.Logger.Info(e);
                                    }
                                }
                            }
                            else
                            {
                                row[i] = reader[i];
                            }
                        }
                        dataTable.Rows.Add(row);
                    }
                }
            }
            return dataTable;
        }
        #endregion

    }
}
