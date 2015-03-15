using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fosc.Dolphin.IBll.IFileService
{
    public interface IDocument
    {
        string Title { get; set; }
        string Content { get; set; }
    }
}
