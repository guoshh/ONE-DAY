using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TencentAPI.DTO
{

    public class Msg
    {
        public string guid { get; set; }
        public string msgType { get; set; }
        public string msgBody { get; set; }
        public string createTime { get; set; }
        public string description { get; set; }
    }

    public class Msgs
    {
        /// <summary>
        /// 
        /// </summary>
        public List<Msg>  Msg { get; set; }

    }
}
