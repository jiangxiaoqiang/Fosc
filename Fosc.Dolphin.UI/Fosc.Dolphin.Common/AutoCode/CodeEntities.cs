using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fosc.Dolphin.Common.AutoCode
{
    public class CodeEntities
    {
        private string[] _Name;
        private string[] _Type;
        private string[] _Get;
        private string[] _Set;


        /// <summary>
        /// 属性命名
        /// </summary>
        public string[] Name
        {
            set { _Name = value; }
            get { return _Name; }
        }

        /// <summary>
        /// 属性的数据类型
        /// </summary>
        public string[] Type
        {
            set { _Type = value; }
            get { return _Type; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string[] Get
        {
            set { _Get = value; }
            get { return _Get; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string[] Set
        {
            set { _Set = value; }
            get { return _Set; }
        }
    }
}
