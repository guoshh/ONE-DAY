using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TencentAPI.DTO
{
    public class ConfigurationsItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string key { get; set; }
        /// <summary>
        /// 配置信息
        /// </summary>
        public string value { get; set; }

    }
    public class DetailsItem1
    {
        /// <summary>
        /// 
        /// </summary>
        public string detailId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string matiCode { get; set; }
        /// <summary>
        /// 传统服务器腾讯 
        /// </summary>
        public string matiName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int quantity { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string deliveryDate { get; set; }
        /// <summary>
        /// xxx 数据中心
        /// </summary>
        public string deliveryStorage { get; set; }
        /// <summary>
        /// xx 市 xx 区 xx 路 101 号
        /// </summary>
        public string address { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string odmBrand { get; set; }
        /// <summary>
        /// 中国
        /// </summary>
        public string country { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public ConfigurationsItem configurations { get; set; }
    }
    public class OrderMsgSend
    {
        public DateTime createTime { get; set; }
        public string boNumber { get; set; }
        public List<DetailsItem1> details { get; set; }
    }
}
