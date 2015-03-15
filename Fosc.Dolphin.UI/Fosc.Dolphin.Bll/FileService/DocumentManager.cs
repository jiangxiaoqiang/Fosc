using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fosc.Dolphin.IBll.IFileService;

namespace Fosc.Dolphin.Bll.FileService
{
    public class DocumentManager<T>
    {
        #region Attribute
        private readonly Queue<T> documentQueue = new Queue<T>();
        private static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(DocumentManager<T>));
        #endregion

        #region Function

        #region AddDocument
        public void AddDocument(T doc)
        {
            lock (this)
            {
                documentQueue.Enqueue(doc);
            }
        }
        #endregion

        #region IsDocumentAvaliable
        public bool IsDocumentAvaliable
        {
            get 
            {
                return documentQueue.Count > 0;
            }
        }
        #endregion

        #region GetDocument
        public T GetDocument()
        {
            /*
             * Null is arrangement to reference type,and 0 arrangement to value type
             */
            T doc=default(T);
            lock (this)
            {
                doc = documentQueue.Dequeue();
            }
            return doc;
        }
        #endregion

        public void DisplayAllDocument()
        {
            foreach (T doc in documentQueue)
            {
                Document doc1 = doc as Document;
                string a=doc1.Title;
                logger.Info(a);
            }
        }

        #endregion

        #region Event
        #endregion
    }
}
