using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Fosc.Dolphin.Object.AutoCode;

namespace Fosc.Dolphin.Common.AutoCode
{
    public class SqlServer
    {
        /// <summary>
        ///  获取一张表的数据操作方法
        /// </summary>
        /// <param name="DataTableName"></param>
        /// <param name="IsAddNamespace"></param>
        /// <returns></returns>
        public string GetDataTableDAL(string DataTableName, bool IsAddNamespace, CodeGenerate model)
        {
            DataTable dt = SqlServerSysObjectHelper.GetDataTableColumn(DataTableName);
            if (dt == null) return null;
            //包装一个类
            string code = ModelGenerateHelper.GetUserPartialCode(model, Get(dt, model), "I" + DataTableName.Replace(".", "_"));
            if (code.IndexOf("err",StringComparison.Ordinal) >= 0) return null;

            //包装命名空间
            if (IsAddNamespace)
                code = ModelGenerateHelper.GetUserNamespaceCode(model, code);
            return code;
        }

        #region Code by fei

        private string GetFieldLengthStr(DataTable dt, string Name)
        {
            if (string.IsNullOrEmpty(Name)) return null;
            DataRow[] DataRow = dt.Select("COLUMN_NAME='" + Name + "'");
            string MaxLength = ModelGenerateHelper.FormatSqlTypeMaxLength(DataRow[0]["CHARACTER_MAXIMUM_LENGTH"].ToString(), DataRow[0]["DATA_TYPE"].ToString());
            if (!string.IsNullOrEmpty(MaxLength))
                MaxLength = ", " + MaxLength;
            else
                MaxLength = "";
            return MaxLength;
        }

        private string GetCodeForPublicClass(CodeGenerate model)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("public @$TABLE__NAMEDAL()"); ModelGenerateHelper.NewLine(sb);
            //sb.Append("public static void initConn()"); ModelGenerateHelper.NewLine(sb);
            sb.Append("{"); ModelGenerateHelper.NewLine(sb);
            sb.Append("DbHelperSQL.connectionString = System.Configuration.ConfigurationManager.ConnectionStrings[\"" + model.ConnectionName + "\"].ToString();"); ModelGenerateHelper.NewLine(sb);
            sb.Append("}"); ModelGenerateHelper.NewLine(sb);

            return sb.ToString();
        }


        private string GetCodeForExists(DataTable dt, string KeyName)
        {
            StringBuilder sb = new StringBuilder();

            #region Template
            sb.Append("public bool Exists(@$TABLE_KEY_type @$TABLE_KEY)"); ModelGenerateHelper.NewLine(sb);
            sb.Append("{"); ModelGenerateHelper.NewLine(sb);
            sb.Append("    StringBuilder strSql=new StringBuilder();"); ModelGenerateHelper.NewLine(sb);
            sb.Append("    strSql.Append(\"select count(1) from @$TABLE_NAME\");"); ModelGenerateHelper.NewLine(sb);
            if (!string.IsNullOrEmpty(KeyName))
            {
                sb.Append("    strSql.Append(\" where Id=@@$TABLE_KEY \");"); ModelGenerateHelper.NewLine(sb);
                sb.Append("    SqlParameter[] parameters = {"); ModelGenerateHelper.NewLine(sb);
                sb.Append("            new SqlParameter(\"@@$TABLE_KEY\", SqlDbType.@$TABLE_KEY_TYPE@$KEY_MAX_LENGTH)};"); ModelGenerateHelper.NewLine(sb);
                sb.Append("    parameters[0].Value = @$TABLE_KEY;"); ModelGenerateHelper.NewLine(sb);
                sb.Append("    return DbHelperSQL.Exists(strSql.ToString(),parameters);"); ModelGenerateHelper.NewLine(sb);
            }
            else
                sb.Append("    return DbHelperSQL.Exists(strSql.ToString());"); ModelGenerateHelper.NewLine(sb);
            sb.Append("}"); ModelGenerateHelper.NewLine(sb);

            #endregion

            return sb.ToString();
        }

        private string GetCodeForAdd(DataTable dt, string KeyName)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("public int Add(@$TABLE__NAMEModel model)"); ModelGenerateHelper.NewLine(sb);
            sb.Append("{"); ModelGenerateHelper.NewLine(sb);
            sb.Append("StringBuilder strSql = new StringBuilder();"); ModelGenerateHelper.NewLine(sb);
            sb.Append("strSql.Append(\"insert into @$TABLE_NAME(\");"); ModelGenerateHelper.NewLine(sb);
            sb.Append("strSql.Append(\"@$TABLE_COLUMN_NAME_NOT_KEY)\");"); ModelGenerateHelper.NewLine(sb);
            sb.Append("strSql.Append(\" values (\");"); ModelGenerateHelper.NewLine(sb);
            sb.Append("strSql.Append(\"@$TABLE_COLUMN_@NAME_NOT_KEY)\");"); ModelGenerateHelper.NewLine(sb);
            sb.Append("strSql.Append(\";select @@IDENTITY\");"); ModelGenerateHelper.NewLine(sb);
            sb.Append("SqlParameter[] parameters = {"); ModelGenerateHelper.NewLine(sb);

            foreach (DataRow dr in dt.Rows)
            {
                if (dr["COLUMN_NAME"].ToString() != KeyName)
                {
                    sb.Append("		new SqlParameter(\"@" + dr["COLUMN_NAME"].ToString() + "\", SqlDbType." + ModelGenerateHelper.FormatDataSqlTypeToSqlDbType(dr["DATA_TYPE"].ToString()).ToString() + "" + GetFieldLengthStr(dt, dr["COLUMN_NAME"].ToString()) + "),"); ModelGenerateHelper.NewLine(sb);
                }
            }
            sb.Append("     };"); ModelGenerateHelper.NewLine(sb);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["COLUMN_NAME"].ToString() != KeyName)
                {
                    sb.Append("parameters[" + (i - 1) + "].Value = model." + dt.Rows[i]["COLUMN_NAME"].ToString() + ";");
                    ModelGenerateHelper.NewLine(sb);
                }
            }

            sb.Append("object obj = DbHelperSQL.GetSingle(strSql.ToString(), parameters);"); ModelGenerateHelper.NewLine(sb);
            sb.Append("if (obj == null)"); ModelGenerateHelper.NewLine(sb);
            sb.Append("    return 0;"); ModelGenerateHelper.NewLine(sb);
            sb.Append("else"); ModelGenerateHelper.NewLine(sb);
            sb.Append("    return Convert.ToInt32(obj);"); ModelGenerateHelper.NewLine(sb);
            sb.Append("}"); ModelGenerateHelper.NewLine(sb);
            return sb.ToString();
        }

        private string GetCodeForUpdate(DataTable dt, string KeyName)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("public bool Update(@$TABLE__NAMEModel model)"); ModelGenerateHelper.NewLine(sb);
            sb.Append("{"); ModelGenerateHelper.NewLine(sb);
            sb.Append("StringBuilder strSql=new StringBuilder();"); ModelGenerateHelper.NewLine(sb);
            sb.Append("strSql.Append(\"update @$TABLE_NAME set \");"); ModelGenerateHelper.NewLine(sb);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["COLUMN_NAME"].ToString() != KeyName)
                {
                    sb.Append("strSql.Append(\"" + dt.Rows[i]["COLUMN_NAME"].ToString() + "=@" + dt.Rows[i]["COLUMN_NAME"].ToString() + "");
                    if (i == dt.Rows.Count - 1)
                        sb.Append("\");");
                    else
                        sb.Append(",\");");
                    ModelGenerateHelper.NewLine(sb);
                }
            }
            sb.Append("strSql.Append(\" where @$TABLE_KEY=@@$TABLE_KEY\");"); ModelGenerateHelper.NewLine(sb);
            sb.Append("SqlParameter[] parameters = {"); ModelGenerateHelper.NewLine(sb);

            foreach (DataRow dr in dt.Rows)
            {
                sb.Append("		new SqlParameter(\"@" + dr["COLUMN_NAME"].ToString() + "\", SqlDbType." + ModelGenerateHelper.FormatDataSqlTypeToSqlDbType(dr["DATA_TYPE"].ToString()).ToString() + "" + GetFieldLengthStr(dt, dr["COLUMN_NAME"].ToString()) + "),");
                ModelGenerateHelper.NewLine(sb);
            }
            sb.Append("     };"); ModelGenerateHelper.NewLine(sb);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sb.Append("parameters[" + i + "].Value = model." + dt.Rows[i]["COLUMN_NAME"].ToString() + ";");
                ModelGenerateHelper.NewLine(sb);
            }

            sb.Append("int rows=DbHelperSQL.ExecuteSql(strSql.ToString(),parameters);"); ModelGenerateHelper.NewLine(sb);
            sb.Append("if (rows > 0)"); ModelGenerateHelper.NewLine(sb);
            sb.Append("	return true;"); ModelGenerateHelper.NewLine(sb);
            sb.Append("else"); ModelGenerateHelper.NewLine(sb);
            sb.Append("	return false;"); ModelGenerateHelper.NewLine(sb);
            sb.Append("}"); ModelGenerateHelper.NewLine(sb);
            return sb.ToString();
        }

        private string GetCodeForDeleteList()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("public bool DeleteList(string @$TABLE_KEYList)"); ModelGenerateHelper.NewLine(sb);
            sb.Append("{"); ModelGenerateHelper.NewLine(sb);
            sb.Append("    StringBuilder strSql=new StringBuilder();"); ModelGenerateHelper.NewLine(sb);
            sb.Append("    strSql.Append(\"delete from @$TABLE_NAME \");"); ModelGenerateHelper.NewLine(sb);
            //sb.Append("    if(t==0)"); ModelGenerateHelper.NewLine(sb);
            sb.Append("         strSql.Append(\" where Id in (\"+@$TABLE_KEYList + \")  \");"); ModelGenerateHelper.NewLine(sb);
            //sb.Append("    else"); ModelGenerateHelper.NewLine(sb);
            //sb.Append("         strSql.Append(\" where \"+@$TABLE_KEYList + \"  \");"); ModelGenerateHelper.NewLine(sb);
            sb.Append("    int rows=DbHelperSQL.ExecuteSql(strSql.ToString());"); ModelGenerateHelper.NewLine(sb);
            sb.Append("    if (rows > 0)"); ModelGenerateHelper.NewLine(sb);
            sb.Append("	    return true;"); ModelGenerateHelper.NewLine(sb);
            sb.Append("    else"); ModelGenerateHelper.NewLine(sb);
            sb.Append("	    return false;"); ModelGenerateHelper.NewLine(sb);
            sb.Append("}"); ModelGenerateHelper.NewLine(sb);
            return sb.ToString();


        }

        private string GetCodeForExeSQL()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("public bool ExeSQL(string sql)"); ModelGenerateHelper.NewLine(sb);
            sb.Append("{"); ModelGenerateHelper.NewLine(sb);
            sb.Append("    int rows=DbHelperSQL.ExecuteSql(sql);"); ModelGenerateHelper.NewLine(sb);
            sb.Append("    if (rows > 0)"); ModelGenerateHelper.NewLine(sb);
            sb.Append("	    return true;"); ModelGenerateHelper.NewLine(sb);
            sb.Append("    else"); ModelGenerateHelper.NewLine(sb);
            sb.Append("	    return false;"); ModelGenerateHelper.NewLine(sb);
            sb.Append("}"); ModelGenerateHelper.NewLine(sb);
            return sb.ToString();


        }



        private string GetCodeForDelete()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("public bool Delete(@$TABLE_KEY_type @$TABLE_KEY)"); ModelGenerateHelper.NewLine(sb);
            sb.Append("{"); ModelGenerateHelper.NewLine(sb);
            sb.Append("StringBuilder strSql = new StringBuilder();"); ModelGenerateHelper.NewLine(sb);
            sb.Append("strSql.Append(\"delete from @$TABLE_NAME \");"); ModelGenerateHelper.NewLine(sb);
            sb.Append("strSql.Append(\" where @$TABLE_KEY=@@$TABLE_KEY\");"); ModelGenerateHelper.NewLine(sb);
            sb.Append("SqlParameter[] parameters = {new SqlParameter(\"@@$TABLE_KEY\", SqlDbType.@$TABLE_KEY_TYPE@$KEY_MAX_LENGTH)};"); ModelGenerateHelper.NewLine(sb);
            sb.Append("parameters[0].Value = @$TABLE_KEY;"); ModelGenerateHelper.NewLine(sb);
            sb.Append("int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);"); ModelGenerateHelper.NewLine(sb);
            sb.Append("    if (rows > 0)"); ModelGenerateHelper.NewLine(sb);
            sb.Append("	    return true;"); ModelGenerateHelper.NewLine(sb);
            sb.Append("    else"); ModelGenerateHelper.NewLine(sb);
            sb.Append("	    return false;"); ModelGenerateHelper.NewLine(sb);
            sb.Append("}"); ModelGenerateHelper.NewLine(sb);
            return sb.ToString();
        }

        private void GetPublicCodeZList(DataTable dt, StringBuilder sb, string template)
        {

            foreach (DataRow dr in dt.Rows)
            {
                string CshipdbType = ModelGenerateHelper.FormatDataType(ModelGenerateHelper.FormatDataSqlTypeToSqlDbType(dr["DATA_TYPE"].ToString()));
                string ConvertType = template + "[\"" + dr["COLUMN_NAME"].ToString() + "\"]";
                if (CshipdbType == "int" || CshipdbType == "decimal" || CshipdbType == "DateTime")
                    ConvertType = CshipdbType + ".Parse(" + ConvertType + ".ToString())";
                if (CshipdbType == "byte[]")
                    ConvertType = "(byte[])" + ConvertType;
                if (CshipdbType == "string")
                    ConvertType += ".ToString()";
                if (CshipdbType == "bool")
                {
                    ConvertType = "(" + template + "[\"" + dr["COLUMN_NAME"].ToString() + "\"].ToString()==\"1\")||(" + template + "[\"" + dr["COLUMN_NAME"].ToString() + "\"].ToString().ToLower()==\"true\")?true:false";
                }

                sb.Append("if(" + template + "[\"" + dr["COLUMN_NAME"].ToString() + "\"].ToString()!=\"\")");
                ModelGenerateHelper.NewLine(sb);
                sb.Append("model." + dr["COLUMN_NAME"].ToString() + " = " + ConvertType + ";");
                ModelGenerateHelper.NewLine(sb);
            }
        }


        private void GetPublicCodeZList2(DataTable dt, StringBuilder sb, string template)
        {
            foreach (DataRow dr in dt.Rows)
            {
                string CshipdbType = ModelGenerateHelper.FormatDataType(ModelGenerateHelper.FormatDataSqlTypeToSqlDbType(dr["DATA_TYPE"].ToString()));
                string ConvertType = template + "[\"" + dr["COLUMN_NAME"].ToString() + "\"]";
                if (CshipdbType == "int" || CshipdbType == "decimal" || CshipdbType == "DateTime")
                    ConvertType = CshipdbType + ".Parse(" + ConvertType + ".ToString())";
                if (CshipdbType == "byte[]")
                    ConvertType = "(byte[])" + ConvertType;
                if (CshipdbType == "string")
                    ConvertType += ".ToString()";
                if (CshipdbType == "bool")
                {
                    ConvertType = "(" + template + "[\"" + dr["COLUMN_NAME"].ToString() + "\"].ToString()==\"1\")||(" + template + "[\"" + dr["COLUMN_NAME"].ToString() + "\"].ToString().ToLower()==\"true\")?true:false";
                }

                sb.Append("if(XINLG.Labs.Utils.ValidateUtil.IsHave(fields,\"" + dr["COLUMN_NAME"].ToString() + "\") && " + template + "[\"" + dr["COLUMN_NAME"].ToString() + "\"].ToString()!=\"\")");
                ModelGenerateHelper.NewLine(sb);
                sb.Append("model." + dr["COLUMN_NAME"].ToString() + " = " + ConvertType + ";");
                ModelGenerateHelper.NewLine(sb);
            }
        }

        private bool isHave(string str, string lookstr)
        {
            foreach (string s in str.Split(','))
                if (lookstr == s) return true;
            return false;
        }




        private string GetCodeForGetModel(DataTable dt)
        {
            StringBuilder sb = new StringBuilder();

            #region Template
            sb.Append("public @$TABLE__NAMEModel GetModel(@$TABLE_KEY_type @$TABLE_KEY)"); ModelGenerateHelper.NewLine(sb);
            sb.Append("{"); ModelGenerateHelper.NewLine(sb);
            sb.Append("StringBuilder strSql = new StringBuilder();"); ModelGenerateHelper.NewLine(sb);
            sb.Append("strSql.Append(\"select top 1 * from @$TABLE_NAME \");"); ModelGenerateHelper.NewLine(sb);
            sb.Append("strSql.Append(\" where @$TABLE_KEY=@@$TABLE_KEY\");"); ModelGenerateHelper.NewLine(sb);
            sb.Append("SqlParameter[] parameters = {new SqlParameter(\"@@$TABLE_KEY\", SqlDbType.@$TABLE_KEY_TYPE@$KEY_MAX_LENGTH)};"); ModelGenerateHelper.NewLine(sb);
            sb.Append("parameters[0].Value = @$TABLE_KEY;"); ModelGenerateHelper.NewLine(sb);
            sb.Append("@$TABLE__NAMEModel model = new @$TABLE__NAMEModel();"); ModelGenerateHelper.NewLine(sb);
            sb.Append("DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);"); ModelGenerateHelper.NewLine(sb);
            sb.Append("if (ds.Tables[0].Rows.Count > 0)"); ModelGenerateHelper.NewLine(sb);
            sb.Append("{"); ModelGenerateHelper.NewLine(sb);

            string template = "ds.Tables[0].Rows[0]";
            GetPublicCodeZList(dt, sb, template);

            sb.Append("    return model;"); ModelGenerateHelper.NewLine(sb);
            sb.Append("}"); ModelGenerateHelper.NewLine(sb);
            sb.Append("return null;"); ModelGenerateHelper.NewLine(sb);
            sb.Append("}"); ModelGenerateHelper.NewLine(sb);

            #endregion

            return sb.ToString();
        }


        private string GetCodeForGetModelInSql(DataTable dt)
        {
            StringBuilder sb = new StringBuilder();

            #region Template
            sb.Append("public @$TABLE__NAMEModel GetModel(string where)"); ModelGenerateHelper.NewLine(sb);
            sb.Append("{"); ModelGenerateHelper.NewLine(sb);
            sb.Append("StringBuilder strSql = new StringBuilder();"); ModelGenerateHelper.NewLine(sb);
            sb.Append("strSql.Append(\"select top 1 * from @$TABLE_NAME \");"); ModelGenerateHelper.NewLine(sb);
            sb.Append("    if (where.Trim() != \"\") strSql.Append(\" where \" + where);"); ModelGenerateHelper.NewLine(sb);
            //sb.Append("    SqlDataReader reader=DbHelperSQL.ExecuteReader(strSql.ToString());"); ModelGenerateHelper.NewLine(sb);
            sb.Append("@$TABLE__NAMEModel model = new @$TABLE__NAMEModel();"); ModelGenerateHelper.NewLine(sb);
            sb.Append("DataSet ds = DbHelperSQL.Query(strSql.ToString());"); ModelGenerateHelper.NewLine(sb);
            sb.Append("if (ds.Tables[0].Rows.Count > 0)"); ModelGenerateHelper.NewLine(sb);
            sb.Append("{"); ModelGenerateHelper.NewLine(sb);

            string template = "ds.Tables[0].Rows[0]";
            GetPublicCodeZList(dt, sb, template);

            sb.Append("    return model;"); ModelGenerateHelper.NewLine(sb);
            sb.Append("}"); ModelGenerateHelper.NewLine(sb);
            sb.Append("return null;"); ModelGenerateHelper.NewLine(sb);
            sb.Append("}"); ModelGenerateHelper.NewLine(sb);

            #endregion

            return sb.ToString();
        }


        private string GetCodeForGetList(DataTable dt)
        {
            StringBuilder sb = new StringBuilder();
            #region Template
            sb.Append("public List<@$TABLE__NAMEModel> GetList(string where)"); ModelGenerateHelper.NewLine(sb);
            sb.Append("{"); ModelGenerateHelper.NewLine(sb);
            sb.Append("    List<@$TABLE__NAMEModel> infoList = new List<@$TABLE__NAMEModel>();"); ModelGenerateHelper.NewLine(sb);
            sb.Append("    StringBuilder strSql = new StringBuilder();"); ModelGenerateHelper.NewLine(sb);
            sb.Append("    strSql.Append(\"select * from @$TABLE_NAME \");"); ModelGenerateHelper.NewLine(sb);
            sb.Append("    if (where.Trim() != \"\") strSql.Append(\" where \" + where);"); ModelGenerateHelper.NewLine(sb);
            sb.Append("    SqlDataReader reader=DbHelperSQL.ExecuteReader(strSql.ToString());"); ModelGenerateHelper.NewLine(sb);
            sb.Append("    while (reader.Read())"); ModelGenerateHelper.NewLine(sb);
            sb.Append("    {"); ModelGenerateHelper.NewLine(sb);
            sb.Append("        @$TABLE__NAMEModel model = new @$TABLE__NAMEModel();"); ModelGenerateHelper.NewLine(sb);

            string template = "reader";
            GetPublicCodeZList(dt, sb, template);

            sb.Append("infoList.Add(model);"); ModelGenerateHelper.NewLine(sb);
            sb.Append("    }"); ModelGenerateHelper.NewLine(sb);
            sb.Append("    reader.Close();"); ModelGenerateHelper.NewLine(sb);
            sb.Append("    return infoList;"); ModelGenerateHelper.NewLine(sb);
            sb.Append("}"); ModelGenerateHelper.NewLine(sb);
            #endregion
            return sb.ToString();
        }

        private string GetCodeForGetList2(DataTable dt)
        {
            StringBuilder sb = new StringBuilder();
            #region Template
            sb.Append("public List<@$TABLE__NAMEModel> GetModelList(int top,string fields,string where,string order)"); ModelGenerateHelper.NewLine(sb);
            sb.Append("{"); ModelGenerateHelper.NewLine(sb);
            sb.Append("    List<@$TABLE__NAMEModel> infoList = new List<@$TABLE__NAMEModel>();"); ModelGenerateHelper.NewLine(sb);
            sb.Append("    StringBuilder strSql = new StringBuilder();"); ModelGenerateHelper.NewLine(sb);
            sb.Append("    strSql.Append(\"select top \"+top+\" \"+fields+\" from @$TABLE_NAME \");"); ModelGenerateHelper.NewLine(sb);
            sb.Append("    if (where.Trim() != \"\") strSql.Append(\" where \" + where);"); ModelGenerateHelper.NewLine(sb);
            sb.Append("    if (order.Trim() != \"\") strSql.Append(\" order by \" + order);"); ModelGenerateHelper.NewLine(sb);
            sb.Append("    SqlDataReader reader=DbHelperSQL.ExecuteReader(strSql.ToString());"); ModelGenerateHelper.NewLine(sb);
            sb.Append("    while (reader.Read())"); ModelGenerateHelper.NewLine(sb);
            sb.Append("    {"); ModelGenerateHelper.NewLine(sb);
            sb.Append("        @$TABLE__NAMEModel model = new @$TABLE__NAMEModel();"); ModelGenerateHelper.NewLine(sb);

            string template = "reader";
            GetPublicCodeZList(dt, sb, template);

            sb.Append("infoList.Add(model);"); ModelGenerateHelper.NewLine(sb);
            sb.Append("    }"); ModelGenerateHelper.NewLine(sb);
            sb.Append("    reader.Close();"); ModelGenerateHelper.NewLine(sb);
            sb.Append("    return infoList;"); ModelGenerateHelper.NewLine(sb);
            sb.Append("}"); ModelGenerateHelper.NewLine(sb);
            #endregion
            return sb.ToString();
        }




        private string GetCodeForGetListQ(DataTable dt)
        {
            StringBuilder sb = new StringBuilder();
            #region Template
            sb.Append("public DataTable GetListSQL(int top,string fields,string where,string order)"); ModelGenerateHelper.NewLine(sb);
            sb.Append("{"); ModelGenerateHelper.NewLine(sb);
            sb.Append("    StringBuilder strSql = new StringBuilder();"); ModelGenerateHelper.NewLine(sb);
            sb.Append("    strSql.Append(\"select \");"); ModelGenerateHelper.NewLine(sb);
            sb.Append("    if (top > 0) strSql.Append(\" top \" + top);"); ModelGenerateHelper.NewLine(sb);
            sb.Append("    strSql.Append(\" \"+fields+\" from @$TABLE_NAME \"); "); ModelGenerateHelper.NewLine(sb);
            sb.Append("    if (where.Trim() != \"\") strSql.Append(\" where \" + where);"); ModelGenerateHelper.NewLine(sb);
            sb.Append("    if (order.Trim() != \"\") strSql.Append(\" order by \" + order);"); ModelGenerateHelper.NewLine(sb);

            sb.Append("    DataSet ds = DbHelperSQL.Query(strSql.ToString());"); ModelGenerateHelper.NewLine(sb);
            //sb.Append("    System.Web.HttpContext.Current.Response.Write(strSql);System.Web.HttpContext.Current.Response.End();"); ModelGenerateHelper.NewLine(sb);
            sb.Append("    if (ds.Tables.Count > 0)"); ModelGenerateHelper.NewLine(sb);
            sb.Append("        return ds.Tables[0];"); ModelGenerateHelper.NewLine(sb);
            sb.Append("    else"); ModelGenerateHelper.NewLine(sb);
            sb.Append("        return null;"); ModelGenerateHelper.NewLine(sb);
            sb.Append("}"); ModelGenerateHelper.NewLine(sb);
            #endregion
            return sb.ToString();
        }



        private string GetCodeForGetListInSql()
        {
            StringBuilder sb = new StringBuilder();
            #region Template
            sb.Append("public DataTable GetListForSQL(string Sql)"); ModelGenerateHelper.NewLine(sb);
            sb.Append("{"); ModelGenerateHelper.NewLine(sb);
            sb.Append("    DataSet ds = DbHelperSQL.Query(Sql);"); ModelGenerateHelper.NewLine(sb);
            sb.Append("    if (ds.Tables.Count > 0)"); ModelGenerateHelper.NewLine(sb);
            sb.Append("        return ds.Tables[0];"); ModelGenerateHelper.NewLine(sb);
            sb.Append("    else"); ModelGenerateHelper.NewLine(sb);
            sb.Append("        return null;"); ModelGenerateHelper.NewLine(sb);
            sb.Append("}"); ModelGenerateHelper.NewLine(sb);
            #endregion
            return sb.ToString();
        }


        private string GetCodeForGetListInPage()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("public DataTable GetListByPage(string strWhere, string fieldList, string orderby, int pz, int pi, out int count)"); ModelGenerateHelper.NewLine(sb);
            sb.Append("{"); ModelGenerateHelper.NewLine(sb);
            sb.Append("    StringBuilder strSql = new StringBuilder();"); ModelGenerateHelper.NewLine(sb);
            //sb.Append("    if (string.IsNullOrEmpty(fieldList)) fieldList = \"*\";"); ModelGenerateHelper.NewLine(sb);
            //sb.Append("    strSql.Append(\"SELECT \" + fieldList + \" FROM @$TABLE_NAME\");"); ModelGenerateHelper.NewLine(sb);
            //sb.Append("    if (!string.IsNullOrEmpty(strWhere.Trim()))"); ModelGenerateHelper.NewLine(sb);
            //sb.Append("        strSql.Append(\" WHERE \" + strWhere);"); ModelGenerateHelper.NewLine(sb);
            //sb.Append("    if (!string.IsNullOrEmpty(orderby.Trim()))"); ModelGenerateHelper.NewLine(sb);
            //sb.Append("        strSql.Append(\" order by \" + orderby);"); ModelGenerateHelper.NewLine(sb);
            //sb.Append("    int page_statrt = (pi - 1) * pz;"); ModelGenerateHelper.NewLine(sb);
            //sb.Append("    string countSql = strSql.ToString().Replace(\"*\", \"count(*)\");"); ModelGenerateHelper.NewLine(sb);
            //sb.Append("    count = Convert.ToInt32(DbHelperSQL.GetSingle(countSql));"); ModelGenerateHelper.NewLine(sb);
            //sb.Append("    strSql.AppendFormat(\" LIMIT {0},{1} \", page_statrt, pz);"); ModelGenerateHelper.NewLine(sb);
            //sb.Append("    System.Web.HttpContext.Current.Response.Write(strSql.ToString());return null;"); ModelGenerateHelper.NewLine(sb);

            sb.Append("int page_statrt = (pi-1) * pz;"); ModelGenerateHelper.NewLine(sb);
            sb.Append("strSql.Append(\"SELECT \"+fieldList+\" FROM ( \");"); ModelGenerateHelper.NewLine(sb);
            sb.Append("strSql.Append(\" SELECT ROW_NUMBER() OVER (\");"); ModelGenerateHelper.NewLine(sb);
            sb.Append("if (!string.IsNullOrEmpty(orderby.Trim()))"); ModelGenerateHelper.NewLine(sb);
            sb.Append("    strSql.Append(\"order by T.\" + orderby);"); ModelGenerateHelper.NewLine(sb);
            sb.Append("else"); ModelGenerateHelper.NewLine(sb);
            sb.Append("    strSql.Append(\"order by T.id desc\");"); ModelGenerateHelper.NewLine(sb);
            sb.Append("strSql.Append(\")AS Row, T.*  from @$TABLE_NAME T \");"); ModelGenerateHelper.NewLine(sb);
            sb.Append("if (!string.IsNullOrEmpty(strWhere.Trim()))"); ModelGenerateHelper.NewLine(sb);
            sb.Append("    strSql.Append(\" WHERE \" + strWhere);"); ModelGenerateHelper.NewLine(sb);
            sb.Append("strSql.Append(\" ) TT\");"); ModelGenerateHelper.NewLine(sb);
            sb.Append("strSql.AppendFormat(\" WHERE TT.Row between {0} and {1}\", page_statrt+1, page_statrt+pz);"); ModelGenerateHelper.NewLine(sb);
            //sb.Append("    string countSql = strSql.ToString().Replace(\"SELECT *\", \"SELECT count(*)\");"); ModelGenerateHelper.NewLine(sb);
            sb.Append("    count = 0;"); ModelGenerateHelper.NewLine(sb);
            //sb.Append("    System.Web.HttpContext.Current.Response.Write(strSql.ToString());"); ModelGenerateHelper.NewLine(sb);
            sb.Append("string countSql = strSql.ToString().Replace(\"SELECT \" + fieldList + \"\", \"select count(*)\");"); ModelGenerateHelper.NewLine(sb);
            //sb.Append(""); ModelGenerateHelper.NewLine(sb);
            sb.Append("int i = countSql.LastIndexOf(\"WHERE\");"); ModelGenerateHelper.NewLine(sb);
            sb.Append("countSql=countSql.Substring(0,i);"); ModelGenerateHelper.NewLine(sb);
            sb.Append("    count = Convert.ToInt32(DbHelperSQL.GetSingle(countSql));"); ModelGenerateHelper.NewLine(sb);
            sb.Append("    return DbHelperSQL.Query(strSql.ToString()).Tables[0];"); ModelGenerateHelper.NewLine(sb);
            sb.Append("}"); ModelGenerateHelper.NewLine(sb);

            return sb.ToString();
        }


        private string GetCodeForGetListInPageOld()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("public DataSet GetList(int PageSize,int PageIndex,string fieldList,string strWhere,string Order,int SortType)"); ModelGenerateHelper.NewLine(sb);
            sb.Append("{"); ModelGenerateHelper.NewLine(sb);
            sb.Append("    //strWhere=string.IsNullOrEmpty(strWhere)?\"@$TABLE_KEY<>NULL\":strWhere;"); ModelGenerateHelper.NewLine(sb);
            sb.Append("    SqlParameter[] parameters = {"); ModelGenerateHelper.NewLine(sb);
            sb.Append("		    new SqlParameter(\"@TableName\", SqlDbType.VarChar, 200),"); ModelGenerateHelper.NewLine(sb);
            sb.Append("		    new SqlParameter(\"@FieldList\", SqlDbType.VarChar, 2000),"); ModelGenerateHelper.NewLine(sb);
            sb.Append("		    new SqlParameter(\"@PrimaryKey\", SqlDbType.VarChar, 100),"); ModelGenerateHelper.NewLine(sb);
            sb.Append("		    new SqlParameter(\"@Where\", SqlDbType.VarChar, 2000),"); ModelGenerateHelper.NewLine(sb);
            sb.Append("		    new SqlParameter(\"@Order\", SqlDbType.VarChar, 1000),"); ModelGenerateHelper.NewLine(sb);
            sb.Append("		    new SqlParameter(\"@SortType\", SqlDbType.Int),"); ModelGenerateHelper.NewLine(sb);
            sb.Append("		    new SqlParameter(\"@RecorderCount\", SqlDbType.Int),"); ModelGenerateHelper.NewLine(sb);
            sb.Append("		    new SqlParameter(\"@PageSize\", SqlDbType.Int),"); ModelGenerateHelper.NewLine(sb);
            sb.Append("		    new SqlParameter(\"@PageIndex\", SqlDbType.Int),"); ModelGenerateHelper.NewLine(sb);
            sb.Append("		    new SqlParameter(\"@TotalCount\", SqlDbType.Int),"); ModelGenerateHelper.NewLine(sb);
            sb.Append("		    new SqlParameter(\"@TotalPageCount\", SqlDbType.Int)"); ModelGenerateHelper.NewLine(sb);
            sb.Append("		    };"); ModelGenerateHelper.NewLine(sb);
            sb.Append("    parameters[0].Value = \"@$TABLE_NAME\";"); ModelGenerateHelper.NewLine(sb);
            sb.Append("    parameters[1].Value = fieldList;"); ModelGenerateHelper.NewLine(sb);
            sb.Append("    parameters[2].Value = \"@$TABLE_KEY\";"); ModelGenerateHelper.NewLine(sb);
            sb.Append("    parameters[3].Value = strWhere;"); ModelGenerateHelper.NewLine(sb);
            sb.Append("    parameters[4].Value = Order;"); ModelGenerateHelper.NewLine(sb);
            sb.Append("    parameters[5].Value = SortType;"); ModelGenerateHelper.NewLine(sb);
            sb.Append("    parameters[6].Value = 0;"); ModelGenerateHelper.NewLine(sb);
            sb.Append("    parameters[7].Value = PageSize;"); ModelGenerateHelper.NewLine(sb);
            sb.Append("    parameters[8].Value = PageIndex;"); ModelGenerateHelper.NewLine(sb);
            sb.Append("    parameters[9].Value = 1;"); ModelGenerateHelper.NewLine(sb);
            sb.Append("    parameters[9].Direction = ParameterDirection.Output;"); ModelGenerateHelper.NewLine(sb);
            sb.Append("    parameters[10].Value = 1;"); ModelGenerateHelper.NewLine(sb);
            sb.Append("    parameters[10].Direction = ParameterDirection.Output;"); ModelGenerateHelper.NewLine(sb);
            //sb.Append("    DbHelperSQL.ExecuteSql(XINLG.Labs.Data.SqlServer.sql.procedure.UP_GetRecordByPage);");
            sb.Append("    DataSet ds = DbHelperSQL.RunProcedure(\"P_viewPage\",parameters,\"ds\");"); ModelGenerateHelper.NewLine(sb);
            //sb.Append("    System.Web.HttpContext.Current.Response.Write(\"===\"+parameters[9].Value);");

            sb.Append("    DataTable dtc = new DataTable();");
            sb.Append("    dtc.Columns.Add(\"count\");");
            sb.Append("    dtc.Columns.Add(\"pagecount\");");
            sb.Append("    DataRow dr = dtc.NewRow();");
            sb.Append("    dr[\"count\"] = parameters[9].Value.ToString();");
            sb.Append("    dr[\"pagecount\"] = parameters[10].Value.ToString();");
            sb.Append("    dtc.Rows.Add(dr);");
            sb.Append("    ds.Tables.Add(dtc);");

            sb.Append("    return ds;");
            sb.Append("}"); ModelGenerateHelper.NewLine(sb);


            return sb.ToString();
        }



        #endregion


        private string GetCodeForSysPublicClass(CodeGenerate model)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("public SysDAL()"); ModelGenerateHelper.NewLine(sb);
            //sb.Append("public static void initConn()"); ModelGenerateHelper.NewLine(sb);
            sb.Append("{"); ModelGenerateHelper.NewLine(sb);
            //sb.Append("DbHelperSQL.connectionString = \"" + model.ConnectionString.Replace("\\", "\\\\") + "\";"); ModelGenerateHelper.NewLine(sb);
            //WebConfigurationManager.ConnectionStrings[key].ToString()
            sb.Append("DbHelperSQL.connectionString = System.Configuration.ConfigurationManager.ConnectionStrings[\"" + model.ConnectionName + "\"].ToString();"); ModelGenerateHelper.NewLine(sb);
            sb.Append("}"); ModelGenerateHelper.NewLine(sb);

            return sb.ToString();
        }


        private string GetSys_TableName()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("public DataTable GetSys_TableName()"); ModelGenerateHelper.NewLine(sb);
            sb.Append("{"); ModelGenerateHelper.NewLine(sb);

            sb.Append("string Sql = \"Select Name FROM SysObjects Where XType='U' orDER BY Name\";"); ModelGenerateHelper.NewLine(sb);
            sb.Append("DataSet ds = DbHelperSQL.Query(Sql);"); ModelGenerateHelper.NewLine(sb);
            sb.Append("if (ds.Tables.Count > 0)"); ModelGenerateHelper.NewLine(sb);
            sb.Append("    return ds.Tables[0];"); ModelGenerateHelper.NewLine(sb);
            sb.Append("else"); ModelGenerateHelper.NewLine(sb);
            sb.Append("    return null;"); ModelGenerateHelper.NewLine(sb);

            sb.Append("}"); ModelGenerateHelper.NewLine(sb);

            return sb.ToString();
        }


        /// <summary>
        /// 获取一张数据表的所有操作方法
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public string Get(DataTable dt, CodeGenerate model)
        {
            if (dt.Rows.Count < 1)
                return "err:无数据";
            else
            {
                string TableName = dt.Rows[0]["TABLE_NAME"].ToString();
                string KeyName = SqlServerSysObjectHelper.GetDataTableColumnKeyName(TableName);
                string KeyType = SqlServerSysObjectHelper.GetDataTableColumnKeyType(TableName);
                if (string.IsNullOrEmpty(KeyName)) KeyType = "";

                #region 所有数据操作方法
                StringBuilder sb = new StringBuilder();
                sb.Append(GetCodeForPublicClass(model));
                ModelGenerateHelper.NewLine(sb);
                sb.Append(GetCodeForExists(dt, KeyName));
                ModelGenerateHelper.NewLine(sb);
                sb.Append(GetCodeForAdd(dt, KeyName));
                ModelGenerateHelper.NewLine(sb);
                sb.Append(GetCodeForGetList(dt));
                ModelGenerateHelper.NewLine(sb);
                sb.Append(GetCodeForGetList2(dt));
                ModelGenerateHelper.NewLine(sb);
                sb.Append(GetCodeForGetListQ(dt));
                ModelGenerateHelper.NewLine(sb);
                sb.Append(GetCodeForExeSQL());
                ModelGenerateHelper.NewLine(sb);
                sb.Append(GetCodeForGetListInPage());
                ModelGenerateHelper.NewLine(sb);
                sb.Append(GetCodeForGetListInPageOld());
                ModelGenerateHelper.NewLine(sb);
                sb.Append(GetCodeForGetListInSql());
                sb.Append(GetCodeForGetModelInSql(dt));
                ModelGenerateHelper.NewLine(sb);
                if (!string.IsNullOrEmpty(KeyName))
                {
                    sb.Append(GetCodeForUpdate(dt, KeyName));
                    ModelGenerateHelper.NewLine(sb);
                    sb.Append(GetCodeForDelete());
                    ModelGenerateHelper.NewLine(sb);
                    sb.Append(GetCodeForDeleteList());
                    ModelGenerateHelper.NewLine(sb);
                    sb.Append(GetCodeForGetModel(dt));
                    ModelGenerateHelper.NewLine(sb);
                }
                #endregion

                string Code = sb.ToString();

                #region 设置表中常用的参数值
                Code = Code.Replace("@$TABLE_NAME", TableName);
                Code = Code.Replace("@$TABLE__NAME", TableName.Replace(".", "_"));

                Code = Code.Replace("@$TABLE_KEY_TYPE", ModelGenerateHelper.FormatDataSqlTypeToSqlDbType(KeyType).ToString());
                if (!string.IsNullOrEmpty(KeyName))
                    Code = Code.Replace("@$TABLE_KEY_type", ModelGenerateHelper.FormatDataType(ModelGenerateHelper.FormatDataSqlTypeToSqlDbType(KeyType)));
                else
                    Code = Code.Replace("@$TABLE_KEY_type", "");

                Code = Code.Replace("@$TABLE_KEY", KeyName);
                Code = Code.Replace("@$KEY_MAX_LENGTH", GetFieldLengthStr(dt, KeyName));
                Code = Code.Replace("@$TABLE_COLUMN_@NAME_NOT_KEY", ModelGenerateHelper.GetTableColumnName(dt, "@", KeyName));
                Code = Code.Replace("@$TABLE_COLUMN_NAME_NOT_KEY", ModelGenerateHelper.GetTableColumnName(dt, "", KeyName));
                #endregion

                return Code;
            }
        }

        /// <summary>
        /// 获取系统数据库操作方法
        /// </summary>
        /// <returns></returns>
        public string GetSys(CodeGenerate model)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(GetCodeForSysPublicClass(model));
            ModelGenerateHelper.NewLine(sb);
            sb.Append(GetSys_TableName());
            ModelGenerateHelper.NewLine(sb);


            return sb.ToString();
        }
    }
}
