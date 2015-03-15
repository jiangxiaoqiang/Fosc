using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Develop.Core.Dto;

namespace Fosc.Dolphin.Dto.SysDto
{
    [Serializable]
    public class SysLoginDto:BaseDto
    {
        ///<summary>
        ///用户登录用的名字，要唯一
        ///</summary>
        public string Code
        {
            get;
            set;
        }        

        ///<summary>
        ///登录密码
        ///</summary>
        public string Password
        {
            get;
            set;
        }

        ///<summary>
        ///User Name
        ///</summary>
        public string Cname
        {
            get;
            set;
        }
    }
}
