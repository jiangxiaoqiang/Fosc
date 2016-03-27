using System.CodeDom.Compiler;
using System.Collections.Generic;
using Fosc.Dolphin.Common.LogCompenent;

namespace Fosc.Dolphin.Common.AutoCode
{
    public static class CoreCompilerHelper
    {
        private static string _code = "";

        public static string Code
        {
            set { _code = value; }
            get { return _code; }
        }

        /// <summary>
        /// 编译源代码
        /// </summary>
        /// <param name="sourceCodeContent"></param>
        /// <param name="outAssemblyPath">DLL输出目录</param>
        /// <param name="referencedAssemblies">编译时需引用的程序集</param>
        /// <returns></returns>
        public static bool DomCompile(string sourceCodeContent, string outAssemblyPath, IEnumerable<string> referencedAssemblies)
        {
            var compileSuccess = true;
            var codeDomProvider = CodeDomProvider.CreateProvider("C#");
            var compilerParameters = new CompilerParameters();
            foreach (var singleReference in referencedAssemblies)
            {
                compilerParameters.ReferencedAssemblies.Add(singleReference);
            }
            compilerParameters.GenerateExecutable = false;
            compilerParameters.GenerateInMemory = false;
            compilerParameters.OutputAssembly = outAssemblyPath;
            var compilerResult = codeDomProvider.CompileAssemblyFromSource(compilerParameters, sourceCodeContent);
            if (compilerResult.Errors.HasErrors)
            {
                var compileErrorInfo = string.Empty;
                foreach (CompilerError compilerError in compilerResult.Errors)
                {
                    compileErrorInfo += compilerError.ErrorText;
                }
                compileSuccess = false;
                LogHelper.Logger.Error("Compile error:" + compileErrorInfo);
            }
            return compileSuccess;
        }
    }
}