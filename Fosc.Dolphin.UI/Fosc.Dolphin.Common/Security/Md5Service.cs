/*------------------------------------------------
<copyright file="Md5Service.cs" company="RRMall">
Copyright (c) RRMall.All Rights Reserved.
</copyright>
CLRVersion:4.0.30319.42000
NameSpace:Fosc.Dolphin.Common.Security 
Author:Administrator
Email:jiangxiaoqiang@renrenmall.com
CreateDate:2016/3/18 15:37:41
Stamp:536b7288-ae60-4df2-87a4-ff521477c353
UserDomain:DOLPHIN

---------------------------
Modifier:
ModifyDate:
ModifyDescription:
-----------------------------------------------*/

using System.Security.Cryptography;
using System.Text;
using RR.Labs.Utils;

namespace Fosc.Dolphin.Common.Security
{
    using System;

    /// <summary>
    /// Write the class summary. 
    /// </summary>
    public class Md5Service
    {
        public static bool IsMd5Encrypt(string originalString, string encryptString)
        {
            return SecurityUtil.EncryptByMD5(originalString).Equals(encryptString, StringComparison.Ordinal);
        }
    }
}
