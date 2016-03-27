using System;
using System.Data;
using System.Text;
using Fosc.Dolphin.Object.AutoCode;

namespace Fosc.Dolphin.Common.AutoCode
{
    public class SqlServerLayerGenerateHelper
    {
        /// <summary>
        ///     获取一张表的数据操作方法
        /// </summary>
        /// <param name="dataTableName"></param>
        /// <param name="isAddNamespace"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public string GetDataTableDal(string dataTableName, bool isAddNamespace, CodeGenerate model)
        {
            var dt = SqlServerSysObjectHelper.GetDataTableColumn(dataTableName);
            if (dt == null) return null;
            //包装一个类
            var code = ModelLayerGenerateHelper.GetUserPartialCode(model, Get(dt, model),
                "I" + dataTableName.Replace(".", "_"));
            if (code.IndexOf("err", StringComparison.Ordinal) >= 0) return null;

            //包装命名空间
            if (isAddNamespace)
                code = ModelLayerGenerateHelper.GetUserNamespaceCode(model, code);
            return code;
        }

        private string GetCodeForSysPublicClass(CodeGenerate model)
        {
            var sb = new StringBuilder();
            sb.Append("public SysDAL()");
            ModelLayerGenerateHelper.NewLine(sb);
            //sb.Append("public static void initConn()"); ModelGenerateHelper.NewLine(sb);
            sb.Append("{");
            ModelLayerGenerateHelper.NewLine(sb);
            //sb.Append("DbHelperSQL.connectionString = \"" + model.ConnectionString.Replace("\\", "\\\\") + "\";"); ModelGenerateHelper.NewLine(sb);
            //WebConfigurationManager.ConnectionStrings[key].ToString()
            sb.Append("DbHelperSQL.connectionString = System.Configuration.ConfigurationManager.ConnectionStrings[\"" +
                      model.ConnectionName + "\"].ToString();");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("}");
            ModelLayerGenerateHelper.NewLine(sb);

            return sb.ToString();
        }

        private string GetSys_TableName()
        {
            var sb = new StringBuilder();
            sb.Append("public DataTable GetSys_TableName()");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("{");
            ModelLayerGenerateHelper.NewLine(sb);

            sb.Append("string Sql = \"Select Name FROM SysObjects Where XType='U' orDER BY Name\";");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("DataSet ds = DbHelperSQL.Query(Sql);");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("if (ds.Tables.Count > 0)");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("    return ds.Tables[0];");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("else");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("    return null;");
            ModelLayerGenerateHelper.NewLine(sb);

            sb.Append("}");
            ModelLayerGenerateHelper.NewLine(sb);

            return sb.ToString();
        }

        /// <summary>
        ///     获取一张数据表的所有操作方法
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public string Get(DataTable dt, CodeGenerate model)
        {
            if (dt.Rows.Count < 1)
                return "err:无数据";
            var tableName = dt.Rows[0]["TABLE_NAME"].ToString();
            var keyName = SqlServerSysObjectHelper.GetDataTableColumnKeyName(tableName);
            var keyType = SqlServerSysObjectHelper.GetDataTableColumnKeyType(tableName);
            if (string.IsNullOrEmpty(keyName)) keyType = "";

            #region 所有数据操作方法

            var sb = new StringBuilder();
            sb.Append(GetCodeForPublicClass(model));
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append(GetCodeForExists(dt, keyName));
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append(GetCodeForAdd(dt, keyName));
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append(GetCodeForGetList(dt));
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append(GetCodeForGetList2(dt));
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append(GetCodeForGetListQ(dt));
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append(GetCodeForExeSql());
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append(GetCodeForGetListInPage());
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append(GetCodeForGetListInPageOld());
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append(GetCodeForGetListInSql());
            sb.Append(GetCodeForGetModelInSql(dt));
            ModelLayerGenerateHelper.NewLine(sb);
            if (!string.IsNullOrEmpty(keyName))
            {
                sb.Append(GetCodeForUpdate(dt, keyName));
                ModelLayerGenerateHelper.NewLine(sb);
                sb.Append(GetCodeForDelete());
                ModelLayerGenerateHelper.NewLine(sb);
                sb.Append(GetCodeForDeleteList());
                ModelLayerGenerateHelper.NewLine(sb);
                sb.Append(GetCodeForGetModel(dt));
                ModelLayerGenerateHelper.NewLine(sb);
            }

            #endregion

            var Code = sb.ToString();

            #region 设置表中常用的参数值

            Code = Code.Replace("@$TABLE_NAME", tableName);
            Code = Code.Replace("@$TABLE__NAME", tableName.Replace(".", "_"));

            Code = Code.Replace("@$TABLE_KEY_TYPE",
                ModelLayerGenerateHelper.FormatDataSqlTypeToSqlDbType(keyType).ToString());
            if (!string.IsNullOrEmpty(keyName))
                Code = Code.Replace("@$TABLE_KEY_type",
                    ModelLayerGenerateHelper.FormatDataType(
                        ModelLayerGenerateHelper.FormatDataSqlTypeToSqlDbType(keyType)));
            else
                Code = Code.Replace("@$TABLE_KEY_type", "");

            Code = Code.Replace("@$TABLE_KEY", keyName);
            Code = Code.Replace("@$KEY_MAX_LENGTH", GetFieldLengthStr(dt, keyName));
            Code = Code.Replace("@$TABLE_COLUMN_@NAME_NOT_KEY",
                ModelLayerGenerateHelper.GetTableColumnName(dt, "@", keyName));
            Code = Code.Replace("@$TABLE_COLUMN_NAME_NOT_KEY",
                ModelLayerGenerateHelper.GetTableColumnName(dt, "", keyName));

            #endregion

            return Code;
        }

        /// <summary>
        ///     获取系统数据库操作方法
        /// </summary>
        /// <returns></returns>
        public string GetSys(CodeGenerate model)
        {
            var sb = new StringBuilder();
            sb.Append(GetCodeForSysPublicClass(model));
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append(GetSys_TableName());
            ModelLayerGenerateHelper.NewLine(sb);


            return sb.ToString();
        }

        #region Code by fei

        private string GetFieldLengthStr(DataTable dt, string Name)
        {
            if (string.IsNullOrEmpty(Name)) return null;
            var dataRow = dt.Select("COLUMN_NAME='" + Name + "'");
            var maxLength =
                ModelLayerGenerateHelper.FormatSqlTypeMaxLength(dataRow[0]["CHARACTER_MAXIMUM_LENGTH"].ToString(),
                    dataRow[0]["DATA_TYPE"].ToString());
            if (!string.IsNullOrEmpty(maxLength))
                maxLength = ", " + maxLength;
            else
                maxLength = "";
            return maxLength;
        }

        private string GetCodeForPublicClass(CodeGenerate model)
        {
            var sb = new StringBuilder();
            sb.Append("public @$TABLE__NAMEDAL()");
            ModelLayerGenerateHelper.NewLine(sb);
            //sb.Append("public static void initConn()"); ModelGenerateHelper.NewLine(sb);
            sb.Append("{");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("DbHelperSQL.connectionString = System.Configuration.ConfigurationManager.ConnectionStrings[\"" +
                      model.ConnectionName + "\"].ToString();");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("}");
            ModelLayerGenerateHelper.NewLine(sb);

            return sb.ToString();
        }


        private string GetCodeForExists(DataTable dt, string KeyName)
        {
            var sb = new StringBuilder();

            #region Template

            sb.Append("public bool Exists(@$TABLE_KEY_type @$TABLE_KEY)");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("{");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("    StringBuilder strSql=new StringBuilder();");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("    strSql.Append(\"select count(1) from @$TABLE_NAME\");");
            ModelLayerGenerateHelper.NewLine(sb);
            if (!string.IsNullOrEmpty(KeyName))
            {
                sb.Append("    strSql.Append(\" where Id=@@$TABLE_KEY \");");
                ModelLayerGenerateHelper.NewLine(sb);
                sb.Append("    SqlParameter[] parameters = {");
                ModelLayerGenerateHelper.NewLine(sb);
                sb.Append("            new SqlParameter(\"@@$TABLE_KEY\", SqlDbType.@$TABLE_KEY_TYPE@$KEY_MAX_LENGTH)};");
                ModelLayerGenerateHelper.NewLine(sb);
                sb.Append("    parameters[0].Value = @$TABLE_KEY;");
                ModelLayerGenerateHelper.NewLine(sb);
                sb.Append("    return DbHelperSQL.Exists(strSql.ToString(),parameters);");
                ModelLayerGenerateHelper.NewLine(sb);
            }
            else
                sb.Append("    return DbHelperSQL.Exists(strSql.ToString());");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("}");
            ModelLayerGenerateHelper.NewLine(sb);

            #endregion

            return sb.ToString();
        }

        private string GetCodeForAdd(DataTable dt, string KeyName)
        {
            var sb = new StringBuilder();

            sb.Append("public int Add(@$TABLE__NAMEModel model)");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("{");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("StringBuilder strSql = new StringBuilder();");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("strSql.Append(\"insert into @$TABLE_NAME(\");");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("strSql.Append(\"@$TABLE_COLUMN_NAME_NOT_KEY)\");");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("strSql.Append(\" values (\");");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("strSql.Append(\"@$TABLE_COLUMN_@NAME_NOT_KEY)\");");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("strSql.Append(\";select @@IDENTITY\");");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("SqlParameter[] parameters = {");
            ModelLayerGenerateHelper.NewLine(sb);

            foreach (DataRow dr in dt.Rows)
            {
                if (dr["COLUMN_NAME"].ToString() != KeyName)
                {
                    sb.Append("		new SqlParameter(\"@" + dr["COLUMN_NAME"] + "\", SqlDbType." +
                              ModelLayerGenerateHelper.FormatDataSqlTypeToSqlDbType(dr["DATA_TYPE"].ToString()) + "" +
                              GetFieldLengthStr(dt, dr["COLUMN_NAME"].ToString()) + "),");
                    ModelLayerGenerateHelper.NewLine(sb);
                }
            }
            sb.Append("     };");
            ModelLayerGenerateHelper.NewLine(sb);

            for (var i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["COLUMN_NAME"].ToString() != KeyName)
                {
                    sb.Append("parameters[" + (i - 1) + "].Value = model." + dt.Rows[i]["COLUMN_NAME"] + ";");
                    ModelLayerGenerateHelper.NewLine(sb);
                }
            }

            sb.Append("object obj = DbHelperSQL.GetSingle(strSql.ToString(), parameters);");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("if (obj == null)");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("    return 0;");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("else");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("    return Convert.ToInt32(obj);");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("}");
            ModelLayerGenerateHelper.NewLine(sb);
            return sb.ToString();
        }

        private string GetCodeForUpdate(DataTable dt, string KeyName)
        {
            var sb = new StringBuilder();

            sb.Append("public bool Update(@$TABLE__NAMEModel model)");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("{");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("StringBuilder strSql=new StringBuilder();");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("strSql.Append(\"update @$TABLE_NAME set \");");
            ModelLayerGenerateHelper.NewLine(sb);
            for (var i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["COLUMN_NAME"].ToString() != KeyName)
                {
                    sb.Append("strSql.Append(\"" + dt.Rows[i]["COLUMN_NAME"] + "=@" + dt.Rows[i]["COLUMN_NAME"] + "");
                    if (i == dt.Rows.Count - 1)
                        sb.Append("\");");
                    else
                        sb.Append(",\");");
                    ModelLayerGenerateHelper.NewLine(sb);
                }
            }
            sb.Append("strSql.Append(\" where @$TABLE_KEY=@@$TABLE_KEY\");");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("SqlParameter[] parameters = {");
            ModelLayerGenerateHelper.NewLine(sb);

            foreach (DataRow dr in dt.Rows)
            {
                sb.Append("		new SqlParameter(\"@" + dr["COLUMN_NAME"] + "\", SqlDbType." +
                          ModelLayerGenerateHelper.FormatDataSqlTypeToSqlDbType(dr["DATA_TYPE"].ToString()) + "" +
                          GetFieldLengthStr(dt, dr["COLUMN_NAME"].ToString()) + "),");
                ModelLayerGenerateHelper.NewLine(sb);
            }
            sb.Append("     };");
            ModelLayerGenerateHelper.NewLine(sb);

            for (var i = 0; i < dt.Rows.Count; i++)
            {
                sb.Append("parameters[" + i + "].Value = model." + dt.Rows[i]["COLUMN_NAME"] + ";");
                ModelLayerGenerateHelper.NewLine(sb);
            }

            sb.Append("int rows=DbHelperSQL.ExecuteSql(strSql.ToString(),parameters);");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("if (rows > 0)");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("	return true;");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("else");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("	return false;");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("}");
            ModelLayerGenerateHelper.NewLine(sb);
            return sb.ToString();
        }

        private string GetCodeForDeleteList()
        {
            var sb = new StringBuilder();
            sb.Append("public bool DeleteList(string @$TABLE_KEYList)");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("{");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("    StringBuilder strSql=new StringBuilder();");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("    strSql.Append(\"delete from @$TABLE_NAME \");");
            ModelLayerGenerateHelper.NewLine(sb);
            //sb.Append("    if(t==0)"); ModelGenerateHelper.NewLine(sb);
            sb.Append("         strSql.Append(\" where Id in (\"+@$TABLE_KEYList + \")  \");");
            ModelLayerGenerateHelper.NewLine(sb);
            //sb.Append("    else"); ModelGenerateHelper.NewLine(sb);
            //sb.Append("         strSql.Append(\" where \"+@$TABLE_KEYList + \"  \");"); ModelGenerateHelper.NewLine(sb);
            sb.Append("    int rows=DbHelperSQL.ExecuteSql(strSql.ToString());");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("    if (rows > 0)");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("	    return true;");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("    else");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("	    return false;");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("}");
            ModelLayerGenerateHelper.NewLine(sb);
            return sb.ToString();
        }

        private string GetCodeForExeSql()
        {
            var sb = new StringBuilder();
            sb.Append("public bool ExeSQL(string sql)");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("{");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("    int rows=DbHelperSQL.ExecuteSql(sql);");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("    if (rows > 0)");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("	    return true;");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("    else");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("	    return false;");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("}");
            ModelLayerGenerateHelper.NewLine(sb);
            return sb.ToString();
        }


        private string GetCodeForDelete()
        {
            var sb = new StringBuilder();
            sb.Append("public bool Delete(@$TABLE_KEY_type @$TABLE_KEY)");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("{");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("StringBuilder strSql = new StringBuilder();");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("strSql.Append(\"delete from @$TABLE_NAME \");");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("strSql.Append(\" where @$TABLE_KEY=@@$TABLE_KEY\");");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append(
                "SqlParameter[] parameters = {new SqlParameter(\"@@$TABLE_KEY\", SqlDbType.@$TABLE_KEY_TYPE@$KEY_MAX_LENGTH)};");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("parameters[0].Value = @$TABLE_KEY;");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("    if (rows > 0)");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("	    return true;");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("    else");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("	    return false;");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("}");
            ModelLayerGenerateHelper.NewLine(sb);
            return sb.ToString();
        }

        private void GetPublicCodeZList(DataTable dt, StringBuilder sb, string template)
        {
            foreach (DataRow dr in dt.Rows)
            {
                var cshipdbType =
                    ModelLayerGenerateHelper.FormatDataType(
                        ModelLayerGenerateHelper.FormatDataSqlTypeToSqlDbType(dr["DATA_TYPE"].ToString()));
                var convertType = template + "[\"" + dr["COLUMN_NAME"] + "\"]";
                if (cshipdbType == "int" || cshipdbType == "decimal" || cshipdbType == "DateTime")
                    convertType = cshipdbType + ".Parse(" + convertType + ".ToString())";
                if (cshipdbType == "byte[]")
                    convertType = "(byte[])" + convertType;
                if (cshipdbType == "string")
                    convertType += ".ToString()";
                if (cshipdbType == "bool")
                {
                    convertType = "(" + template + "[\"" + dr["COLUMN_NAME"] + "\"].ToString()==\"1\")||(" + template +
                                  "[\"" + dr["COLUMN_NAME"] + "\"].ToString().ToLower()==\"true\")?true:false";
                }

                sb.Append("if(" + template + "[\"" + dr["COLUMN_NAME"] + "\"].ToString()!=\"\")");
                ModelLayerGenerateHelper.NewLine(sb);
                sb.Append("model." + dr["COLUMN_NAME"] + " = " + convertType + ";");
                ModelLayerGenerateHelper.NewLine(sb);
            }
        }


        private void GetPublicCodeZList2(DataTable dt, StringBuilder sb, string template)
        {
            foreach (DataRow dr in dt.Rows)
            {
                var cshipdbType =
                    ModelLayerGenerateHelper.FormatDataType(
                        ModelLayerGenerateHelper.FormatDataSqlTypeToSqlDbType(dr["DATA_TYPE"].ToString()));
                var convertType = template + "[\"" + dr["COLUMN_NAME"] + "\"]";
                if (cshipdbType == "int" || cshipdbType == "decimal" || cshipdbType == "DateTime")
                    convertType = cshipdbType + ".Parse(" + convertType + ".ToString())";
                if (cshipdbType == "byte[]")
                    convertType = "(byte[])" + convertType;
                if (cshipdbType == "string")
                    convertType += ".ToString()";
                if (cshipdbType == "bool")
                {
                    convertType = "(" + template + "[\"" + dr["COLUMN_NAME"] + "\"].ToString()==\"1\")||(" + template +
                                  "[\"" + dr["COLUMN_NAME"] + "\"].ToString().ToLower()==\"true\")?true:false";
                }

                sb.Append("if(XINLG.Labs.Utils.ValidateUtil.IsHave(fields,\"" + dr["COLUMN_NAME"] + "\") && " + template +
                          "[\"" + dr["COLUMN_NAME"] + "\"].ToString()!=\"\")");
                ModelLayerGenerateHelper.NewLine(sb);
                sb.Append("model." + dr["COLUMN_NAME"] + " = " + convertType + ";");
                ModelLayerGenerateHelper.NewLine(sb);
            }
        }

        private bool IsHave(string str, string lookstr)
        {
            foreach (var s in str.Split(','))
                if (lookstr == s) return true;
            return false;
        }


        private string GetCodeForGetModel(DataTable dt)
        {
            var sb = new StringBuilder();

            #region Template

            sb.Append("public @$TABLE__NAMEModel GetModel(@$TABLE_KEY_type @$TABLE_KEY)");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("{");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("StringBuilder strSql = new StringBuilder();");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("strSql.Append(\"select top 1 * from @$TABLE_NAME \");");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("strSql.Append(\" where @$TABLE_KEY=@@$TABLE_KEY\");");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append(
                "SqlParameter[] parameters = {new SqlParameter(\"@@$TABLE_KEY\", SqlDbType.@$TABLE_KEY_TYPE@$KEY_MAX_LENGTH)};");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("parameters[0].Value = @$TABLE_KEY;");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("@$TABLE__NAMEModel model = new @$TABLE__NAMEModel();");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("if (ds.Tables[0].Rows.Count > 0)");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("{");
            ModelLayerGenerateHelper.NewLine(sb);

            const string template = "ds.Tables[0].Rows[0]";
            GetPublicCodeZList(dt, sb, template);

            sb.Append("    return model;");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("}");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("return null;");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("}");
            ModelLayerGenerateHelper.NewLine(sb);

            #endregion

            return sb.ToString();
        }


        private string GetCodeForGetModelInSql(DataTable dt)
        {
            var sb = new StringBuilder();

            #region Template

            sb.Append("public @$TABLE__NAMEModel GetModel(string where)");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("{");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("StringBuilder strSql = new StringBuilder();");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("strSql.Append(\"select top 1 * from @$TABLE_NAME \");");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("    if (where.Trim() != \"\") strSql.Append(\" where \" + where);");
            ModelLayerGenerateHelper.NewLine(sb);
            //sb.Append("    SqlDataReader reader=DbHelperSQL.ExecuteReader(strSql.ToString());"); ModelGenerateHelper.NewLine(sb);
            sb.Append("@$TABLE__NAMEModel model = new @$TABLE__NAMEModel();");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("DataSet ds = DbHelperSQL.Query(strSql.ToString());");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("if (ds.Tables[0].Rows.Count > 0)");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("{");
            ModelLayerGenerateHelper.NewLine(sb);

            const string template = "ds.Tables[0].Rows[0]";
            GetPublicCodeZList(dt, sb, template);

            sb.Append("    return model;");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("}");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("return null;");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("}");
            ModelLayerGenerateHelper.NewLine(sb);

            #endregion

            return sb.ToString();
        }


        private string GetCodeForGetList(DataTable dt)
        {
            var sb = new StringBuilder();

            #region Template

            sb.Append("public List<@$TABLE__NAMEModel> GetList(string where)");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("{");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("    List<@$TABLE__NAMEModel> infoList = new List<@$TABLE__NAMEModel>();");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("    StringBuilder strSql = new StringBuilder();");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("    strSql.Append(\"select * from @$TABLE_NAME \");");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("    if (where.Trim() != \"\") strSql.Append(\" where \" + where);");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("    SqlDataReader reader=DbHelperSQL.ExecuteReader(strSql.ToString());");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("    while (reader.Read())");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("    {");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("        @$TABLE__NAMEModel model = new @$TABLE__NAMEModel();");
            ModelLayerGenerateHelper.NewLine(sb);

            const string template = "reader";
            GetPublicCodeZList(dt, sb, template);

            sb.Append("infoList.Add(model);");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("    }");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("    reader.Close();");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("    return infoList;");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("}");
            ModelLayerGenerateHelper.NewLine(sb);

            #endregion

            return sb.ToString();
        }

        private string GetCodeForGetList2(DataTable dt)
        {
            var sb = new StringBuilder();

            #region Template

            sb.Append("public List<@$TABLE__NAMEModel> GetModelList(int top,string fields,string where,string order)");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("{");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("    List<@$TABLE__NAMEModel> infoList = new List<@$TABLE__NAMEModel>();");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("    StringBuilder strSql = new StringBuilder();");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("    strSql.Append(\"select top \"+top+\" \"+fields+\" from @$TABLE_NAME \");");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("    if (where.Trim() != \"\") strSql.Append(\" where \" + where);");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("    if (order.Trim() != \"\") strSql.Append(\" order by \" + order);");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("    SqlDataReader reader=DbHelperSQL.ExecuteReader(strSql.ToString());");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("    while (reader.Read())");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("    {");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("        @$TABLE__NAMEModel model = new @$TABLE__NAMEModel();");
            ModelLayerGenerateHelper.NewLine(sb);

            const string template = "reader";
            GetPublicCodeZList(dt, sb, template);

            sb.Append("infoList.Add(model);");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("    }");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("    reader.Close();");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("    return infoList;");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("}");
            ModelLayerGenerateHelper.NewLine(sb);

            #endregion

            return sb.ToString();
        }


        private string GetCodeForGetListQ(DataTable dt)
        {
            var sb = new StringBuilder();

            #region Template

            sb.Append("public DataTable GetListSQL(int top,string fields,string where,string order)");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("{");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("    StringBuilder strSql = new StringBuilder();");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("    strSql.Append(\"select \");");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("    if (top > 0) strSql.Append(\" top \" + top);");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("    strSql.Append(\" \"+fields+\" from @$TABLE_NAME \"); ");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("    if (where.Trim() != \"\") strSql.Append(\" where \" + where);");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("    if (order.Trim() != \"\") strSql.Append(\" order by \" + order);");
            ModelLayerGenerateHelper.NewLine(sb);

            sb.Append("    DataSet ds = DbHelperSQL.Query(strSql.ToString());");
            ModelLayerGenerateHelper.NewLine(sb);
            //sb.Append("    System.Web.HttpContext.Current.Response.Write(strSql);System.Web.HttpContext.Current.Response.End();"); ModelGenerateHelper.NewLine(sb);
            sb.Append("    if (ds.Tables.Count > 0)");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("        return ds.Tables[0];");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("    else");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("        return null;");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("}");
            ModelLayerGenerateHelper.NewLine(sb);

            #endregion

            return sb.ToString();
        }


        private string GetCodeForGetListInSql()
        {
            var sb = new StringBuilder();

            #region Template

            sb.Append("public DataTable GetListForSQL(string Sql)");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("{");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("    DataSet ds = DbHelperSQL.Query(Sql);");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("    if (ds.Tables.Count > 0)");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("        return ds.Tables[0];");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("    else");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("        return null;");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("}");
            ModelLayerGenerateHelper.NewLine(sb);

            #endregion

            return sb.ToString();
        }


        private string GetCodeForGetListInPage()
        {
            var sb = new StringBuilder();

            sb.Append(
                "public DataTable GetListByPage(string strWhere, string fieldList, string orderby, int pz, int pi, out int count)");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("{");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("    StringBuilder strSql = new StringBuilder();");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("int page_statrt = (pi-1) * pz;");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("strSql.Append(\"SELECT \"+fieldList+\" FROM ( \");");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("strSql.Append(\" SELECT ROW_NUMBER() OVER (\");");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("if (!string.IsNullOrEmpty(orderby.Trim()))");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("    strSql.Append(\"order by T.\" + orderby);");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("else");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("    strSql.Append(\"order by T.id desc\");");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("strSql.Append(\")AS Row, T.*  from @$TABLE_NAME T \");");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("if (!string.IsNullOrEmpty(strWhere.Trim()))");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("    strSql.Append(\" WHERE \" + strWhere);");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("strSql.Append(\" ) TT\");");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("strSql.AppendFormat(\" WHERE TT.Row between {0} and {1}\", page_statrt+1, page_statrt+pz);");
            ModelLayerGenerateHelper.NewLine(sb);
            //sb.Append("    string countSql = strSql.ToString().Replace(\"SELECT *\", \"SELECT count(*)\");"); ModelGenerateHelper.NewLine(sb);
            sb.Append("    count = 0;");
            ModelLayerGenerateHelper.NewLine(sb);
            //sb.Append("    System.Web.HttpContext.Current.Response.Write(strSql.ToString());"); ModelGenerateHelper.NewLine(sb);
            sb.Append(
                "string countSql = strSql.ToString().Replace(\"SELECT \" + fieldList + \"\", \"select count(*)\");");
            ModelLayerGenerateHelper.NewLine(sb);
            //sb.Append(""); ModelGenerateHelper.NewLine(sb);
            sb.Append("int i = countSql.LastIndexOf(\"WHERE\");");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("countSql=countSql.Substring(0,i);");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("    count = Convert.ToInt32(DbHelperSQL.GetSingle(countSql));");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("    return DbHelperSQL.Query(strSql.ToString()).Tables[0];");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("}");
            ModelLayerGenerateHelper.NewLine(sb);

            return sb.ToString();
        }


        private string GetCodeForGetListInPageOld()
        {
            var sb = new StringBuilder();
            sb.Append(
                "public DataSet GetList(int PageSize,int PageIndex,string fieldList,string strWhere,string Order,int SortType)");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("{");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("    //strWhere=string.IsNullOrEmpty(strWhere)?\"@$TABLE_KEY<>NULL\":strWhere;");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("    SqlParameter[] parameters = {");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("		    new SqlParameter(\"@TableName\", SqlDbType.VarChar, 200),");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("		    new SqlParameter(\"@FieldList\", SqlDbType.VarChar, 2000),");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("		    new SqlParameter(\"@PrimaryKey\", SqlDbType.VarChar, 100),");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("		    new SqlParameter(\"@Where\", SqlDbType.VarChar, 2000),");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("		    new SqlParameter(\"@Order\", SqlDbType.VarChar, 1000),");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("		    new SqlParameter(\"@SortType\", SqlDbType.Int),");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("		    new SqlParameter(\"@RecorderCount\", SqlDbType.Int),");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("		    new SqlParameter(\"@PageSize\", SqlDbType.Int),");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("		    new SqlParameter(\"@PageIndex\", SqlDbType.Int),");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("		    new SqlParameter(\"@TotalCount\", SqlDbType.Int),");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("		    new SqlParameter(\"@TotalPageCount\", SqlDbType.Int)");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("		    };");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("    parameters[0].Value = \"@$TABLE_NAME\";");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("    parameters[1].Value = fieldList;");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("    parameters[2].Value = \"@$TABLE_KEY\";");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("    parameters[3].Value = strWhere;");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("    parameters[4].Value = Order;");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("    parameters[5].Value = SortType;");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("    parameters[6].Value = 0;");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("    parameters[7].Value = PageSize;");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("    parameters[8].Value = PageIndex;");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("    parameters[9].Value = 1;");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("    parameters[9].Direction = ParameterDirection.Output;");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("    parameters[10].Value = 1;");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("    parameters[10].Direction = ParameterDirection.Output;");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("    DataSet ds = DbHelperSQL.RunProcedure(\"P_viewPage\",parameters,\"ds\");");
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("    DataTable dtc = new DataTable();");
            sb.Append("    dtc.Columns.Add(\"count\");");
            sb.Append("    dtc.Columns.Add(\"pagecount\");");
            sb.Append("    DataRow dr = dtc.NewRow();");
            sb.Append("    dr[\"count\"] = parameters[9].Value.ToString();");
            sb.Append("    dr[\"pagecount\"] = parameters[10].Value.ToString();");
            sb.Append("    dtc.Rows.Add(dr);");
            sb.Append("    ds.Tables.Add(dtc);");
            sb.Append("    return ds;");
            sb.Append("}");
            ModelLayerGenerateHelper.NewLine(sb);
            return sb.ToString();
        }

        #endregion
    }
}