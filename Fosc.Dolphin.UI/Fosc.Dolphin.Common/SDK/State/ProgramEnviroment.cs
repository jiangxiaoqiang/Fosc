/*------------------------------------------------
<copyright file="ProgramEnviroment.cs" company="RRMall">
Copyright (c) RRMall.All Rights Reserved.
</copyright>
CLRVersion:4.0.30319.42000
NameSpace:Fosc.Dolphin.Common.SDK.State 
Author:Administrator
Email:jiangxiaoqiang@renrenmall.com
CreateDate:2016/3/22 14:43:35
Stamp:23bb88ed-e268-4315-9425-fbf64ea70e92
UserDomain:DOLPHIN

---------------------------
Modifier:
ModifyDate:
ModifyDescription:
-----------------------------------------------*/

using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Fosc.Dolphin.Common.SDK.State
{
    using System;
    using System.Reflection;

    /// <summary>
    /// Write the class summary. 
    /// </summary>
    public class ProgramEnviroment
    {
        #region Is Debug Model
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dllPath">Assembly path</param>
        /// <returns></returns>
        public static bool IsDebugged(string dllPath)
        {
            var assembly =Assembly.LoadFile(Path.GetFullPath(dllPath));
            return assembly.GetCustomAttributes(false).OfType<DebuggableAttribute>().Any(debuggableAttribute => debuggableAttribute.IsJITTrackingEnabled);
        }
        #endregion
    }
}
