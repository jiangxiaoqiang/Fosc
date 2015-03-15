using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fosc.Dolphin.Dto.SysDto;
using Develop.Core.RowMapper;

namespace Fosc.Dolphin.Dal.RowMapper
{
    class SysUserInfoRowMapper<T> : BaseRowMapper<T>
        where T : SysUserInfoDto, new()
    {
        /// <summary>
        /// 映射方法
        /// </summary>
        /// <param name="dto">实体</param>
        /// <param name="dataReader">数据读取器</param>
        public override void FieldMap(T dto, System.Data.IDataReader dataReader)
        {
            dto.UserName = Convert.ToString(dataReader["st_name"]);
            dto.UserID = Convert.ToInt32(dataReader["st_id"]);
        }
    }
}
