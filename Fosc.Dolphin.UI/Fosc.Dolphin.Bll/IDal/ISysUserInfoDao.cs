using System;
using System.Collections.Generic;
using Fosc.Dolphin.Dto.SysDto;

namespace Fosc.Dolphin.Bll.IDal
{
	public interface ISysUserInfoDao
	{
		IList<SysUserInfoDto> QuerySysUserInfo();
	}
}

