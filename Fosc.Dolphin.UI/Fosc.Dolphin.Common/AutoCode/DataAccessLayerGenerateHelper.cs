using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fosc.Dolphin.Common.AutoCode
{
    public static class DataAccessLayerGenerateHelper
    {

        /// <summary>
        ///  获取一张表的数据操作方法
        /// </summary>
        /// <param name="DataTableName"></param>
        /// <param name="IsAddNamespace"></param>
        /// <returns></returns>
        public static string GetDataTableDataAccess(string DataTableName)
        {
            return GetCodeForCreateDAL(DataTableName);
        }

        /// <summary>
        /// DAL的程序集
        /// </summary>
        /// <param name="AssemblyPath"></param>
        /// <returns></returns>
        public static string GetCodeForAssemblyPath(string AssemblyPath)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("private static readonly string AssemblyPath = \"" + AssemblyPath + "\";"); ModelLayerGenerateHelper.NewLine(sb);
            return sb.ToString();
        }

        public static string GetCodeForCreateObject()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" static object CreateObject(string AssemblyPath,string ClassNamespace)"); ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("{"); ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("	object objType = GetCache(ClassNamespace);"); ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("	if (objType == null)"); ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("	{"); ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("		try"); ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("		{"); ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("			objType = Assembly.Load(AssemblyPath).CreateInstance(ClassNamespace);"); ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("			SetCache(ClassNamespace, objType);"); ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("		}"); ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("catch{}"); ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("	}"); ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("	return objType;"); ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("}"); ModelLayerGenerateHelper.NewLine(sb);

            return sb.ToString();
        }



        public static string GetCodeForCreateDAL(string DataTableName)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("public static I" + DataTableName.Replace(".", "_") + " Create" + DataTableName.Replace(".", "_") + "()"); ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("{"); ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("string ClassNamespace = AssemblyPath + \"." + DataTableName.Replace(".", "_") + "DAL\";"); ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("object objType = CreateObject(AssemblyPath, ClassNamespace);"); ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("return (I" + DataTableName.Replace(".", "_") + ")objType; "); ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("}"); ModelLayerGenerateHelper.NewLine(sb);
            return sb.ToString();
        }

        public static string GetCodeForGetCache()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("public static object GetCache(string CacheKey) "); ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("{ "); ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("System.Web.Caching.Cache objCache = HttpRuntime.Cache; "); ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("return objCache[CacheKey]; "); ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("} "); ModelLayerGenerateHelper.NewLine(sb);

            return sb.ToString();

        }

        public static string GetCodeForSetCache()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("public static void SetCache(string CacheKey, object objObject) "); ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("{ "); ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("System.Web.Caching.Cache objCache = HttpRuntime.Cache; "); ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("objCache.Insert(CacheKey, objObject); "); ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("} "); ModelLayerGenerateHelper.NewLine(sb);
            return sb.ToString();

        }

        public static string GetCodeForSetCache2()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("public static void SetCache(string CacheKey, object objObject, DateTime absoluteExpiration,TimeSpan slidingExpiration )"); ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("{"); ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("System.Web.Caching.Cache objCache = HttpRuntime.Cache;"); ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("objCache.Insert(CacheKey, objObject,null,absoluteExpiration,slidingExpiration);"); ModelLayerGenerateHelper.NewLine(sb);
            sb.Append("}"); ModelLayerGenerateHelper.NewLine(sb);

            return sb.ToString();
        }
    }
}
