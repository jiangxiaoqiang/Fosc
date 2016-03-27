using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Fosc.Dolphin.Object.AutoCode;
using Maticsoft.DBUtility;

namespace Fosc.Dolphin.Common.AutoCode
{
    public static class ModelLayerGenerateHelper
    {
        /// <summary>
        /// 获取数据表中所有列名
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="LChar"></param>
        /// <returns></returns>
        public static string GetTableColumnName(DataTable dt, string LChar, string NotFieldName)
        {
            string Str = "";
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["COLUMN_NAME"].ToString() != NotFieldName && !string.IsNullOrEmpty(NotFieldName))
                    Str += LChar + dr["COLUMN_NAME"].ToString() + ",";
            }
            if (string.IsNullOrEmpty(Str)) return Str;
            Str = Str.Substring(0, Str.Length - 1);
            return Str;
        }

        /// <summary>
        /// 获取字段最大字符长度
        /// </summary>
        /// <param name="Length"></param>
        /// <param name="SqlType"></param>
        /// <returns></returns>
        public static string FormatSqlTypeMaxLength(string Length, string SqlType)
        {
            if (!string.IsNullOrEmpty(Length)) return Length;
            if (SqlType == "int") return "4";
            return null;
        }

        /// <summary>
        /// 设置用户的一个接口类
        /// </summary>
        /// <param name="m"></param>
        /// <param name="Code">所有的方法</param>
        /// <returns></returns>
        public static string GetUserSealedCode(CodeGenerate m, string Code)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(ModelLayerGenerateHelper.GetSpace(1) + "public sealed class " + m.ClassName);
            ModelLayerGenerateHelper.NewLine(sb);
            sb.Append(ModelLayerGenerateHelper.SetBrace(Code, 1));
            ModelLayerGenerateHelper.NewLine(sb);
            return sb.ToString();
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

        /// <summary>
        /// 返回指定数据表的主键名称
        /// </summary>
        /// <param name="TableName"></param>
        /// <returns></returns>
        public static string GetDataTableColumnKeyName(string TableName)
        {
            string Sql = "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE WHERE TABLE_NAME='" + TableName + "'";
            DataSet ds = DbHelperSQL.Query(Sql);
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                    return ds.Tables[0].Rows[0][0].ToString();
            }
            return null;
        }

        public static string GetUserPartialCode(CodeGenerate m, string code)
        {
            var sb = new StringBuilder();
            sb.Append(GetSpace(1) + "public partial class " + m.ClassName);
            NewLine(sb);
            sb.Append(SetBrace(code, 1));
            NewLine(sb);
            return sb.ToString();
        }

        public static string GetUserPartialCode(CodeGenerate m, string code, string inherit)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(GetSpace(1) + "public partial class " + m.ClassName + ":" + inherit);
            NewLine(sb);
            sb.Append(SetBrace(code, 1));
            NewLine(sb);
            return sb.ToString();
        }

        /// <summary>
        /// 设置用户的一个接口类
        /// </summary>
        /// <param name="m"></param>
        /// <param name="Code">所有的方法</param>
        /// <returns></returns>
        public static string GetUserInterfaceCode(CodeGenerate m, string Code)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(GetSpace(1) + "public interface " + m.ClassName);
            NewLine(sb);
            sb.Append(SetBrace(Code, 1));
            NewLine(sb);
            return sb.ToString();
        }

        public static string GetAllDataTableModel(CodeGenerate model)
        {
            var codeText = new StringBuilder();
            var databaseTable = SqlServerSysObjectHelper.GetDataTableName();
            foreach (DataRow dr in databaseTable.Rows)
            {
                model.ClassName = dr[0].ToString().Replace(".", "_") + "Model";
                codeText.Append(GetDataTableModel(dr[0].ToString(), false, model));
                NewLine(codeText);
            }

            //加入命名空间并生成代码
            return GetUserNamespaceCode(model, codeText.ToString());
        }

        #region 数据表 操作类

        /// <summary>
        /// 获取一张表的实体类
        /// </summary>
        /// <param name="dataTableName"></param>
        /// <param name="isAddNamespace"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        private static string GetDataTableModel(string dataTableName, bool isAddNamespace, CodeGenerate model)
        {
            DataTable dt = SqlServerSysObjectHelper.GetDataTableColumn(dataTableName);
            if (dt == null) return null;
            CodeEntities model2 = new CodeEntities();
            model2.Name = new string[dt.Rows.Count];
            model2.Type = new string[dt.Rows.Count];
            model2.Get = new string[dt.Rows.Count];
            model2.Set = new string[dt.Rows.Count];
            int i = 0;
            foreach (DataRow dr in dt.Rows)
            {
                model2.Name[i] = dr[0].ToString();
                model2.Type[i] = FormatDataType(FormatDataSqlTypeToSqlDbType(dr[1].ToString()));
                model2.Get[i] = "true";
                model2.Set[i] = "true";
                i++;
            }
            //包装一个类
            var code = GetUserClassCode(model, Get(model2));
            if (code.IndexOf("err", StringComparison.Ordinal) >= 0) return null;
            //包装命名空间
            if (isAddNamespace)
                code = GetUserNamespaceCode(model, code);
            return code;
        }


        #endregion

        /// <summary>
        /// 获取实体所有方法
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Type"></param>
        /// <param name="Get"></param>
        /// <param name="Set"></param>
        /// <returns></returns>
        private static string Get(CodeEntities m)
        {
            StringBuilder sb = new StringBuilder();
            if (m.Name.Length == m.Type.Length && m.Get.Length == m.Set.Length && m.Name.Length == m.Get.Length)
            {
                for (int i = 0; i < m.Name.Length; i++)
                {
                    sb.Append(GetEntitiesPrivateCode(m.Name[i], m.Type[i]));
                    NewLine(sb);
                    sb.Append(GetEntitiesPublicAttributeCode(m.Name[i], m.Type[i], m.Get[i], m.Set[i]));
                    NewLine(sb);
                }
            }
            else
                return "err:指定的参数长度不一致！";
            return sb.ToString();
        }

        private static string GetEntitiesAttributeCodeForGet(string Name)
        {
            StringBuilder sb = new StringBuilder("get{ return this._" + Name + "; }");
            NewLine(sb);
            return sb.ToString();
        }

        private static string GetEntitiesAttributeCodeForSet(string Name)
        {
            StringBuilder sb = new StringBuilder("set{ this._" + Name + " = value; }");
            NewLine(sb);
            return sb.ToString();
        }

        private static string GetEntitiesPublicAttributeCode(string Name, string Type, string Get, string Set)
        {
            StringBuilder sb = new StringBuilder();
            string pCode = "";
            sb.Append(GetSpace(2) + "public " + Type + " " + Name);
            NewLine(sb);
            if (Get == "true") pCode = GetEntitiesAttributeCodeForGet(Name);
            if (Set == "true") pCode += GetEntitiesAttributeCodeForSet(Name);
            sb.Append(SetBrace(pCode, 2));
            NewLine(sb);
            return sb.ToString();
        }

        private static string GetEntitiesPrivateCode(string Name, string Type)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(GetSpace(2) + "private " + Type + " _" + Name + ";");
            NewLine(sb);
            return sb.ToString();
        }

        /// <summary>
        /// 设置用户的一个类
        /// </summary>
        /// <param name="m"></param>
        /// <param name="Code">所有的方法</param>
        /// <returns></returns>
        public static string GetUserClassCode(CodeGenerate m, string Code)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(GetSpace(1) + "public class " + m.ClassName);
            NewLine(sb);
            sb.Append(SetBrace(Code, 1));
            NewLine(sb);
            return sb.ToString();
        }

        /// <summary>
        /// 格式化数据库字段类型转SqlDbType数据类型
        /// </summary>
        /// <param name="Source"></param>
        /// <returns></returns>
        public static SqlDbType FormatDataSqlTypeToSqlDbType(string Source)
        {
            SqlDbType dbType = SqlDbType.Variant;//默认为Object

            switch (Source)
            {
                case "int":
                    dbType = SqlDbType.Int;
                    break;
                case "varchar":
                    dbType = SqlDbType.VarChar;
                    break;
                case "bit":
                    dbType = SqlDbType.Bit;
                    break;
                case "datetime":
                    dbType = SqlDbType.VarChar;
                    break;
                case "decimal":
                    dbType = SqlDbType.Decimal;
                    break;
                case "float":
                    dbType = SqlDbType.Float;
                    break;
                case "image":
                    dbType = SqlDbType.Image;
                    break;
                case "money":
                    dbType = SqlDbType.Money;
                    break;
                case "ntext":
                    dbType = SqlDbType.NText;
                    break;
                case "nvarchar":
                    dbType = SqlDbType.NVarChar;
                    break;
                case "smalldatetime":
                    dbType = SqlDbType.SmallDateTime;
                    break;
                case "smallint":
                    dbType = SqlDbType.SmallInt;
                    break;
                case "text":
                    dbType = SqlDbType.Text;
                    break;
                case "bigint":
                    dbType = SqlDbType.BigInt;
                    break;
                case "binary":
                    dbType = SqlDbType.Binary;
                    break;
                case "char":
                    dbType = SqlDbType.Char;
                    break;
                case "nchar":
                    dbType = SqlDbType.NChar;
                    break;
                case "numeric":
                    dbType = SqlDbType.Decimal;
                    break;
                case "real":
                    dbType = SqlDbType.Real;
                    break;
                case "smallmoney":
                    dbType = SqlDbType.SmallMoney;
                    break;
                case "sql_variant":
                    dbType = SqlDbType.Variant;
                    break;
                case "timestamp":
                    dbType = SqlDbType.Timestamp;
                    break;
                case "tinyint":
                    dbType = SqlDbType.TinyInt;
                    break;
                case "uniqueidentifier":
                    dbType = SqlDbType.VarChar;
                    break;
                case "varbinary":
                    dbType = SqlDbType.VarBinary;
                    break;
                case "xml":
                    dbType = SqlDbType.Xml;
                    break;
            }
            return dbType;
        }

        /// <summary>
        /// 将SqlDbType数据类型转换成c#数据类型 如nvchar > string
        /// </summary>
        /// <param name="sqlType"></param>
        /// <returns></returns>
        public static string FormatDataType(SqlDbType sqlType)
        {
            switch (sqlType)
            {
                case SqlDbType.BigInt:
                    return "int";
                case SqlDbType.Binary:
                    return "byte[]";
                case SqlDbType.Bit:
                    return "bool";
                case SqlDbType.Char:
                    return "string";
                case SqlDbType.DateTime:
                    return "DateTime";
                case SqlDbType.Decimal:
                    return "decimal";
                case SqlDbType.Float:
                    return "decimal";
                case SqlDbType.Image:
                    return "byte[]";
                case SqlDbType.Int:
                    return "int";
                case SqlDbType.Money:
                    return "decimal";
                case SqlDbType.NChar:
                    return "string";
                case SqlDbType.NText:
                    return "string";
                case SqlDbType.NVarChar:
                    return "string";
                case SqlDbType.Real:
                    return "decimal";
                case SqlDbType.SmallDateTime:
                    return "DateTime";
                case SqlDbType.SmallInt:
                    return "int";
                case SqlDbType.SmallMoney:
                    return "decimal";
                case SqlDbType.Text:
                    return "string";
                case SqlDbType.Timestamp:
                    return "DateTime";
                case SqlDbType.TinyInt:
                    return "int";
                case SqlDbType.Udt://自定义的数据类型
                    return typeof(System.Object).Name;
                case SqlDbType.UniqueIdentifier:
                    return "string";
                case SqlDbType.VarBinary:
                    return "byte[]";
                case SqlDbType.VarChar:
                    return "string";
                case SqlDbType.Variant:
                    return "byte[]";
                case SqlDbType.Xml:
                    return typeof(System.Object).Name;

                default:
                    return null;
            }
        }


        /// <summary>
        /// 设置自定义命名空间和系统命名空间
        /// </summary>
        /// <param name="m">所有的类</param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string GetUserNamespaceCode(CodeGenerate m, string code)
        {
            var sb = new StringBuilder();
            sb.Append(GetSystemNamespaceCode(m));
            NewLine(sb);
            sb.Append("namespace " + m.UserNamespace);
            NewLine(sb);
            sb.Append(SetBrace(code, 0));
            NewLine(sb);
            return sb.ToString();
        }

        /// <summary>
        /// 使用大括号包装
        /// </summary>
        /// <param name="Code"></param>
        /// <returns></returns>
        private static string SetBrace(string Code, int SpaceLength)
        {
            StringBuilder s = new StringBuilder();
            string space = GetSpace(SpaceLength);
            s.Append(space + "{");
            s.Append(Environment.NewLine);
            s.Append(space + space + Code);
            s.Append(Environment.NewLine);
            s.Append(space + "}");
            return s.ToString();
        }

        /// <summary>
        /// 获取缩进数 1表示缩进4个字符
        /// </summary>
        /// <param name="Length"></param>
        /// <returns></returns>
        private static string GetSpace(int Length)
        {
            string space = "";
            for (int i = 0; i < Length; i++)
                space += "    ";
            return space;
        }

        /// <summary>
        /// 获取系统命名空间
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        private static string GetSystemNamespaceCode(CodeGenerate m)
        {
            StringBuilder sb = new StringBuilder();
            if (m.SysNamespace.Length > 0)
            {
                foreach (string s in m.SysNamespace)
                {
                    sb.Append("using " + s + ";");
                    NewLine(sb);
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 获取换行符
        /// </summary>
        /// <param name="sb"></param>
        /// <returns></returns>
        public static StringBuilder NewLine(StringBuilder sb)
        {
            sb.Append(Environment.NewLine);
            return sb;
        }
    }
}
