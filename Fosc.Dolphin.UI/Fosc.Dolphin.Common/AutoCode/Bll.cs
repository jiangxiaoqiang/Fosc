using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Fosc.Dolphin.Object.AutoCode;

namespace Fosc.Dolphin.Common.AutoCode
{
    public class Bll
    {
         private string GetCodeForCreateDal()
        {
            return "private readonly I@$TABLE__NAME dal=DataAccess.DataAccess.Create@$TABLE__NAME();";
        }

        private string GetCodeForPublicClass(CodeGenerate model)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("public @$TABLE__NAMEBLL()"); ModelGenerateHelper.NewLine(sb);
            //sb.Append("public static void initConn()"); ModelGenerateHelper.NewLine(sb);
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
            StringBuilder sb = new StringBuilder();
            sb.Append("public bool Update(@$TABLE__NAMEModel model)"); ModelGenerateHelper.NewLine(sb);
            sb.Append("{"); ModelGenerateHelper.NewLine(sb);
            sb.Append("	return dal.Update(model);"); ModelGenerateHelper.NewLine(sb);
            sb.Append("}"); ModelGenerateHelper.NewLine(sb);
            return sb.ToString();
        }

        private string GetCodeForDelete()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("public bool Delete(@$TABLE_KEY_type @$TABLE_KEY)"); ModelGenerateHelper.NewLine(sb);
            sb.Append("{"); ModelGenerateHelper.NewLine(sb);
            sb.Append("	return dal.Delete(@$TABLE_KEY);"); ModelGenerateHelper.NewLine(sb);
            sb.Append("}"); ModelGenerateHelper.NewLine(sb);
            return sb.ToString();
        }

        private string GetCodeForDeleteList()
        {
            StringBuilder sb = new StringBuilder();
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
            sb.Append("	{");ModelGenerateHelper.NewLine(sb);
            sb.Append("		objModel = dal.GetModel(@$TABLE_KEY);"); ModelGenerateHelper.NewLine(sb);
            sb.Append("		if (objModel != null)"); ModelGenerateHelper.NewLine(sb);
            sb.Append("		{"); ModelGenerateHelper.NewLine(sb);
            sb.Append("			int ModelCache = 60;"); ModelGenerateHelper.NewLine(sb);
            sb.Append("			DataAccess.DataAccess.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);"); ModelGenerateHelper.NewLine(sb);
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
            StringBuilder sb = new StringBuilder();
            sb.Append("public DataTable GetListForSQL(string Sql)"); ModelGenerateHelper.NewLine(sb);
            sb.Append("{"); ModelGenerateHelper.NewLine(sb);
            sb.Append("	return dal.GetListForSQL(Sql);"); ModelGenerateHelper.NewLine(sb);
            sb.Append("}"); ModelGenerateHelper.NewLine(sb);
            return sb.ToString();
        }


        private string GetCodeForGetPageList()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("public DataTable GetListByPage(string strWhere, string fieldList, string orderby, int pz, int pi, out int count)"); ModelGenerateHelper.NewLine(sb);
            sb.Append("{"); ModelGenerateHelper.NewLine(sb);
            sb.Append("return dal.GetListByPage(strWhere, fieldList, orderby, pz, pi, out count);"); ModelGenerateHelper.NewLine(sb);
            sb.Append("}"); ModelGenerateHelper.NewLine(sb);
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
                StringBuilder sb = new StringBuilder(); ModelGenerateHelper.NewLine(sb);
                sb.Append(GetCodeForCreateDal()); ModelGenerateHelper.NewLine(sb);
                sb.Append(GetCodeForPublicClass(model)); ModelGenerateHelper.NewLine(sb);
                sb.Append(GetCodeForAdd()); ModelGenerateHelper.NewLine(sb);
                sb.Append(GetCodeForExists()); ModelGenerateHelper.NewLine(sb);
                sb.Append(GetCodeForGetModelList()); ModelGenerateHelper.NewLine(sb);
                sb.Append(GetCodeForGetModelListInInfo()); ModelGenerateHelper.NewLine(sb);
                sb.Append(GetCodeForGetPageList()); ModelGenerateHelper.NewLine(sb);
                sb.Append(GetCodeForGetPageListOld()); ModelGenerateHelper.NewLine(sb);
                sb.Append(GetCodeForGetModelInSql()); ModelGenerateHelper.NewLine(sb);
                ModelGenerateHelper.NewLine(sb);
                sb.Append(GetCodeForExeSQL());
                sb.Append(GetCodeForGetListInSql()); ModelGenerateHelper.NewLine(sb);
                if (!string.IsNullOrEmpty(KeyName))
                {
                    sb.Append(GetCodeForDelete()); ModelGenerateHelper.NewLine(sb);
                    sb.Append(GetCodeForDeleteList()); ModelGenerateHelper.NewLine(sb);
                    sb.Append(GetCodeForGetModelByCache()); ModelGenerateHelper.NewLine(sb);
                    sb.Append(GetCodeForGetModel()); ModelGenerateHelper.NewLine(sb);
                    sb.Append(GetCodeForUpdate()); ModelGenerateHelper.NewLine(sb);
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
                #endregion

                return Code;

            }
        
        
        }


    }
}
