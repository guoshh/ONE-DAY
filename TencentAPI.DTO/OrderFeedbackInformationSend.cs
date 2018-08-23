using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TencentAPI.DTO
{

    public class DetailsItem3
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
        /// 
        /// </summary>
        public string estDeliveryDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int estQuantity { get; set; }
    }

    public class OrderFeedbackInformationSend
    {
        /// <summary>
        /// 
        /// </summary>
        public string boNumber { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<DetailsItem3> details { get; set; }
    }
}
