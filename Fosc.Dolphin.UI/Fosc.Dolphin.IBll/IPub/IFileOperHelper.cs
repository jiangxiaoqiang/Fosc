using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fosc.Dolphin.IBll.IPub
{
    public interface IFileOperHelper
    {
        void ClearCacheFile(List<string> Paths);
    }
}
