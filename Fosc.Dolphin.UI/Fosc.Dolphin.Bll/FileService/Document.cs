using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fosc.Dolphin.IBll.IFileService;

namespace Fosc.Dolphin.Bll.FileService
{
    public class Document : IDocument
    {
        public string Title { get; set; }
        public string Content { get; set; }
                
        public Document(string title, string content)
        {
            this.Title = title;
            this.Content = content;
        }
    }
}
