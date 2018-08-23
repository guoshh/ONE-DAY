using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TencentAPI.DTO
{
    public class SupplyCapacitySend
    {
      public  List<PartDatasItem1>   partDatas { get; set; }
    }
    public class PartDatasItem1
    {
        /// <summary>
        /// 物料编码
        /// </summary>
        public string matiCode { get; set; }
        /// <summary>
        /// 部件类型 CPU,HDD,SSD,MEM,L6
        /// </summary>
        public string partType { get; set; }
        /// <summary>
        /// 能够提供的部件物料时间（按周）
        /// </summary>
        public string weekName { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int quantity { get; set; }
        /// <summary>
        /// 所在区域
        /// </summary>
        public string area { get; set; }
    }
}
