using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fosc.Dolphin.Dto.SysDto;
using Develop.Core.Dal;
using Fosc.Dolphin.Dal.RowMapper;
using Fosc.Dolphin.Bll.IDal;

namespace Fosc.Dolphin.Dal.Dao
{
    public class SysLoginDao:BaseDao,ISysLoginDao
    {
        /// <summary>
        /// 通过ID查询SysLogin表数据
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns>实体</returns>
        public SysLoginDto QuerySysLoginByID(string id)
        {
            string sql = SqlExcutor.SqlReader.ReadSqlByKey("QuerySysLoginByID");
            IDictionary<string, object> para = new Dictionary<string, object>();
            para.Add("Code", id);
            return SqlExcutor.SqlQueryForObject<SysLoginDto>(sql, new SysLoginRowMapper<SysLoginDto>(), para);
        }
    }
}
