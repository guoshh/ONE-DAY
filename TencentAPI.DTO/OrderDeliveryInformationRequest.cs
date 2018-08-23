using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TencentAPI.DTO
{
    public class OrderDeliveryInformationRequest
    {
        /// <summary>
        /// 
        /// </summary>
        public string poNumber { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<DetailsItem> Details { get; set; }
    }
    public class DetailsItem
    {
        /// <summary>
        ///订单行明细 ID
        /// </summary>
        public string detailId { get; set; }
        /// <summary>
        /// 需求单号
        /// </summary>
        public string prNumber { get; set; }
        /// <summary>
        /// 订货单号
        /// </summary>
        public string boNumber { get; set; }
        /// <summary>
        /// bo 单反馈的日期
        /// </summary>
        public string estDeliveryDate { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string memo { get; set; }
        /// <summary>
        /// SN 码
        /// </summary>
        public string snCode { get; set; }
        /// <summary>
        /// 物料编码
        /// </summary>
        public string matiCode { get; set; }
        /// <summary>
        /// 物料名称
        /// </summary>
        public string matiName { get; set; }
        /// <summary>
        /// 资产编码
        /// </summary>
        public string assetCode { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int quantity { get; set; }
        /// <summary>
        /// 期望配送日期
        /// </summary>
        public string deliveryDate { get; set; }
        /// <summary>
        /// 配送仓库
        /// </summary>
        public string deliveryStorage { get; set; }
        /// <summary>
        /// 配送地址
        /// </summary>
        public string address { get; set; }
        /// <summary>
        /// 机架名称
        /// </summary>
        public string positionName { get; set; }
        /// <summary>
        /// 机位名称
        /// </summary>
        public int subPositionName { get; set; }
        public List<configurationsItem> configurations { get; set; }

    }
    public class configurationsItem
    {
        public string key { get; set; }
        /// <summary>
        /// 配置信息
        /// </summary>
        public string value { get; set; }
    }

}
