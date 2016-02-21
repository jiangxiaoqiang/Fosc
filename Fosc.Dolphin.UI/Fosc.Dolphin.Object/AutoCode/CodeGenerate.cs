using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fosc.Dolphin.Object.AutoCode
{
    public class CodeGenerate
    {
        private string[] _sysNamespace = new string[] { };
        private string _userNamespace = "XINLG.Untitled_";
        private string _className = "NoNameClass";
        private string _assemblyName = "XINLG.Labs.NewAssembly";
        private string _sqlType = "SqlServer";
        private string _connectionName = "";
        private string _outAssemblyPath = "bin/CodeOutPath/";


        /// <summary>
        /// 系统命名空间
        /// </summary>
        public string[] SysNamespace
        {
            set { _sysNamespace = value; }
            get { return _sysNamespace; }
        }

        /// <summary>
        /// 当前用户命名空间
        /// </summary>
        public string UserNamespace
        {
            set { _userNamespace = value; }
            get { return _userNamespace; }
        }

        /// <summary>
        /// 类名
        /// </summary>
        public string ClassName
        {
            set { _className = value; }
            get { return _className; }
        }

        /// <summary>
        /// 创建新的程序集的名称
        /// </summary>
        public string AssemblyName
        {
            set { _assemblyName = value; }
            get { return _assemblyName; }
        }

        /// <summary>
        /// 设置数据库类型
        /// </summary>
        public string SqlType
        {
            set { _sqlType = value; }
            get { return _sqlType; }
        }

        /// <summary>
        /// 设置数据库连接字串
        /// </summary>
        public string ConnectionName
        {
            set { _connectionName = value; }
            get { return _connectionName; }
        }

        /// <summary>
        /// 程序集输出目录
        /// </summary>
        public string OutAssemblyPath
        {
            set { _outAssemblyPath = value; }
            get
            {
                return _outAssemblyPath;
            }
        }
    }
}
