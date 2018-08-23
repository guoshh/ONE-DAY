using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TencentAPI.DTO
{
   public class MachineOrderProductionInformationSend
    {
        public string poNumber { get; set; }
        public List<details> details { get; set; }
    }
    public class details
    {
        /// <summary>
        /// 
        /// </summary>
        public int detailId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string assetCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string snCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string estimateDeliveryDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string actArriveDate { get; set; }
        /// <summary>
        /// 延期原因备注
        /// </summary>
        public string vendorMemo { get; set; }
        /// <summary>
        /// 生产
        /// </summary>
        public string status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string deliverNumber { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string vendorOutStorageTime { get; set; }
    }
}
