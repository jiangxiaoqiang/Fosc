/*------------------------------------------------
<copyright file="EncryptTypeGuessService.cs" company="RRMall">
Copyright (c) RRMall.All Rights Reserved.
</copyright>
CLRVersion:4.0.30319.42000
NameSpace:Fosc.Dolphin.Common.Security 
Author:Administrator
Email:jiangxiaoqiang@renrenmall.com
CreateDate:2016/3/18 15:34:39
Stamp:a7efa7ed-4e90-4a18-949f-c6203641686c
UserDomain:DOLPHIN

---------------------------
Modifier:
ModifyDate:
ModifyDescription:
-----------------------------------------------*/

using Fosc.Dolphin.Common.LogCompenent;

namespace Fosc.Dolphin.Common.Security
{
    using System;

    /// <summary>
    /// Write the class summary. 
    /// </summary>
    public class EncryptTypeGuessService
    {
        public static void Guessing(string originalString,string encryptString)
        {
            if (Md5Service.IsMd5Encrypt(originalString, encryptString))
            {
                LogHelper.Logger.Info("Is Md5 encrypt.....");
            }
        }
    }
}
