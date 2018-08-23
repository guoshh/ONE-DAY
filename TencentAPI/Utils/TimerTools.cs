using Cont.DataHelperEntity;
using Cont.Web.App_Code.Com;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using TencentAPI.DTO;
using Transport.Utils;

namespace TencentAPI.Utils
{
    public class TimerTools
    {
        public static string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["connstr1"].ConnectionString;
        IDbConnection conn = new SqlConnection(connectionString);
        HanaHelper Hana = new HanaHelper();

        /// <summary>
        /// 获取消息接口
        /// </summary>
        public void TimerRequest()
        {

            string Url = "vendor-svc/vendor/getMsgs";
            string returnjson = Requests(Url, "", "");
            List<Msg> dbMsg = JsonConvert.DeserializeObject<Msgs>(returnjson).Msg;
            string guid = "";
            string msgType = "";
            string msgBody = "";
            string createTime = "";
            string description = "";
            for (int i = 0; i < dbMsg.Count; i++)
            {
                guid = dbMsg[i].guid;
                msgType = dbMsg[i].msgType;
                msgBody = dbMsg[i].msgBody;
                createTime = dbMsg[i].createTime;
                description = dbMsg[i].description;
                CaseRequest(guid, msgType, msgBody);
            }
        }
        /// <summary>
        /// 执行调用接口
        /// </summary>
        /// <param name="Url"></param>
        /// <param name="guid"></param>
        /// <param name="Jsonstring"></param>
        /// <returns></returns>
        public string Requests(string Url, string guid, string Jsonstring)
        {
            //string query = "SELECT * FROM tb_Signature";
            //List<Signature> DBSignature = conn.Query<Signature>(query).ToList();
            //var LastDBSignature = DBSignature.LastOrDefault();
            //var ClientId = LastDBSignature.ClientId;
            //var SecretKey = LastDBSignature.SecretKey;         
            var ClientId = "aa40ee8b-fb21";
            var SecretKey = "ebdd6350-524b-6d12";
            var ts = WebRequestUtil.GetTimeStamp(DateTime.Now);
            var sign = WebRequestUtil.SHA256(ClientId + "," + SecretKey + "," + ts);
            Dictionary<string, string> headers = new Dictionary<string, string>
                {
                    { "Host", "14.17.22.55:8028"},
                    { "Content-Type", "application/json; charset=UTF-8"},
                    { "Cache-Control", "no-cache" },
                };
            Dictionary<string, string> dict = new Dictionary<string, string>();
            if (guid != "")
                dict.Add("guid", guid);
            if (Jsonstring != "")
                dict.Add("", Jsonstring);

            WebRequestUtil webClieng = new WebRequestUtil
            {
                url = "http://14.17.22.55:8028/public-api/" + Url + "?clientId=" + ClientId + "&sign=" + sign + "&ts=" + ts,
                headers = headers,
                parameters = dict,
                method = "post",
                Timeout = 6000,
                Encode = "utf-8"
            };
            string returnJson = "";
            try
            {
                returnJson = webClieng.SendPost();
            }
            catch (Exception ex)
            {
                throw;
            }
            return returnJson;
        }
        /// <summary>
        /// 轮询执行方案
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="msgType"></param>
        /// <param name="msgBody"></param>
        public void CaseRequest(string guid, string msgType, string msgBody)
        {
            string Url = "";
            string Jsonstring = "";
            string ReturnJson = "";
            #region 发送部件采购单信息 
            if (msgType == "PART_ORDER_ACCEPT")
            {
                Url = "sodm-svc/partOrder/getPartOrderAccept?orderNumber=" + msgBody;
                #region Json类
                var PartPurchaseOrderSendModel = new PartPurchaseOrderSend()
                {
                    orderNumber = "",
                    vendorName = "",
                    createDate = "",
                    companyName = "",
                    currency = "",
                    orderType = "",
                    details = new List<DetailsItem0>()
                    {
                        new DetailsItem0
                        {
                        detailId = 10001,
                        matiCode = "795050DBIK6471",
                        quantity = 1000,
                        deliveryDate = "2018-02-01",
                        storage = "仓库名称",
                        address = "详细送货地址",
                        price = "10.00",
                        memo = ""
                        }
                    }
                };
                #endregion
                Jsonstring = JsonConvert.SerializeObject(PartPurchaseOrderSendModel).ToString();
                ReturnJson = Requests(Url, guid, Jsonstring);
                ReturnMsg dbmsg = JsonConvert.DeserializeObject<ReturnMsg>(ReturnJson);
                string code = dbmsg.code;
                string data = dbmsg.data;
                string message = dbmsg.message;
            }
            #endregion
            #region 发送订货单信息  反馈订货单交期,采购机型
            if (msgType == "BTO_SUBMIT")
            {
                Url = "sodm-svc/bo/getBoData?boNumber=" + msgBody;
                #region Json类
                DataTable dt_DetailsItem1 = Hana.GetTable("zbodata", "detailId,matiCode,matiName,quantity,deliveryDate,deliveryStorage,address,odmBrand,country,configurations");
                var OrderMsgSendModel = new OrderMsgSend();
                OrderMsgSendModel.details = new List<DetailsItem1>();
                OrderMsgSendModel.createTime = DateTime.Now;
                OrderMsgSendModel.boNumber = "";
                for (int i = 0; i < dt_DetailsItem1.Rows.Count; i++)
                {
                    DetailsItem1 details = new DetailsItem1()
                    {
                        detailId = dt_DetailsItem1.Rows[i]["matiCode"].ToString(), //订货单明细 ID（对应数据库明细表主键 ID）
                        matiCode = dt_DetailsItem1.Rows[i]["matiCode"].ToString(), //物料编码
                        matiName = dt_DetailsItem1.Rows[i]["matiCode"].ToString(), //物品描述
                        quantity = Convert.ToInt32(dt_DetailsItem1.Rows[i]["quantity"]), //数量
                        deliveryDate = Convert.ToDateTime(dt_DetailsItem1.Rows[i]["quantity"]).ToString("yyyy-MM-dd"), //期望的配送日期
                        deliveryStorage = dt_DetailsItem1.Rows[i]["matiCode"].ToString(), //收货仓库
                        address = dt_DetailsItem1.Rows[i]["matiCode"].ToString(), //收货地址
                        odmBrand = dt_DetailsItem1.Rows[i]["matiCode"].ToString(),
                        configurations = new ConfigurationsItem()
                        {
                            key = "configDesc",
                            value = "配置信息"
                        }
                    };
                    OrderMsgSendModel.details.Add(details);
                }
                #endregion
                Jsonstring = JsonConvert.SerializeObject(OrderMsgSendModel).ToString();
                ReturnJson = Requests(Url, guid, Jsonstring);
                ReturnMsg dbmsg = JsonConvert.DeserializeObject<ReturnMsg>(ReturnJson);
                string code = dbmsg.code;
                string data = dbmsg.data;
                string message = dbmsg.message;
                TagProcessed(msgBody);//返回处理成功结果
            }
            #endregion
            #region 发送订货单信息   订单配送信息   订单配送信息,更新 ODM 配送信息及上架信息
            if (msgType == "ORDER_SUBMIT" || msgType == "ORDER_DELIVERY_CHANGE")
            {
                Url = "sodm-svc/bo/getPoDota?poNumber=" + msgBody;
                ReturnJson = Requests(Url, guid, Jsonstring);
                OrderDeliveryInformationRequest dbOrderDeliveryInformation = JsonConvert.DeserializeObject<OrderDeliveryInformationRequest>(ReturnJson);
                string poNumber = dbOrderDeliveryInformation.poNumber;
                List<DetailsItem> Details = dbOrderDeliveryInformation.Details;
                for (int i = 0; i < Details.Count(); i++)
                {
                    string detailId = Details[i].detailId; //订单行明细 ID
                    string prNumber = Details[i].prNumber; //需求单号
                    string boNumber = Details[i].boNumber; //订货单号
                    string estDeliveryDate = Details[i].estDeliveryDate; // bo 单反馈的日期
                    string memo = Details[i].memo; //备注",
                    string snCode = Details[i].snCode; //SN 码
                    string matiCode = Details[i].matiCode; //物料编码
                    string matiName = Details[i].matiName; //"传统服务器 腾讯 Y0-TS80
                    string assetCode = Details[i].assetCode; //资产编码
                    int quantity = Details[i].quantity;//数量
                    string deliveryDate = Details[i].deliveryDate; //期望配送日期
                    string deliveryStorage = Details[i].deliveryStorage; //配送仓库
                    string address = Details[i].address; //配送地址
                    string positionName = Details[i].positionName; //机架名称
                    int subPositionName = Details[i].subPositionName; //机位名称
                    List<configurationsItem> configurations = Details[i].configurations;
                    for (int j = 0; j < configurations.Count(); j++)
                    {
                        string key = configurations[j].key;
                        string value = configurations[j].value;
                    }
                }

            }
            #endregion
        }
        /// <summary>
        /// 标记消息为已处理 
        /// </summary>
        /// <param name="guid"></param>
        public void TagProcessed(string guid)
        {
            string Url = "vendor-svc/vendor/handleMsg";
            string returnjson = Requests(Url, guid, "");
            ReturnMsg dbMsg = JsonConvert.DeserializeObject<ReturnMsg>(returnjson);
            string code = dbMsg.code;
            string message = dbMsg.message;
            string Success = dbMsg.Success;
            string data = dbMsg.data;
        }
        /// <summary>
        /// 获取需求预测
        /// </summary>
        public void TimerDemandForecastRequest()
        {

            string Url = "sodm-svc/forecast/getForecasts";
            string Jsonstring = Requests(Url, "", "");
            DemandForecastRequest dbDemandForecast = JsonConvert.DeserializeObject<DemandForecastRequest>(Jsonstring);
            List<BackupPartDatasItem> backupPartDatas = dbDemandForecast.backupPartDatas;
            List<PartDatasItem> partDatas = dbDemandForecast.partDatas;
            List<ModelDatasItem> modelDatas = dbDemandForecast.modelDatas;
            for (int i = 0; i < backupPartDatas.Count; i++)
            {
                string weekName = backupPartDatas[i].weekName;
                string matiCode = backupPartDatas[i].matiCode;
                string matiName = backupPartDatas[i].matiName;
                int quantity = backupPartDatas[i].quantity;
                string partType = backupPartDatas[i].partType;
                string createDate = backupPartDatas[i].createDate;
                string beginDate = backupPartDatas[i].beginDate;
                string endDate = backupPartDatas[i].endDate;
                string area = backupPartDatas[i].area;
            }
            for (int i = 0; i < partDatas.Count; i++)
            {
                string weekName = partDatas[i].weekName;
                string matiCode = partDatas[i].matiCode;
                string matiName = partDatas[i].matiName;
                int quantity = partDatas[i].quantity;
                string partType = partDatas[i].partType;
                string createDate = partDatas[i].createDate;
                string beginDate = partDatas[i].beginDate;
                string endDate = partDatas[i].endDate;
                string area = partDatas[i].area;

            }
            for (int i = 0; i < modelDatas.Count; i++)
            {
                string weekName = modelDatas[i].weekName;
                string modelName = modelDatas[i].modelName;
                int quantity = modelDatas[i].quantity;
                string createDate = modelDatas[i].createDate;
                string beginDate = modelDatas[i].beginDate;
                string endDate = modelDatas[i].endDate;
                string area = modelDatas[i].area;
            }
        }
        /// <summary>
        /// 定期向腾讯发送部件详细数据,
        /// </summary>
        public void TimerPartDatailsSend()
        {
            DataTable dt_AvailableDatasItem = Hana.GetTable("ZAVAILABLEDATAS", "matiCode,quantity,area");
            DataTable dt_UnavailableDatasItem = Hana.GetTable("ZunavailableData", "matiCode,quantity,status,area");
            DataTable dt_OnWayDatasItem = Hana.GetTable("ZONWAYDATAS", "matiCode,quantity,deliveryDate,poNumber,area");

            var PartDatailsSendModel = new PartDatailsSend();
            PartDatailsSendModel.availableDatas = new List<AvailableDatasItem>();
            PartDatailsSendModel.unavailableDatas = new List<UnavailableDatasItem>();
            PartDatailsSendModel.onWayDatas = new List<OnWayDatasItem>();
            for (int i = 0; i < dt_AvailableDatasItem.Rows.Count; i++)
            {
                AvailableDatasItem availableDatas = new AvailableDatasItem()
                {
                    matiCode = dt_AvailableDatasItem.Rows[i]["matiCode"].ToString(), //物料编码
                    quantity = Convert.ToInt32(dt_AvailableDatasItem.Rows[i]["quantity"]), //库存数量
                    area = dt_AvailableDatasItem.Rows[i]["area"].ToString() //所在区域
                };
                PartDatailsSendModel.availableDatas.Add(availableDatas);
            };
            for (int i = 0; i < dt_UnavailableDatasItem.Rows.Count; i++)
            {
                UnavailableDatasItem unavailableDatas = new UnavailableDatasItem()
                {
                    matiCode = dt_AvailableDatasItem.Rows[i]["matiCode"].ToString(),
                    quantity = Convert.ToInt32(dt_AvailableDatasItem.Rows[i]["quantity"]),
                    status = dt_AvailableDatasItem.Rows[i]["status"].ToString(), //不良品
                    area = dt_AvailableDatasItem.Rows[i]["area"].ToString()
                };
                PartDatailsSendModel.unavailableDatas.Add(unavailableDatas);
            };
            for (int i = 0; i < dt_OnWayDatasItem.Rows.Count; i++)
            {
                OnWayDatasItem onWayDatas = new OnWayDatasItem()
                {
                    matiCode = dt_AvailableDatasItem.Rows[i]["matiCode"].ToString(),
                    quantity = Convert.ToInt32(dt_AvailableDatasItem.Rows[i]["quantity"]),
                    deliveryDate = Convert.ToDateTime(dt_AvailableDatasItem.Rows[i]["status"]).ToString("yyyy-MM-dd"), //派送日期
                    poNumber = dt_AvailableDatasItem.Rows[i]["poNumber"].ToString(), //在途订单号
                    area = dt_AvailableDatasItem.Rows[i]["area"].ToString()
                };
                PartDatailsSendModel.onWayDatas.Add(onWayDatas);
            };
            string Jsonstring = JsonConvert.SerializeObject(PartDatailsSendModel).ToString();
            string Url = "sodm-svc/stock/pushStockPart";
            Requests(Url, "", Jsonstring);
        }
        /// <summary>
        /// 定期向腾讯发送整机库存数据
        /// </summary>
        public void TimerTotalInventorySend()
        {
            #region Json类
            DataTable dt_AvailableDatasItem = Hana.GetTable("ZSERVERDATAS", "matiCode,quantity,assetCode,snCode,area,storageName");
            var TotalInventorySendModel = new TotalInventorySend();
            TotalInventorySendModel.serverDatas = new List<serverDatasItem>();
            for (int i = 0; i < dt_AvailableDatasItem.Rows.Count; i++)
            {
                serverDatasItem serverDatas = new serverDatasItem()
                {
                    matiCode = dt_AvailableDatasItem.Rows[i]["matiCode"].ToString(),
                    quantity = Convert.ToInt32(dt_AvailableDatasItem.Rows[i]["matiCode"]),
                    assetCode = dt_AvailableDatasItem.Rows[i]["matiCode"].ToString(),
                    snCode = dt_AvailableDatasItem.Rows[i]["matiCode"].ToString(),
                    area = dt_AvailableDatasItem.Rows[i]["matiCode"].ToString(),
                    storageName = dt_AvailableDatasItem.Rows[i]["matiCode"].ToString()
                };
                TotalInventorySendModel.serverDatas.Add(serverDatas);
            }
            #endregion
            string Jsonstring = JsonConvert.SerializeObject(TotalInventorySendModel).ToString();
            string Url = "sodm-svc/stock/pushStockServer";
            Requests(Url, "", Jsonstring);
        }
        /// <summary>
        /// 定期向腾讯发送供应能力
        /// </summary>
        public string TimerSupplyCapacitySend()
        {
            DataTable dtPart = Hana.GetTable("ZpartDatas", "matiCode,partType,weekName,quantity,area");

            #region Json类

            SupplyCapacitySend SupplyCapacitySendModel = new SupplyCapacitySend();
            SupplyCapacitySendModel.partDatas = new List<PartDatasItem1>();
            for (int i = 0; i < dtPart.Rows.Count; i++)
            {
                PartDatasItem1 PartData = new PartDatasItem1()
                {
                    matiCode = "795050ABHV8243", //物料编码
                    partType = "CPU", //部件类型 CPU,HDD,SSD,MEM,L6
                    weekName = "2018-01W", //能够提供的部件物料时间（按周）
                    quantity = 1000, //数量
                    area = "中国" //所在区域
                };
                SupplyCapacitySendModel.partDatas.Add(PartData);
            };
            #endregion
            string Jsonstring = JsonConvert.SerializeObject(SupplyCapacitySendModel).ToString();
            string Url = "sodm-svc/stock/pushSupplyPart";
            return Requests(Url, "", Jsonstring);
        }
        /// <summary>
        /// 发送整机订单生产信息
        /// </summary>
        public void MachineOrderProductionInformationSend()
        {
            #region Json类
            var MachineOrderProductionInformationSendModel = new MachineOrderProductionInformationSend()
            {
                poNumber = "PO201801010001",
                details = new List<details>()
                {
                    new details()
                    {
                        detailId = 10001,//订单行明细 ID
                        assetCode = "TYSV2018010001", //资产编码
                        snCode = "xxxxxxxxxxxxxxxxxxxx", //SN 码
                        estimateDeliveryDate = "2018-03-01", //预计配送日期
                        actArriveDate = "2018-03-04", //实际到货日期
                        vendorMemo = "延期原因备注", //备注
                        status = "生产", //状态信息：生产、配送
                        deliverNumber = "", //物流单号
                        vendorOutStorageTime = "2018-05-09 12:00:00", //出库时间
                    }
                }
            };
            string Jsonstring = JsonConvert.SerializeObject(MachineOrderProductionInformationSendModel).ToString();
            string Url = "sodm-svc/po/sendPoFeedback";
            string ReturnJson = "";
            var messages = "";
            try
            {
                ReturnJson = Requests(Url, "", Jsonstring);
            }
            catch (Exception ex)
            {
                messages = ex.Message;
                throw;
            }
            ReturnMsg dbmsg = JsonConvert.DeserializeObject<ReturnMsg>(ReturnJson);
            string code = dbmsg.code;
            string data = dbmsg.data;
            string Success = dbmsg.Success;
            string message = dbmsg.message;
            #endregion
        }
        /// <summary>
        /// 获取周期数据
        /// </summary>
        public void TimerCycleDataRequest()
        {
            string Url = "sodm-svc/weekInfo/getWeekInfo";
            string ReturnJson = Requests(Url, "", "");
            CycleDataRequest dbCycleData = JsonConvert.DeserializeObject<CycleDataRequest>(ReturnJson);
            List<CycleItem> Cycle = dbCycleData.Cycle;
            for (int i = 0; i < Cycle.Count(); i++)
            {
                int seq = Cycle[i].seq;
                string weekName = Cycle[i].weekName;
                string beginDate = Cycle[i].beginDate;
                string endDate = Cycle[i].endDate;
                int year = Cycle[i].year;
                int month = Cycle[i].month;
                int weekOfMonth = Cycle[i].weekOfMonth;
            }

        }
        /// <summary>
        /// 发送部件订单验收信息
        /// </summary>
        public void PartReceiveMsgSend()
        {
            PartReceiveMsgSend PartReceiveMsgSendModel = new PartReceiveMsgSend()
            {
                recNumber = "",
                memo = "",
                area = "",
                storage = "",
                address = "",
                createDate = "",
                details = new List<DetailsItem4>()
                {
                     new DetailsItem4()
                      {
                          detailId = "",
                         orderNumber = "",
                         orderDetailID = "",
                         matiCode = "",
                         matiName = "",
                         recQuantity = "",
                     }
                }
            };
            string Url = "sodm-svc/partOrder/sendPartOrder";
            string Jsonstring = JsonConvert.SerializeObject(PartReceiveMsgSendModel).ToString();
            string ReturnJson = Requests(Url, "", Jsonstring);
            ReturnMsg dbReturnMsg = JsonConvert.DeserializeObject<ReturnMsg>(ReturnJson);
            string Code = dbReturnMsg.code;
            string Data = dbReturnMsg.data;
            string Message = dbReturnMsg.message;
            string Success = dbReturnMsg.Success;
        }
    }
}