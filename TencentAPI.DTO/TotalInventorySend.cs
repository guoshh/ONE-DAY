using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TencentAPI.DTO
{
   public class TotalInventorySend
    {
        public List<serverDatasItem> serverDatas { get; set; }
    }
    public class serverDatasItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string matiCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int quantity { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string assetCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string snCode { get; set; }
        /// <summary>
        /// 天津
        /// </summary>
        public string area { get; set; }
        /// <summary>
        /// 天津一号仓库
        /// </summary>
        public string storageName { get; set; }
    }
}
