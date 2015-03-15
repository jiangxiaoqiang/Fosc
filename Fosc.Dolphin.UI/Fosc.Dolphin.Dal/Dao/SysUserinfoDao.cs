using System;
using System.Collections.Generic;
using Fosc.Dolphin.Dto.SysDto;
using Develop.Core.Dal;
using Fosc.Dolphin.Bll.IDal;
using Fosc.Dolphin.Dal.RowMapper;

namespace Fosc.Dolphin.Dal
{
	public class SysUserInfoDao:BaseDao,ISysUserInfoDao
	{
		public SysUserInfoDao ()
		{
			
		}
		
		public IList<SysUserInfoDto> QuerySysUserInfo()
		{
			string sql = SqlExcutor.SqlReader.ReadSqlByKey("QuerySysUserinfo");
            return SqlExcutor.SqlQueryWithRowMapper<SysUserInfoDto>(sql, new SysUserInfoRowMapper<SysUserInfoDto>());
		}		
	}
}

