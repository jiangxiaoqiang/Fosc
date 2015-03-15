using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Develop.Core.Dto;

namespace Fosc.Dolphin.Dto.SysDto
{
    /// <summary>
    /// SysUserinfo数据实体
    /// </summary>
    [Serializable]
    public class SysUserInfoDto:BaseDto
    {
        ///<summary>
        ///UserName
        ///</summary>
        public string UserName
        {
            get;
            set;
        }

        ///<summary>
        ///UserID
        ///</summary>
        public int UserID
        {
            get;
            set;
        }
    }
}
