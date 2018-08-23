using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TencentAPI.DTO
{
    public  class DetailsItem0
    {
        /// <summary>
        /// 
        /// </summary>
        public int detailId { get; set; }
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
        public string deliveryDate { get; set; }
        /// <summary>
        /// 仓库名称
        /// </summary>
        public string storage { get; set; }
        /// <summary>
        /// 详细送货地址
        /// </summary>
        public string address { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public  string price { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string memo { get; set; }
    }

    public class PartPurchaseOrderSend
    {
        /// <summary>
        /// 
        /// </summary>
        public string orderNumber { get; set; }
        /// <summary>
        /// 腾讯
        /// </summary>
        public string vendorName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string createDate { get; set; }
        /// <summary>
        /// 采购公司主体名称
        /// </summary>
        public string companyName { get; set; }
        /// <summary>
        /// 人民币
        /// </summary>
        public string currency { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string orderType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<DetailsItem0> details { get; set; }
    }


}
