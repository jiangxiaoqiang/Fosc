using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Fosc.Dolphin.Object.AutoCode;

namespace Fosc.Dolphin.Common.AutoCode
{
    public class BllCodeGenerateHelper
    {
         private string GetCodeForCreateDal()
        {
            return "private readonly I@$TABLE__NAME dal=DataAccess.DataAccess.Create@$TABLE__NAME();";
        }

        private string GetCodeForPublicClass(CodeGenerate model)
        {
            var sb = new StringBuilder();
            sb.AppendLine("public @$TABLE__NAMEBLL()");
            sb.AppendLine("{"); 
            sb.AppendLine("DbHelperSQL.connectionString = System.Configuration.ConfigurationManager.ConnectionStrings[\"" + model.ConnectionName + "\"].ToString();"); 
            sb.AppendLine("}"); 

            return sb.ToString();
        }

        private string GetCodeForExists()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("public bool Exists(@$TABLE_KEY_type @$TABLE_KEY)"); ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("{"); ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("	return dal.Exists(@$TABLE_KEY);"); ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("}"); ModelLayerGenerateHelper.NewLine(sb);
            return sb.ToString();
        }

        private string GetCodeForAdd()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("public int Add(@$TABLE__NAMEModel model)"); ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("{"); ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("	return dal.Add(model);"); ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("}"); ModelLayerGenerateHelper.NewLine(sb);
            return sb.ToString();
        }

        private string GetCodeForUpdate()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("public bool Update(@$TABLE__NAMEModel model)"); ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("{"); ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("	return dal.Update(model);"); ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("}"); ModelLayerGenerateHelper.NewLine(sb);
            return sb.ToString();
        }

        private string GetCodeForDelete()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("public bool Delete(@$TABLE_KEY_type @$TABLE_KEY)"); ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("{"); ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("	return dal.Delete(@$TABLE_KEY);"); ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("}"); ModelLayerGenerateHelper.NewLine(sb);
            return sb.ToString();
        }

        private string GetCodeForDeleteList()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("public bool DeleteList(string @$TABLE_KEYList)"); ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("{"); ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("	return dal.DeleteList(@$TABLE_KEYList);"); ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("}"); ModelLayerGenerateHelper.NewLine(sb);
            return sb.ToString();
        }

        private string GetCodeForExeSQL()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("public bool ExeSQL(string sql)"); ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("{"); ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("	return dal.ExeSQL(sql);"); ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("}"); ModelLayerGenerateHelper.NewLine(sb);
            return sb.ToString();
        }

        private string GetCodeForGetModel()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("public @$TABLE__NAMEModel GetModel(@$TABLE_KEY_type @$TABLE_KEY)"); ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("{"); ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("	return dal.GetModel(@$TABLE_KEY);"); ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("}"); ModelLayerGenerateHelper.NewLine(sb);
            return sb.ToString();
        }

        private string GetCodeForGetModelInSql()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("public @$TABLE__NAMEModel GetModel(string where)"); ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("{"); ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("	return dal.GetModel(where);"); ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("}"); ModelLayerGenerateHelper.NewLine(sb);
            return sb.ToString();
        }



        private string GetCodeForGetModelByCache()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("public @$TABLE__NAMEModel GetModelByCache(@$TABLE_KEY_type @$TABLE_KEY)"); ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("{"); ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("string CacheKey = \"@$TABLE__NAMEModel-\" + @$TABLE_KEY;"); ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("object objModel = DataAccess.DataAccess.GetCache(CacheKey);"); ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("if (objModel == null)"); ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("{"); ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("	try"); ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("	{");ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("		objModel = dal.GetModel(@$TABLE_KEY);"); ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("		if (objModel != null)"); ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("		{"); ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("			int ModelCache = 60;"); ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("			DataAccess.DataAccess.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);"); ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("		}"); ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("	}"); ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("	catch{}"); ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("}"); ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("return (@$TABLE__NAMEModel)objModel;"); ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("}"); ModelLayerGenerateHelper.NewLine(sb);

            return sb.ToString();
        }


        private string GetCodeForGetModelList()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("public List<@$TABLE__NAMEModel> GetModelList(string where)"); ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("{"); ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("	return dal.GetList(where);"); ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("}"); ModelLayerGenerateHelper.NewLine(sb);
            return sb.ToString();
        }


        private string GetCodeForGetModelListInInfo()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("public List<@$TABLE__NAMEModel> GetModelList(int top,string fields,string where,string order)"); ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("{"); ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("	return dal.GetModelList(top,fields,where,order);"); ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("}"); ModelLayerGenerateHelper.NewLine(sb);
            return sb.ToString();
        }


        private string GetCodeForGetListInSql()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("public DataTable GetListForSQL(string Sql)"); ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("{"); ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("	return dal.GetListForSQL(Sql);"); ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("}"); ModelLayerGenerateHelper.NewLine(sb);
            return sb.ToString();
        }


        private string GetCodeForGetPageList()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("public DataTable GetListByPage(string strWhere, string fieldList, string orderby, int pz, int pi, out int count)"); ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("{"); ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("return dal.GetListByPage(strWhere, fieldList, orderby, pz, pi, out count);"); ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("}"); ModelLayerGenerateHelper.NewLine(sb);
            return sb.ToString();
        }

        private string GetCodeForGetPageListOld()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("public DataSet GetList(int PageSize,int PageIndex,string fieldList,string strWhere,string Order,int SortType)"); ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("{"); ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("return dal.GetList(PageSize,PageIndex,fieldList,strWhere,Order,SortType);"); ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("}"); ModelLayerGenerateHelper.NewLine(sb);
            return sb.ToString();
        }


        public string Get(DataTable dt,CodeGenerate model)
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
                StringBuilder sb = new StringBuilder(); ModelLayerGenerateHelper.NewLine(sb);
                sb.Append(GetCodeForCreateDal()); ModelLayerGenerateHelper.NewLine(sb);
                sb.Append(GetCodeForPublicClass(model)); ModelLayerGenerateHelper.NewLine(sb);
                sb.Append(GetCodeForAdd()); ModelLayerGenerateHelper.NewLine(sb);
                sb.Append(GetCodeForExists()); ModelLayerGenerateHelper.NewLine(sb);
                sb.Append(GetCodeForGetModelList()); ModelLayerGenerateHelper.NewLine(sb);
                sb.Append(GetCodeForGetModelListInInfo()); ModelLayerGenerateHelper.NewLine(sb);
                sb.Append(GetCodeForGetPageList()); ModelLayerGenerateHelper.NewLine(sb);
                sb.Append(GetCodeForGetPageListOld()); ModelLayerGenerateHelper.NewLine(sb);
                sb.Append(GetCodeForGetModelInSql()); ModelLayerGenerateHelper.NewLine(sb);
                ModelLayerGenerateHelper.NewLine(sb);
                sb.Append(GetCodeForExeSQL());
                sb.Append(GetCodeForGetListInSql()); ModelLayerGenerateHelper.NewLine(sb);
                if (!string.IsNullOrEmpty(KeyName))
                {
                    sb.Append(GetCodeForDelete()); ModelLayerGenerateHelper.NewLine(sb);
                    sb.Append(GetCodeForDeleteList()); ModelLayerGenerateHelper.NewLine(sb);
                    sb.Append(GetCodeForGetModelByCache()); ModelLayerGenerateHelper.NewLine(sb);
                    sb.Append(GetCodeForGetModel()); ModelLayerGenerateHelper.NewLine(sb);
                    sb.Append(GetCodeForUpdate()); ModelLayerGenerateHelper.NewLine(sb);
                }
                #endregion

                string Code = sb.ToString();

                #region 设置表中常用的参数值
                Code = Code.Replace("@$TABLE_NAME", TableName);
                Code = Code.Replace("@$TABLE__NAME", TableName.Replace(".", "_"));

                Code = Code.Replace("@$TABLE_KEY_TYPE", ModelLayerGenerateHelper.FormatDataSqlTypeToSqlDbType(KeyType).ToString());
                if (!string.IsNullOrEmpty(KeyName))
                    Code = Code.Replace("@$TABLE_KEY_type", ModelLayerGenerateHelper.FormatDataType(ModelLayerGenerateHelper.FormatDataSqlTypeToSqlDbType(KeyType)));
                else
                    Code = Code.Replace("@$TABLE_KEY_type", "");

                Code = Code.Replace("@$TABLE_KEY", KeyName);
                #endregion

                return Code;

            }
        
        
        }


    }
}
