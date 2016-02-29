using System;
using System.Data;
using System.IO;
using System.Text;
using Fosc.Dolphin.Object.AutoCode;

namespace Fosc.Dolphin.Common.AutoCode
{
    public static class CompileModelImpl
    {
        /// <summary>
        /// 生成抽象工厂
        /// </summary>
        /// <returns></returns>
        public static bool CompileDATA_DataAccess(string assemblyName)
        {

            var codeGenerateModel = new CodeGenerate
            {
                UserNamespace = assemblyName + ".DataAccess",
                ClassName = "DataAccess",
                SysNamespace = new[]
                {
                    "System.Reflection",
                    "System",
                    "System.Configuration",
                    "System.Web",
                    assemblyName+".IDAL"
                }
            };
            var outputPath = AppDomain.CurrentDomain.BaseDirectory + @"CodeOutPath\";
            var Code = GetAllDataTableDataAccess(codeGenerateModel, assemblyName);
            Compile.Code += Code;
            if (Code.IndexOf("err", StringComparison.Ordinal) >= 0)
                return false;
            else
            {
                string OutPath = outputPath + assemblyName + ".DataAccess.dll";
                string[] assemblies = new string[] { 
                "System.dll",
                "System.Configuration.dll",
                "System.Web.dll",
                outputPath+assemblyName+".IDAL.dll"
                };
                return Compile.DomCompile(Code, OutPath, assemblies);
            }
        }

        /// <summary>
        /// 生成所有数据表数据层接口
        /// </summary>
        /// <returns></returns>
        public static string GetAllDataTableDataAccess(CodeGenerate model, string assemblyName)
        {
            StringBuilder sb = new StringBuilder();
            DataTable dt = SqlServerSysObjectHelper.GetDataTableName();
            sb.Append(DataAccess.GetCodeForGetCache());
            ModelGenerateHelper.NewLine(sb);
            sb.Append(DataAccess.GetCodeForSetCache());
            ModelGenerateHelper.NewLine(sb);
            sb.Append(DataAccess.GetCodeForSetCache2());
            ModelGenerateHelper.NewLine(sb);
            sb.Append(DataAccess.GetCodeForAssemblyPath(assemblyName + ".DAL_SqlServer"));
            ModelGenerateHelper.NewLine(sb);
            sb.Append(DataAccess.GetCodeForCreateObject());

            foreach (DataRow dr in dt.Rows)
            {
                sb.Append(DataAccess.GetDataTableDataAccess(dr[0].ToString()));
                ModelGenerateHelper.NewLine(sb);
                ModelGenerateHelper.NewLine(sb);
            }
            string Code = ModelGenerateHelper.GetUserSealedCode(model, sb.ToString());
            ///加入命名空间并生成代码
            return ModelGenerateHelper.GetUserNamespaceCode(model, Code);
        }

        /// <summary>
        /// 生成数据接口层
        /// </summary>
        /// <returns></returns>
        public static bool CompileDataAccessLayerInterface(string assemblyName)
        {
            var codeGenerateModel = new CodeGenerate
            {
                UserNamespace = assemblyName + ".IDAL",
                SysNamespace = new[]
                {
                    "System.Data",
                    "System",
                    assemblyName + ".Model",
                    "System.Collections.Generic"
                }
            };
            var outputPath = AppDomain.CurrentDomain.BaseDirectory + @"CodeOutPath\";
            if (!Directory.Exists(outputPath)) Directory.CreateDirectory(outputPath);
            var outPath = outputPath + assemblyName + ".IDAL.dll";
            var code = GetAllDataTableIdal(codeGenerateModel);
            Compile.Code += code;
            if (code.IndexOf("err", StringComparison.Ordinal) >= 0)
            {
                return false;
            }
            else
            {
                string[] assemblies = new string[]
                {
                    "System.dll",
                    "System.Data.dll",
                    "System.Xml.dll",
                    outputPath + assemblyName + ".Model.dll"
                };
                return Compile.DomCompile(code, outPath, assemblies);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static bool CompileDATA_DAL_SqlServer(string assemblyName)
        {
            var codeGenerateModel = new CodeGenerate
            {
                UserNamespace = assemblyName + ".DAL_SqlServer",
                SysNamespace = new[]
                {
                    "System",
                    "System.Text",
                    "System.Data",
                    "System.Data.SqlClient",
                    "Maticsoft.DBUtility",
                    "System.Web",
                    "System.Configuration",
                    "System.Xml",
                    "System.Collections.Generic",
                    assemblyName+".Model",
                    assemblyName+".IDAL"
                }
            };

            var code = GetAllDataTableDal(codeGenerateModel);
            Compile.Code += code;
            if (code.IndexOf("err", StringComparison.Ordinal) >= 0)
                return false;
            else
            {
                var outputPath = AppDomain.CurrentDomain.BaseDirectory + @"CodeOutPath\";
                var outPath = outputPath + assemblyName + ".DAL_SqlServer.dll";
                string[] assemblies = new string[] { 
                "System.dll",
                "System.Data.dll",
                "System.Xml.dll",
                "System.Web.dll",
                "System.Configuration.dll",
                "Maticsoft.DBUtility.dll",
                "XINLG.Labs.dll",
                outputPath+assemblyName+".IDAL.dll",
                outputPath+assemblyName+".Model.dll",
                "XINLG.Labs.Data.dll"
                };
                return Compile.DomCompile(code, outPath, assemblies);
            }
        }

        /// <summary>
        /// 生成所有数据表DAL类并包装
        /// </summary>
        /// <returns></returns>
        public static string GetAllDataTableDal(CodeGenerate model)
        {
            var sb = new StringBuilder();
            var dt = SqlServerSysObjectHelper.GetDataTableName();
            foreach (DataRow dr in dt.Rows)
            {
                model.ClassName = dr[0].ToString().Replace(".", "_") + "DAL";
                sb.Append((new SqlServer()).GetDataTableDAL(dr[0].ToString(), false, model));
                ModelGenerateHelper.NewLine(sb);
                ModelGenerateHelper.NewLine(sb);
            }
            sb.Append(GetSysDataDal(model));
            //加入命名空间并生成代码
            return ModelGenerateHelper.GetUserNamespaceCode(model, sb.ToString());
        }

        /// <summary>
        /// 生成BLL层
        /// </summary>
        /// <returns></returns>
        public static bool CompileDATA_BLL(string assemblyName)
        {
            var codeGenerateModel = new CodeGenerate
            {
                UserNamespace = assemblyName + ".BLL",
                SysNamespace = new[]
                {
                    "System.Data",
                    "System",
                    "Maticsoft.DBUtility",
                    "System.Collections.Generic",
                    "System.Configuration",
                    assemblyName+".IDAL",
                    assemblyName+".DataAccess",
                    assemblyName+".Model"
                }
            };
            var outputPath = AppDomain.CurrentDomain.BaseDirectory + @"CodeOutPath\";

            string code = GetAllDataTableBll(codeGenerateModel);
            Compile.Code += code;
            if (code.IndexOf("err", StringComparison.Ordinal) >= 0)
                return false;
            else
            {
                string outPath = outputPath + assemblyName + ".BLL.dll";
                string[] assemblies = new string[] { 
                "System.dll",
                "System.Data.dll",
                "System.Xml.dll",
                "System.Configuration.dll",
                "XINLG.Labs.dll",
                "Maticsoft.DBUtility.dll",
                outputPath+assemblyName+".Model.dll",
                outputPath+assemblyName+".DataAccess.dll",
                outputPath+assemblyName+".IDAL.dll"
                };
                return Compile.DomCompile(code, outPath, assemblies);
            }
        }
        /// <summary>
        /// 生成所有数据表DAL类并包装
        /// </summary>
        /// <returns></returns>
        public static string GetAllDataTableBll(CodeGenerate model)
        {
            var sb = new StringBuilder();
            var dt = SqlServerSysObjectHelper.GetDataTableName();
            foreach (DataRow dr in dt.Rows)
            {
                model.ClassName = dr[0].ToString().Replace(".", "_") + "BLL";
                sb.Append(GetDataTableBll(dr[0].ToString(), false, model));
                ModelGenerateHelper.NewLine(sb);
                ModelGenerateHelper.NewLine(sb);
            }
            //加入命名空间并生成代码
            return ModelGenerateHelper.GetUserNamespaceCode(model, sb.ToString());
        }


        /// <summary>
        /// 获取所有系统数据表操作类
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static string GetSysDataDal(CodeGenerate model)
        {
            model.ClassName = "SysDAL";
            var code = ModelGenerateHelper.GetUserClassCode(model, new SqlServer().GetSys(model));
            if (code.IndexOf("err", StringComparison.Ordinal) >= 0) return null;
            //加入命名空间并生成代码
            return code;
        }


        /// <summary>
        ///  获取一张表的数据操作方法
        /// </summary>
        /// <param name="dataTableName"></param>
        /// <param name="isAddNamespace"></param>
        /// <returns></returns>
        private static string GetDataTableBll(string dataTableName, bool isAddNamespace, CodeGenerate model)
        {
            DataTable dt = SqlServerSysObjectHelper.GetDataTableColumn(dataTableName);
            if (dt == null) return null;
            //包装一个类
            string code = ModelGenerateHelper.GetUserPartialCode(model, (new Bll()).Get(dt, model));
            if (code.IndexOf("err", StringComparison.Ordinal) >= 0) return null;

            //包装命名空间
            if (isAddNamespace)
                code = ModelGenerateHelper.GetUserNamespaceCode(model, code);
            return code;
        }


        #region 数据表 操作类
        /// <summary>
        /// 获取一张表的实体类
        /// </summary>
        /// <param name="DataTableName"></param>
        /// <returns></returns>
        private static string GetDataTableIDAL(string DataTableName, bool IsAddNamespace, CodeGenerate model)
        {
            var dt = SqlServerSysObjectHelper.GetDataTableColumn(DataTableName);
            if (dt == null) return null;

            //包装一个接口类
            var code = ModelGenerateHelper.GetUserInterfaceCode(model, GetIdal(dt));
            if (code.IndexOf("err", StringComparison.Ordinal) >= 0) return null;
            //包装命名空间
            if (IsAddNamespace)
                code = ModelGenerateHelper.GetUserNamespaceCode(model, code);
            return code;
        }


        #endregion

        public static string GetIdal(DataTable dt)
        {
            var sb = new StringBuilder();
            if (dt.Rows.Count < 1)
                return "err:无数据";
            else
            {
                string tableName = dt.Rows[0]["TABLE_NAME"].ToString();
                string keyName = SqlServerSysObjectHelper.GetDataTableColumnKeyName(tableName);
                string keyType = SqlServerSysObjectHelper.GetDataTableColumnKeyType(tableName);
                string cshipType = ModelGenerateHelper.FormatDataType(ModelGenerateHelper.FormatDataSqlTypeToSqlDbType(keyType));
                if (string.IsNullOrEmpty(keyName)) keyType = "";

                if (string.IsNullOrEmpty(keyName))
                    sb.Append("bool Exists();");
                else
                {
                    sb.Append("bool Exists(" + cshipType + " " + keyName + ");");
                    sb.Append("bool Delete(" + cshipType + " " + keyName + ");"); ModelGenerateHelper.NewLine(sb);
                    sb.Append("bool DeleteList(string " + keyName + "List);"); ModelGenerateHelper.NewLine(sb);
                    sb.Append("" + tableName.Replace(".", "_") + "Model GetModel(" + cshipType + " " + keyName + ");"); ModelGenerateHelper.NewLine(sb);
                    sb.Append("bool Update(" + tableName.Replace(".", "_") + "Model model);"); ModelGenerateHelper.NewLine(sb);
                }
                ModelGenerateHelper.NewLine(sb);

                sb.Append("" + tableName.Replace(".", "_") + "Model GetModel(string where);"); ModelGenerateHelper.NewLine(sb);
                sb.Append("bool ExeSQL(string sql);"); ModelGenerateHelper.NewLine(sb);
                sb.Append("int Add(" + tableName.Replace(".", "_") + "Model model);"); ModelGenerateHelper.NewLine(sb);
                sb.Append("List<" + tableName.Replace(".", "_") + "Model> GetList(string where);"); ModelGenerateHelper.NewLine(sb);
                sb.Append("List<" + tableName.Replace(".", "_") + "Model> GetModelList(int top,string fields,string where,string order);"); ModelGenerateHelper.NewLine(sb);
                sb.Append("DataTable GetListByPage(string strWhere, string fieldList, string orderby, int pz, int pi, out int count);"); ModelGenerateHelper.NewLine(sb);
                sb.Append("DataSet GetList(int PageSize,int PageIndex,string fieldList,string strWhere,string Order,int SortType);"); ModelGenerateHelper.NewLine(sb);
                sb.Append("DataTable GetListForSQL(string Sql);"); ModelGenerateHelper.NewLine(sb);
            }
            return sb.ToString();
        }


        /// <summary>
        /// 生成所有数据表实体类并包装
        /// </summary>
        /// <returns></returns>
        public static string GetAllDataTableIdal(CodeGenerate model)
        {
            var sb = new StringBuilder();
            DataTable dt = SqlServerSysObjectHelper.GetDataTableName();
            foreach (DataRow dr in dt.Rows)
            {
                model.ClassName = "I" + dr[0].ToString().Replace(".", "_");
                sb.Append(GetDataTableIDAL(dr[0].ToString(), false, model));
                ModelGenerateHelper.NewLine(sb);
            }

            ///加入命名空间并生成代码
            return ModelGenerateHelper.GetUserNamespaceCode(model, sb.ToString());
        }

        /// <summary>
        /// 生成Model层
        /// </summary>
        /// <returns></returns>
        public static bool CompileModel(string assemblyName)
        {
            var codeGenerateModel = new CodeGenerate
            {
                UserNamespace = assemblyName + ".Model",
                SysNamespace = new[]
                {
                    "System",
                    "System.Text"
                }
            };
            var code = ModelGenerateHelper.GetAllDataTableModel(codeGenerateModel);
            Compile.Code += code;
            if (code.IndexOf("err", StringComparison.Ordinal) >= 0)
            {
                return false;
            }
            var outputPath = AppDomain.CurrentDomain.BaseDirectory + @"CodeOutPath\";
            if (!Directory.Exists(outputPath)) Directory.CreateDirectory(outputPath);
            var outPath = outputPath + assemblyName + ".Model.dll";
            var assemblies = new[] { "System.dll" };
            return Compile.DomCompile(code, outPath, assemblies);
        }
    }
}