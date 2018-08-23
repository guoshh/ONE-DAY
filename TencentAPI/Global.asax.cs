using System;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using TencentAPI.Utils;
using System.Web.Http;

namespace TencentAPI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            log4net.Config.XmlConfigurator.Configure();

            GlobalConfiguration.Configuration.Filters.Add(new Log.ApiTrackerFilter());

            BundleConfig.RegisterBundles(BundleTable.Bundles);



            // 在应用程序启动时运行的代码 这里设置34个小时间隔 122400000 300000
            System.Timers.Timer myTimer = new System.Timers.Timer(60000);//修改时间间隔
                                                                         //关联事件
            myTimer.Elapsed += new System.Timers.ElapsedEventHandler(Timer1_Elapsed);
            myTimer.AutoReset = true;
            myTimer.Enabled = true;
        }

        public void Timer1_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            return;
            TimerTools Timer = new TimerTools();
            // 得到 hour minute second  如果等于某个值就开始执行某个程序。
            int intHour = e.SignalTime.Hour;
            int intMinute = e.SignalTime.Minute;
            int intSecond = e.SignalTime.Second;
            //定制时间； 比如 在10：30 ：00 的时候执行某个函数
            //int iHour = 10;
            //int iMinute = 30;
            //int iSecond = 00;

            //设置 每秒钟的开始执行一次
            //if (intSecond == iSecond)
            //{

            //}
            //设置 每个小时的偶数分钟开始执行

            if (intMinute % 2 == 0)
            {
                Timer.TimerRequest();
                string WeekDay = DateTime.Now.DayOfWeek.ToString();
                //每周一调用一次
                if (WeekDay == "Monday" && intHour == 1 && intMinute == 0)
                {
                    Timer.TimerSupplyCapacitySend();
                    Timer.TimerDemandForecastRequest();//获取需求预测 
                }
            }
            //设置 每天的2点发送部件详细数据
            if ((intHour == 2 && intMinute == 00))
            {
                Timer.TimerPartDatailsSend();
            }
            if ((intHour == 3 && intMinute == 00))
            {
                Timer.TimerTotalInventorySend();
            }
        }

        protected void Application_End()
        {
            //  在应用程序关闭时运行的代码
            //如果出错，删除下面代码
            //下面的代码是关键，可解决IIS应用程序池自动回收的问题
            System.Threading.Thread.Sleep(1000);
            ////这里设置你的web地址，可以随便指向你的任意一个aspx页面甚至不存在的页面，目的是要激发Application_Start
            ////string url = "http://www.xxxxx.com";
            string url = "http://localhost:82/111.aspx";
            System.Net.HttpWebRequest myHttpWebRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
            System.Net.HttpWebResponse myHttpWebResponse = (System.Net.HttpWebResponse)myHttpWebRequest.GetResponse();
            System.IO.Stream receiveStream = myHttpWebResponse.GetResponseStream();//得到回写的字节流

            //在此添加其它代码
        }

    }
}
