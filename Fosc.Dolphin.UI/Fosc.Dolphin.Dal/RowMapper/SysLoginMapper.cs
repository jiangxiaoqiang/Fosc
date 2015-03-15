using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Develop.Core.RowMapper;
using Fosc.Dolphin.Dto.SysDto;

namespace Fosc.Dolphin.Dal.RowMapper
{
    public class SysLoginRowMapper<T> : BaseRowMapper<T>
        where T : SysLoginDto, new()
    {
        /// <summary>
        /// 映射方法
        /// </summary>
        /// <param name="dto">实体</param>
        /// <param name="dataReader">数据读取器</param>
        public override void FieldMap(T dto, System.Data.IDataReader dataReader)
        {
            dto.Code = dataReader["CODE"].ToString();
            dto.Password = dataReader["PASSWORD"].ToString();
            dto.Cname = dataReader["CNAME"].ToString();
        }
    }
}
