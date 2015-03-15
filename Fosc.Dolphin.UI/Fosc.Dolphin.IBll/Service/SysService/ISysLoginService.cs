using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Fosc.Dolphin.Dto.SysDto;

namespace Fosc.Dolphin.IBll.Service.SysService
{
    [ServiceContract]
    public interface ISysLoginService
    {
        [OperationContract]
        SysLoginDto QuerySysLoginByID(string id);
    }
}
