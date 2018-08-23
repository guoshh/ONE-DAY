using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TencentAPI.DTO
{
    public class PartReceiveMsgSend
    {
        public string recNumber { get; set; }
        public string memo { get; set; }
        public string area { get; set; }
        public string storage { get; set; }
        public string address { get; set; }
        public string createDate { get; set; }
        public List<DetailsItem4> details { get; set; }


    }
    public class DetailsItem4
    {
        public string detailId { get; set; }
        public string orderNumber { get; set; }
        public string orderDetailID { get; set; }
        public string matiCode { get; set; }
        public string matiName { get; set; }
        public string recQuantity { get; set; }
    }
}
