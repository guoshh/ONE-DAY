/******************************************
 * Description:Js对象转换使用
 * Author:郭盛寒
 * CreateDate:2018-5-7 
 **********************************************
 */
using System.Collections.Generic;
using Cont.DataHelperEntity;

namespace Cont.Web.App_Code.Com
{
    public class JsonObject
    {
        /// <summary>
        /// 根据Json串返回FormData（ListDictionary）对象
        /// </summary>
        /// <param name="Json"><seealso cref="Json串"/></param>
        /// <returns></returns>
        public static List<FormData> GetListFormDataByJson(string jsonStr)
        {
            //格式如：[{"formcode":"formid1","data":[{"BID":"","TITLE":"","PRICE":"0","CONTENT":"108将，三打祝家庄、李逵\r\n武松，\"一丈青\"，‘爽快’","PUBDATE":""},{"BID":"刊号ISNO98988","TITLE":"书名/三打白骨精","PRICE":"10.89","CONTENT":"唐僧师徒四人为取真经，行至白虎岭前。在白虎岭内，住着一个尸魔白骨精。为了吃唐僧肉，先后变幻为村姑、妇人，全被孙悟空识破。但唐僧却不辨人妖，反而责怪孙悟空恣意行凶，连伤母女两命，违反戒律。第三次白骨精又变成白发老公公又被孙悟空识破。唐僧写下贬书，将孙悟空赶回了花果山。","PUBDATE":"2018-09-11"}]},{"formid":"formid2","data":[{"Project_NAME":"采伐设计工程","HUMAN":"负责人","Here":"测试下面的全局属性可用于任何 HTML元素"}]}]
            IList<Dictionary<string, object>> listData = Cont.Utility.Json.Deserialize<List<Dictionary<string, object>>>(jsonStr);
            List<FormData> ListFormData = new List<FormData>();
            foreach (Dictionary<string, object> dictionary in listData)
            {
                FormData formData = new FormData();
                foreach (var item in dictionary)
                {
                    if (item.Key.ToLower() == "tablecode")
                    {
                        formData.TableCode = item.Value.ToString();
                    }
                    if (item.Key.ToLower() == "data")
                    {
                        IList<Dictionary<string, object>> listDictionary = Cont.Utility.Json.Deserialize<List<Dictionary<string, object>>>(item.Value.ToString());
                        formData.ListDictionary = listDictionary;
                    }
                }
                ListFormData.Add(formData);
            }
            return ListFormData;
        }
    }
}