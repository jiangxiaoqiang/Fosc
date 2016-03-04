using System;
using System.Data;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using Fosc.Dolphin.Object.AutoCode;

namespace Fosc.Dolphin.Common.AutoCode
{
    public class CompileModelImpl
    {
        #region Attribute
        /// <summary>
        /// 
        /// </summary>
        private static readonly string OutputPath = AppDomain.CurrentDomain.BaseDirectory + @"CodeOutPath\";

        #endregion
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
            var code = GetAllDataTableDataAccess(codeGenerateModel, assemblyName);
            Compile.Code += code;
            if (code.IndexOf("err", StringComparison.Ordinal) >= 0)
            {
                return false;
            }
            else
            {
                var outPath = OutputPath + assemblyName + ".DataAccess.dll";
                var assemblies = new string[]
                {
                    "System.dll",
                    "System.Configuration.dll",
                    "System.Web.dll",
                    OutputPath + assemblyName + ".IDAL.dll"
                };
                return Compile.DomCompile(code, outPath, assemblies);
            }
        }

        /// <summary>
        /// 生成所有数据表数据层接口
        /// </summary>
        /// <returns></returns>
        public static string GetAllDataTableDataAccess(CodeGenerate model, string assemblyName)
        {
            var sb = new StringBuilder();
            var dt = SqlServerSysObjectHelper.GetDataTableName();
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
            string code = ModelGenerateHelper.GetUserSealedCode(model, sb.ToString());
            ///加入命名空间并生成代码
            return ModelGenerateHelper.GetUserNamespaceCode(model, code);
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
            if (!Directory.Exists(OutputPath)) Directory.CreateDirectory(OutputPath);
            var outPath = OutputPath + assemblyName + ".IDAL.dll";
            var code = GetAllDataTableIdal(codeGenerateModel);
            Compile.Code += code;
            if (code.IndexOf("err", StringComparison.Ordinal) >= 0)
            {
                return false;
            }
            else
            {
                var assemblies = new string[]
                {
                    "System.dll",
                    "System.Data.dll",
                    "System.Xml.dll",
                    OutputPath + assemblyName + ".Model.dll"
                };
                return Compile.DomCompile(code, outPath, assemblies);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static bool CompileDalSqlServer(string assemblyName)
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
            {
                return false;
            }
            else
            {
                var outPath = OutputPath + assemblyName + ".DAL_SqlServer.dll";
                var assemblies = new string[]
                {
                    "System.dll",
                    "System.Data.dll",
                    "System.Xml.dll",
                    "System.Web.dll",
                    "System.Configuration.dll",
                    "Maticsoft.DBUtility.dll",
                    "XINLG.Labs.dll",
                    OutputPath + assemblyName + ".IDAL.dll",
                    OutputPath + assemblyName + ".Model.dll",
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
            var code = GetAllDataTableBll(codeGenerateModel);
            Compile.Code += code;
            if (code.IndexOf("err", StringComparison.Ordinal) >= 0)
            {
                return false;
            }
            else
            {
                string outPath = OutputPath + assemblyName + ".BLL.dll";
                string[] assemblies = new string[]
                {
                    "System.dll",
                    "System.Data.dll",
                    "System.Xml.dll",
                    "System.Configuration.dll",
                    "XINLG.Labs.dll",
                    "Maticsoft.DBUtility.dll",
                    OutputPath + assemblyName + ".Model.dll",
                    OutputPath + assemblyName + ".DataAccess.dll",
                    OutputPath + assemblyName + ".IDAL.dll"
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
        /// <param name="model"></param>
        /// <returns></returns>
        private static string GetDataTableBll(string dataTableName, bool isAddNamespace, CodeGenerate model)
        {
            var dt = SqlServerSysObjectHelper.GetDataTableColumn(dataTableName);
            if (dt == null) return null;
            //包装一个类
            var code = ModelGenerateHelper.GetUserPartialCode(model, (new Bll()).Get(dt, model));
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
        /// <param name="dataTableName"></param>
        /// <param name="isAddNamespace"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        private static string GetDataTableIdal(string dataTableName, bool isAddNamespace, CodeGenerate model)
        {
            var dt = SqlServerSysObjectHelper.GetDataTableColumn(dataTableName);
            if (dt == null) return null;

            //包装一个接口类
            var code = ModelGenerateHelper.GetUserInterfaceCode(model, DalLayerHelper.GetIdal(dt));
            if (code.IndexOf("err", StringComparison.Ordinal) >= 0) return null;
            //包装命名空间
            if (isAddNamespace)
                code = ModelGenerateHelper.GetUserNamespaceCode(model, code);
            return code;
        }


        #endregion

        /// <summary>
        /// 生成所有数据表实体类并包装
        /// </summary>
        /// <returns></returns>
        public static string GetAllDataTableIdal(CodeGenerate model)
        {
            var sb = new StringBuilder();
            var dt = SqlServerSysObjectHelper.GetDataTableName();
            foreach (DataRow dr in dt.Rows)
            {
                model.ClassName = "I" + dr[0].ToString().Replace(".", "_");
                sb.Append(GetDataTableIdal(dr[0].ToString(), false, model));
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
            if (!Directory.Exists(OutputPath)) Directory.CreateDirectory(OutputPath);
            var outPath = OutputPath + assemblyName + ".Model.dll";
            var assemblies = new[] { "System.dll" };
            return Compile.DomCompile(code, outPath, assemblies);
        }
    }
}