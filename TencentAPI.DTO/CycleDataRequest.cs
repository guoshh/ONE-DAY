using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TencentAPI.DTO
{
    public class CycleDataRequest
    {
        public List<CycleItem> Cycle { get; set; }
    }
    public class CycleItem
    {
        /// <summary>
        /// 
        /// </summary>
        public int seq { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string weekName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string beginDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string endDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int year { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int month { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int weekOfMonth { get; set; }
    }
}
