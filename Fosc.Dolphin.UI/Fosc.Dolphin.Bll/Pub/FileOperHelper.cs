using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fosc.Dolphin.IBll.IPub;
using System.IO;

namespace Fosc.Dolphin.Bll.Pub
{
    public class FileOperHelper : IFileOperHelper
    {
        #region Clear Cache File
        public void ClearCacheFile(List<string> Paths)
        {
            foreach (string TempPath in Paths)
            {
                if (!System.IO.Directory.Exists(TempPath))
                {
                    continue;
                }
                foreach (string fileName in Directory.GetFiles(TempPath))
                {
                    File.SetAttributes(fileName, FileAttributes.Normal);
                    if (fileName == TempPath + "DcoreEcaConfig.xml")
                    {
                        continue;
						
                    }
                    else
                    {
                        try
                        {
                            File.Delete(fileName);
                        }
                        catch (Exception)
                        {

                        }
                    }
                }
            }
        }
        #endregion
    }
}
