using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fosc.Dolphin.IBll.Service.SysService;
using Spring.Stereotype;
using Spring.Transaction.Interceptor;
using Fosc.Dolphin.Bll.IDal;

namespace Fosc.Dolphin.Bll.SysService
{
    [Service()]
    [Transaction(ReadOnly = false, RollbackFor = new Type[] { typeof(Develop.Core.BaseException.BusinessException) })]
    public class SysLoginService : ISysLoginService
    {
        /// <summary>
        /// SysLogin表数据访问对像
        /// </summary>
        private ISysLoginDao sysLoginDao;

        /// <summary>
        /// 构造函数
        /// </summary>
        public SysLoginService()
        {
        }

        public Dto.SysDto.SysLoginDto QuerySysLoginByID(string id)
        {
            if (sysLoginDao == null)
            {
                //从容器取数据访问对象
                sysLoginDao = Develop.Core.Context.ObjectContainer.GetObject<ISysLoginDao>();
            }
            return sysLoginDao.QuerySysLoginByID(id);
        }
    }
}
