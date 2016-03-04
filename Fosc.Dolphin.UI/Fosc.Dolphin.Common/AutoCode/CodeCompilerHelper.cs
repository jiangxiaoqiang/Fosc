using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Fosc.Dolphin.Common.ConfigCompenent;
using Fosc.Dolphin.Common.LogCompenent;
using Maticsoft.DBUtility;
using System.IO;
using Fosc.Dolphin.Object.AutoCode;

namespace Fosc.Dolphin.Common.AutoCode
{
    public class CodeCompilerHelper
    {
        /// <summary>
        /// 生成数据操作类
        /// </summary>
        /// <param name="path">数据库类型</param>
        /// <param name="configModel"></param>
        /// <returns></returns>
        public static bool AutoGenerateImpl(string path, DatabaseConfig configModel)
        {
            var compileSuccess = false;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            var connectionString = ConfigReader.GetConfig(configModel.DatabaseName);
            if (InitSqlConn(connectionString, "sqlserver"))
            {
                CompileModelImpl.CompileModel(configModel.DatabaseAlias);
                CompileModelImpl.CompileDataAccessLayerInterface(configModel.DatabaseAlias);
                CompileModelImpl.CompileDATA_DataAccess(configModel.DatabaseAlias);
                CompileModelImpl.CompileDalSqlServer(configModel.DatabaseAlias);
                CompileModelImpl.CompileDATA_BLL(configModel.DatabaseAlias);
                compileSuccess = true;
            }
            else
            {
                LogHelper.Logger.Error("Database is not connect...");
            }
            return compileSuccess;
        }

        public string Get(DataTable dt, CodeGenerate model)
        {
            if (dt.Rows.Count < 1)
            {
                return "err:无数据";
            }
            else
            {
                var tableName = dt.Rows[0]["TABLE_NAME"].ToString();
                var keyName = ModelGenerateHelper.GetDataTableColumnKeyName(tableName);
                var keyType = ModelGenerateHelper.GetDataTableColumnKeyType(tableName);
                if (string.IsNullOrEmpty(keyName)) keyType = "";

                #region 所有数据操作方法
                var sb = new StringBuilder(); 
                sb.AppendLine(GetCodeForCreateDal()); ModelGenerateHelper.NewLine(sb);
                sb.AppendLine(GetCodeForPublicClass(model)); ModelGenerateHelper.NewLine(sb);
                sb.AppendLine(GetCodeForAdd()); ModelGenerateHelper.NewLine(sb);
                sb.AppendLine(GetCodeForExists()); ModelGenerateHelper.NewLine(sb);
                sb.AppendLine(GetCodeForGetModelList()); ModelGenerateHelper.NewLine(sb);
                sb.AppendLine(GetCodeForGetModelListInInfo()); ModelGenerateHelper.NewLine(sb);
                sb.AppendLine(GetCodeForGetPageList()); ModelGenerateHelper.NewLine(sb);
                sb.AppendLine(GetCodeForGetPageListOld()); ModelGenerateHelper.NewLine(sb);
                sb.AppendLine(GetCodeForGetModelInSql()); ModelGenerateHelper.NewLine(sb);
                ModelGenerateHelper.NewLine(sb);
                sb.Append(GetCodeForExeSQL());
                sb.Append(GetCodeForGetListInSql()); ModelGenerateHelper.NewLine(sb);
                if (!string.IsNullOrEmpty(keyName))
                {
                    sb.AppendLine(GetCodeForDelete()); ModelGenerateHelper.NewLine(sb);
                    sb.AppendLine(GetCodeForDeleteList()); ModelGenerateHelper.NewLine(sb);
                    sb.AppendLine(GetCodeForGetModelByCache()); ModelGenerateHelper.NewLine(sb);
                    sb.AppendLine(GetCodeForGetModel()); ModelGenerateHelper.NewLine(sb);
                    sb.AppendLine(GetCodeForUpdate()); ModelGenerateHelper.NewLine(sb);
                }
                #endregion

                var code = sb.ToString();

                #region 设置表中常用的参数值
                code = code.Replace("@$TABLE_NAME", tableName);
                code = code.Replace("@$TABLE__NAME", tableName.Replace(".", "_"));

                code = code.Replace("@$TABLE_KEY_TYPE", ModelGenerateHelper.FormatDataSqlTypeToSqlDbType(keyType).ToString());
                if (!string.IsNullOrEmpty(keyName))
                    code = code.Replace("@$TABLE_KEY_type", ModelGenerateHelper.FormatDataType(ModelGenerateHelper.FormatDataSqlTypeToSqlDbType(keyType)));
                else
                    code = code.Replace("@$TABLE_KEY_type", "");

                code = code.Replace("@$TABLE_KEY", keyName);
                #endregion

                return code;
            }
        }


        private string GetCodeForCreateDal()
        {
            return "private readonly I@$TABLE__NAME dal=DataAccess.DataAccess.Create@$TABLE__NAME();";
        }

        private string GetCodeForPublicClass(CodeGenerate model)
        {
            var sb = new StringBuilder();
            sb.Append("public @$TABLE__NAMEBLL()"); ModelGenerateHelper.NewLine(sb);
            sb.Append("{"); ModelGenerateHelper.NewLine(sb);
            sb.Append("DbHelperSQL.connectionString = System.Configuration.ConfigurationManager.ConnectionStrings[\"" + model.ConnectionName + "\"].ToString();"); ModelGenerateHelper.NewLine(sb);
            sb.Append("}"); ModelGenerateHelper.NewLine(sb);

            return sb.ToString();
        }

        private string GetCodeForExists()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("public bool Exists(@$TABLE_KEY_type @$TABLE_KEY)"); ModelGenerateHelper.NewLine(sb);
            sb.Append("{"); ModelGenerateHelper.NewLine(sb);
            sb.Append("	return dal.Exists(@$TABLE_KEY);"); ModelGenerateHelper.NewLine(sb);
            sb.Append("}"); ModelGenerateHelper.NewLine(sb);
            return sb.ToString();
        }

        private string GetCodeForAdd()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("public int Add(@$TABLE__NAMEModel model)"); ModelGenerateHelper.NewLine(sb);
            sb.Append("{"); ModelGenerateHelper.NewLine(sb);
            sb.Append("	return dal.Add(model);"); ModelGenerateHelper.NewLine(sb);
            sb.Append("}"); ModelGenerateHelper.NewLine(sb);
            return sb.ToString();
        }

        private string GetCodeForUpdate()
        {
            var sb = new StringBuilder();
            sb.Append("public bool Update(@$TABLE__NAMEModel model)"); ModelGenerateHelper.NewLine(sb);
            sb.Append("{"); ModelGenerateHelper.NewLine(sb);
            sb.Append("	return dal.Update(model);"); ModelGenerateHelper.NewLine(sb);
            sb.Append("}"); ModelGenerateHelper.NewLine(sb);
            return sb.ToString();
        }

        private string GetCodeForDelete()
        {
            var sb = new StringBuilder();
            sb.Append("public bool Delete(@$TABLE_KEY_type @$TABLE_KEY)"); ModelGenerateHelper.NewLine(sb);
            sb.Append("{"); ModelGenerateHelper.NewLine(sb);
            sb.Append("	return dal.Delete(@$TABLE_KEY);"); ModelGenerateHelper.NewLine(sb);
            sb.Append("}"); ModelGenerateHelper.NewLine(sb);
            return sb.ToString();
        }

        private string GetCodeForDeleteList()
        {
            var sb = new StringBuilder();
            sb.Append("public bool DeleteList(string @$TABLE_KEYList)"); ModelGenerateHelper.NewLine(sb);
            sb.Append("{"); ModelGenerateHelper.NewLine(sb);
            sb.Append("	return dal.DeleteList(@$TABLE_KEYList);"); ModelGenerateHelper.NewLine(sb);
            sb.Append("}"); ModelGenerateHelper.NewLine(sb);
            return sb.ToString();
        }

        private string GetCodeForExeSQL()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("public bool ExeSQL(string sql)"); ModelGenerateHelper.NewLine(sb);
            sb.Append("{"); ModelGenerateHelper.NewLine(sb);
            sb.Append("	return dal.ExeSQL(sql);"); ModelGenerateHelper.NewLine(sb);
            sb.Append("}"); ModelGenerateHelper.NewLine(sb);
            return sb.ToString();
        }

        private string GetCodeForGetModel()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("public @$TABLE__NAMEModel GetModel(@$TABLE_KEY_type @$TABLE_KEY)"); ModelGenerateHelper.NewLine(sb);
            sb.Append("{"); ModelGenerateHelper.NewLine(sb);
            sb.Append("	return dal.GetModel(@$TABLE_KEY);"); ModelGenerateHelper.NewLine(sb);
            sb.Append("}"); ModelGenerateHelper.NewLine(sb);
            return sb.ToString();
        }

        private string GetCodeForGetModelInSql()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("public @$TABLE__NAMEModel GetModel(string where)"); ModelGenerateHelper.NewLine(sb);
            sb.Append("{"); ModelGenerateHelper.NewLine(sb);
            sb.Append("	return dal.GetModel(where);"); ModelGenerateHelper.NewLine(sb);
            sb.Append("}"); ModelGenerateHelper.NewLine(sb);
            return sb.ToString();
        }

        private string GetCodeForGetModelByCache()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("public @$TABLE__NAMEModel GetModelByCache(@$TABLE_KEY_type @$TABLE_KEY)"); ModelGenerateHelper.NewLine(sb);
            sb.Append("{"); ModelGenerateHelper.NewLine(sb);
            sb.Append("string CacheKey = \"@$TABLE__NAMEModel-\" + @$TABLE_KEY;"); ModelGenerateHelper.NewLine(sb);
            sb.Append("object objModel = DataAccess.DataAccess.GetCache(CacheKey);"); ModelGenerateHelper.NewLine(sb);
            sb.Append("if (objModel == null)"); ModelGenerateHelper.NewLine(sb);
            sb.Append("{"); ModelGenerateHelper.NewLine(sb);
            sb.Append("	try"); ModelGenerateHelper.NewLine(sb);
            sb.Append("	{"); ModelGenerateHelper.NewLine(sb);
            sb.Append("		objModel = dal.GetModel(@$TABLE_KEY);"); ModelGenerateHelper.NewLine(sb);
            sb.Append("		if (objModel != null)"); ModelGenerateHelper.NewLine(sb);
            sb.Append("		{"); ModelGenerateHelper.NewLine(sb);
            sb.Append("			int ModelCache = 60;"); ModelGenerateHelper.NewLine(sb);
            sb.Append("			DataAccess.DataAccess.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);");
            ModelGenerateHelper.NewLine(sb);
            sb.Append("		}"); ModelGenerateHelper.NewLine(sb);
            sb.Append("	}"); ModelGenerateHelper.NewLine(sb);
            sb.Append("	catch{}"); ModelGenerateHelper.NewLine(sb);
            sb.Append("}"); ModelGenerateHelper.NewLine(sb);
            sb.Append("return (@$TABLE__NAMEModel)objModel;"); ModelGenerateHelper.NewLine(sb);
            sb.Append("}"); ModelGenerateHelper.NewLine(sb);

            return sb.ToString();
        }


        private string GetCodeForGetModelList()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("public List<@$TABLE__NAMEModel> GetModelList(string where)"); ModelGenerateHelper.NewLine(sb);
            sb.Append("{"); ModelGenerateHelper.NewLine(sb);
            sb.Append("	return dal.GetList(where);"); ModelGenerateHelper.NewLine(sb);
            sb.Append("}"); ModelGenerateHelper.NewLine(sb);
            return sb.ToString();
        }


        private string GetCodeForGetModelListInInfo()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("public List<@$TABLE__NAMEModel> GetModelList(int top,string fields,string where,string order)"); ModelGenerateHelper.NewLine(sb);
            sb.Append("{"); ModelGenerateHelper.NewLine(sb);
            sb.Append("	return dal.GetModelList(top,fields,where,order);"); ModelGenerateHelper.NewLine(sb);
            sb.Append("}"); ModelGenerateHelper.NewLine(sb);
            return sb.ToString();
        }


        private string GetCodeForGetListInSql()
        {
            var sb = new StringBuilder();
            sb.AppendLine("public DataTable GetListForSQL(string Sql)");
            sb.AppendLine("{");
            sb.AppendLine("	return dal.GetListForSQL(Sql);");
            sb.AppendLine("}");
            return sb.ToString();
        }


        private string GetCodeForGetPageList()
        {
            var sb = new StringBuilder();
            sb.AppendLine("public DataTable GetListByPage(string strWhere, string fieldList, string orderby, int pz, int pi, out int count)"); ModelGenerateHelper.NewLine(sb);
            sb.AppendLine("{");
            sb.AppendLine("return dal.GetListByPage(strWhere, fieldList, orderby, pz, pi, out count);");
            ModelGenerateHelper.NewLine(sb);
            sb.AppendLine("}"); 
            return sb.ToString();
        }

        private string GetCodeForGetPageListOld()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("public DataSet GetList(int PageSize,int PageIndex,string fieldList,string strWhere,string Order,int SortType)"); ModelGenerateHelper.NewLine(sb);
            sb.Append("{"); ModelGenerateHelper.NewLine(sb);
            sb.Append("return dal.GetList(PageSize,PageIndex,fieldList,strWhere,Order,SortType);"); ModelGenerateHelper.NewLine(sb);
            sb.Append("}"); ModelGenerateHelper.NewLine(sb);
            return sb.ToString();
        }

        /// <summary>
        /// 初始化数据库连接字串
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="dbType"></param>
        private static bool InitSqlConn(string conn, string dbType)
        {
            if (dbType.ToLower() == "sqlserver")
            {
                if (ConnectionTest(conn))
                {
                    DbHelperSQL.connectionString = conn;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        /// <summary>
        /// DB connection success
        /// </summary>
        /// <returns></returns>
        private static bool ConnectionTest(string conn)
        {
            var conned = false;
            var sqlServerConn = new SqlConnection(conn);
            try
            {
                sqlServerConn.Open();
                conned = true;
            }
            catch (Exception e)
            {
                LogHelper.Logger.Error("Connection failed,conn:" + conn, e);
                conned = false;
            }
            finally
            {
                sqlServerConn.Close();
            }
            if (sqlServerConn.State == ConnectionState.Closed || sqlServerConn.State == ConnectionState.Broken)
            {
                return conned;
            }
            else
            {
                return conned;
            }
        }
    }
}
