using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fosc.Dolphin.Dto.SysDto;

namespace Fosc.Dolphin.Bll.IDal
{
    public interface ISysLoginDao
    {
        /// <summary>
        /// 通过ID查询SysLogin表数据
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns>实体</returns>
        SysLoginDto QuerySysLoginByID(string id);
    }
}
