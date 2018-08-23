using ServiceStack.Redis;
using System;
using System.Data;
using System.Web.Mvc;
using TencentAPI.Utils;

namespace TencentAPI.Controllers
{
    public class ClientController : Controller
    {
        TimerTools Timer = new TimerTools();
        // GET: Client
        public ActionResult Touch()
        {
            return View();
        }
        public ActionResult Tencent_View()
        {
            return View();
        }
        /// <summary>
        /// 发送整机订单生产信息
        /// </summary>
        /// <returns></returns>
        public string MachineOrderProductionInformationSend()
        {
            Timer.MachineOrderProductionInformationSend();
            return "";
        }
        /// <summary>
        /// 发送部件采购单信息
        /// </summary>
        /// <returns></returns>
        public string PartPurchaseOrderSend()
        {
            string msgType = "PART_ORDER_ACCEPT";
            string msgBody = "";
            string guid = "";
            Timer.CaseRequest(guid, msgType, msgBody);
            return "";
        }
        /// <summary>
        /// 获取需求预测
        /// </summary>
        /// <returns></returns>
        public string DemandForecastRequest()
        {
            Timer.TimerDemandForecastRequest();
            return "";
        }
        /// <summary>
        /// 发送供应能力数据
        /// </summary>
        /// 
        /// <returns></returns>
        public JsonResult TimerSupplyCapacitySend()
        {
            string Stage = Request["Stage"].ToString();
            if (Stage == "0")
            {
                HanaHelper Hana = new HanaHelper();
                DataTable dtPart = Hana.GetTable("ZpartDatas", "matiCode,partType,weekName,quantity,area");
                var jsonReturn = new { a = 0, b = dtPart };
                return Json(jsonReturn);
            }
            else
            {
                string JsonString = Timer.TimerSupplyCapacitySend();
                var jsonReturn = new { a = 1, b = JsonString };
                return Json(jsonReturn);
            }
        }
        /// <summary>
        /// 发送订单部件验收信息
        /// </summary>
        /// <returns></returns>
        public string PartReceiveMsgSend()
        {
            Timer.PartReceiveMsgSend();
            return "";
        }
        /// <summary>
        /// 发送订单部件验收信息
        /// </summary>
        /// <returns></returns>
        public string CaseRequest()
        {
            var guid = Request["guid"].ToString();
            var msgType = Request["msgType"].ToString();
            var msgBody = Request["msgBody"].ToString();
            Timer.CaseRequest(guid, msgType, msgBody);
            return "";
        }

        public void Redis()
        {
            try
            {
                var client = new RedisClient("10.0.66.236", 6379);
                client.Set("name", "redis value");
                var a = client.Get<string>("name");
                Console.WriteLine(client.Get<string>("name"));
                var q = Guid.NewGuid();
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
        }
    }
}