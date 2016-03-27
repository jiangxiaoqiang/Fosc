using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Fosc.Dolphin.Common.AutoCode
{
    public class DalLayerGenerateHelper
    {
        public static string GetIdal(DataTable dt)
        {
            var sb = new StringBuilder();
            if (dt.Rows.Count < 1)
            {
                return "err:无数据";
            }
            else
            {
                var tableName = dt.Rows[0]["TABLE_NAME"].ToString();
                var keyName = SqlServerSysObjectHelper.GetDataTableColumnKeyName(tableName);
                var keyType = SqlServerSysObjectHelper.GetDataTableColumnKeyType(tableName);
                var cshipType =
                    ModelLayerGenerateHelper.FormatDataType(ModelLayerGenerateHelper.FormatDataSqlTypeToSqlDbType(keyType));
                if (string.IsNullOrEmpty(keyName)) keyType = "";

                if (string.IsNullOrEmpty(keyName))
                    sb.Append("bool Exists();");
                else
                {
                    sb.AppendLine("bool Exists(" + cshipType + " " + keyName + ");");
                    sb.AppendLine("bool Delete(" + cshipType + " " + keyName + ");");
                    ModelLayerGenerateHelper.NewLine(sb);
                    sb.AppendLine("bool DeleteList(string " + keyName + "List);");
                    ModelLayerGenerateHelper.NewLine(sb);
                    sb.AppendLine("" + tableName.Replace(".", "_") + "Model GetModel(" + cshipType + " " + keyName +
                                  ");");
                    ModelLayerGenerateHelper.NewLine(sb);
                    sb.AppendLine("bool Update(" + tableName.Replace(".", "_") + "Model model);");
                    ModelLayerGenerateHelper.NewLine(sb);
                }
                ModelLayerGenerateHelper.NewLine(sb);
                sb.AppendLine("" + tableName.Replace(".", "_") + "Model GetModel(string where);");
                ModelLayerGenerateHelper.NewLine(sb);
                sb.AppendLine("bool ExeSQL(string sql);");
                ModelLayerGenerateHelper.NewLine(sb);
                sb.AppendLine("int Add(" + tableName.Replace(".", "_") + "Model model);");
                ModelLayerGenerateHelper.NewLine(sb);
                sb.AppendLine("List<" + tableName.Replace(".", "_") + "Model> GetList(string where);");
                ModelLayerGenerateHelper.NewLine(sb);
                sb.AppendLine("List<" + tableName.Replace(".", "_") +
                              "Model> GetModelList(int top,string fields,string where,string order);");
                ModelLayerGenerateHelper.NewLine(sb);
                sb.AppendLine(
                    "DataTable GetListByPage(string strWhere, string fieldList, string orderby, int pz, int pi, out int count);");
                ModelLayerGenerateHelper.NewLine(sb);
                sb.AppendLine(
                    "DataSet GetList(int PageSize,int PageIndex,string fieldList,string strWhere,string Order,int SortType);");
                ModelLayerGenerateHelper.NewLine(sb);
                sb.AppendLine("DataTable GetListForSQL(string Sql);");
                ModelLayerGenerateHelper.NewLine(sb);
            }
            return sb.ToString();
        }
    }
}
