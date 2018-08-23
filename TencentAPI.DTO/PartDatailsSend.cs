using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TencentAPI.DTO
{
    public class AvailableDatasItem
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
        /// 天津
        /// </summary>
        public string area { get; set; }
    }

    public class UnavailableDatasItem
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
        public string status { get; set; }
        /// <summary>
        /// 天津
        /// </summary>
        public string area { get; set; }
    }

    public class OnWayDatasItem
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
        public string deliveryDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string poNumber { get; set; }
        /// <summary>
        /// 天津
        /// </summary>
        public string area { get; set; }
    }

    public class PartDatailsSend
    {
        /// <summary>
        /// 
        /// </summary>
        public List<AvailableDatasItem> availableDatas { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<UnavailableDatasItem> unavailableDatas { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<OnWayDatasItem> onWayDatas { get; set; }
    }
}
