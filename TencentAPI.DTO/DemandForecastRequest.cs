using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TencentAPI.DTO
{
    public class PartDatasItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string weekName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string matiCode { get; set; }
        /// <summary>
        /// 服务器内存 三星 512M/DDRII Pcs
        /// </summary>
        public string matiName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int quantity { get; set; }
        /// <summary>
        /// 内存
        /// </summary>
        public string partType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string createDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string beginDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string endDate { get; set; }
        /// <summary>
        /// 中国
        /// </summary>
        public string area { get; set; }
    }

    public class BackupPartDatasItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string weekName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string matiCode { get; set; }
        /// <summary>
        /// 服务器电源线 联想 国际三扁(2米/10A/IEC320C13/RVV型3*1.0mm2) Pcs
        /// </summary>
        public string matiName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int quantity { get; set; }
        /// <summary>
        /// 电源线
        /// </summary>
        public string partType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string createDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string beginDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string endDate { get; set; }
        /// <summary>
        /// 中国
        /// </summary>
        public string area { get; set; }
    }

    public class ModelDatasItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string weekName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string modelName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int quantity { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string createDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string beginDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string endDate { get; set; }
        /// <summary>
        /// 中国
        /// </summary>
        public string area { get; set; }
    }

    public class DemandForecastRequest
    {
        /// <summary>
        /// 
        /// </summary>
        public List<PartDatasItem> partDatas { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<BackupPartDatasItem> backupPartDatas { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<ModelDatasItem> modelDatas { get; set; }
    }
}
