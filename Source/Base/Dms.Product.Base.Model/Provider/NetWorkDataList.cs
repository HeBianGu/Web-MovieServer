using System;
using System.Collections.Generic;
using System.Text;

namespace Dms.Product.Base.Model
{
    public class NetWorkDataCollection<T>
    {
        /// <summary>
        /// 
        /// </summary>
        public bool succeed { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<T> data { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string message { get; set; }
    } 

}
