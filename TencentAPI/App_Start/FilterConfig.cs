using System.Web;
using System.Web.Mvc;

namespace TencentAPI
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //监控日志
            filters.Add(new Log.TrackerFilter());

            filters.Add(new HandleErrorAttribute());
        }
    }
}
