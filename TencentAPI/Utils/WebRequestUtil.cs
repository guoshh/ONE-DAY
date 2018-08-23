using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Specialized;
using System.Net.Security;
using System.Security.Cryptography;
using System.Diagnostics;

namespace Transport.Utils
{
    /// <summary>
    /// Web请求工具类
    /// </summary>
    public class WebRequestUtil
    {
        #region 参数

        /// <summary>
        /// 请求URL
        /// </summary>
        public string url { get; set; }
        /// <summary>
        /// 请求参数（POST方式，如果为空，将把URL作为数据流提交后台）
        /// </summary>
        public IDictionary<string, string> parameters { get; set; }

        /// <summary>
        /// 上传文件
        /// </summary>
        public IDictionary<string, HttpPostedFileBase> files { get; set; }

        /// <summary>
        /// 头文件
        /// </summary>
        public IDictionary<string, string> headers { get; set; }
        /// <summary>
        /// 证书
        /// </summary>
        public ICredentials credentials { get; set; }
        /// <summary>
        /// 请求方法
        /// </summary>
        public string method { get; set; }
        /// <summary>
        /// 当前Cookie
        /// </summary>
        public CookieContainer CurrentCookie { get; set; }
        /// <summary>
        /// 请求编码
        /// </summary>
        public string Encode { get; set; }
        /// <summary>
        /// 请求超时（毫秒）
        /// </summary>
        public int Timeout { get; set; }

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="url">请求URL</param>
        /// <param name="method">请求方法</param>
        /// <param name="Encode">请求编码</param>
        /// <param name="Timeout">请求超时（毫秒）</param>
        public WebRequestUtil(string url = null, string method = "post", string Encode = "utf-8", int Timeout = 2000)
        {
            Init(url, method, Encode, Timeout);
        }

        /// <summary>
        /// 初始化Web请求
        /// </summary>
        /// <param name="url">请求URL</param>
        /// <param name="method">请求方法</param>
        /// <param name="Encode">请求编码</param>
        /// <param name="Timeout">请求超时（毫秒）</param>
        public void Init(string url = null, string method = "post", string Encode = "utf-8", int Timeout = 2000)
        {
            this.url = url;
            this.parameters = new Dictionary<string, string>();
            this.files = new Dictionary<string, HttpPostedFileBase>();
            this.headers = new Dictionary<string, string>();
            this.method = method;
            this.credentials = CredentialCache.DefaultCredentials;

            this.Encode = Encode;
            this.Timeout = Timeout;
        }

        #endregion

        #region 方法

        #region HTTP 请求

        /// <summary>
        /// HTTP(GET/POST 键值对参数列表)
        /// </summary>
        /// <returns>响应内容</returns>
        public string SendPost()
        {
            if (this.files.Count > 0)
                return HttpUploadFile(this.files.ToDictionary(k => k.Key, v => v.Value as object));

            HttpWebRequest req = null;
            HttpWebResponse rsp = null;
            Stream reqStream = null;
            Encoding encoding = null;
            try
            {
                if (method.ToLower() == "post")
                {
                    req = (HttpWebRequest)WebRequest.Create(url);
                    req.Method = "POST";
                    req.KeepAlive = false;
                    req.ProtocolVersion = HttpVersion.Version10;
                    req.Timeout = Timeout;
                    //头文件
                    if (headers.Count > 0)
                    {
                        foreach (KeyValuePair<string, string> header in headers)
                            if (!WebHeaderCollection.IsRestricted(header.Key))
                                req.Headers.Add(header.Key, header.Value);
                            else
                            {
                                req.Host = "14.17.22.55:8028";
                                req.ContentType = "application/json";
                            }
                    }
                    //证书
                    if (credentials != null)
                        req.Credentials = credentials;

                    req.CookieContainer = CurrentCookie ?? (CurrentCookie = new CookieContainer());
                    reqStream = req.GetRequestStream();
                    if (parameters.Count > 0)
                    {
                        byte[] postData = null;
                        //StreamWriter myStreamWriter = new StreamWriter(reqStream, Encoding.GetEncoding("utf-8"));
                        //postData = Encoding.UTF8.GetBytes(BuildQuery(parameters, false, Encode));
                        postData = Encoding.UTF8.GetBytes(BuildQueryTest(parameters, false, Encode));
                        reqStream.Write(postData, 0, postData.Length);
                    }
                    else
                    {
                        byte[] postData = Encoding.GetEncoding(Encode).GetBytes(url);
                        reqStream.Write(postData, 0, postData.Length);
                    }
                }
                ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });
                try
                {
                    rsp = (HttpWebResponse)req.GetResponse();
                }
                catch (WebException ex)
                {
                    var rs = ex.Response.GetResponseStream();
                    var sr = new StreamReader(rs, System.Text.Encoding.UTF8);

                    var Error = sr.ReadToEnd();
                    rs.Close();
                    sr.Close();

                    Debug.Write(ex.Message);//远程服务器返回错误: (400) 错误的请求。  
                    var strResponse = GetResponseAsString((HttpWebResponse)ex.Response, encoding);//这样获取web服务器返回数据  
                }
                if (CurrentCookie.Count == 0)
                    CurrentCookie.Add(rsp.Cookies);

                encoding = Encoding.GetEncoding(rsp.CharacterSet ?? Encode);
                return GetResponseAsString(rsp, encoding);
            }
            finally
            {
                if (reqStream != null) reqStream.Close();
                if (rsp != null) rsp.Close();
            }
        }

        /// <summary>
        /// HTTP(POST JSON)
        /// </summary>
        /// <returns></returns>
        public string SendPostJson()
        {
            HttpWebRequest req = null;
            HttpWebResponse rsp = null;
            Stream reqStream = null;

            try
            {
                string body = JsonConvert.SerializeObject(parameters);

                Encoding encoding = Encoding.GetEncoding(Encode);
                req = (HttpWebRequest)WebRequest.Create(url);
                req.Host = "xxx.tencent.com";
                req.Method = "POST";
                req.Accept = "text/html, application/xhtml+xml, */*";
                req.ContentType = "application/json; charset=utf-8";
                req.Timeout = Timeout;

                byte[] buffer = encoding.GetBytes(body);
                req.ContentLength = buffer.Length;

                //头文件
                if (headers.Count > 0)
                {
                    foreach (KeyValuePair<string, string> header in headers)
                        req.Headers.Add(header.Key, header.Value);
                }

                //证书
                if (credentials != null)
                    req.Credentials = credentials;

                req.CookieContainer = CurrentCookie ?? (CurrentCookie = new CookieContainer());

                reqStream = req.GetRequestStream();
                reqStream.Write(buffer, 0, buffer.Length);

                rsp = (HttpWebResponse)req.GetResponse();

                if (CurrentCookie.Count == 0)
                    CurrentCookie.Add(rsp.Cookies);

                encoding = Encoding.GetEncoding(rsp.CharacterSet ?? Encode);
                return GetResponseAsString(rsp, encoding);
            }
            finally
            {
                if (reqStream != null) reqStream.Close();
                if (rsp != null) rsp.Close();
            }
        }

        #endregion

        #region HTTP 上传文件

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="files">文件列表（name:本地文件地址/上传文件）</param>
        /// <returns></returns>
        public string HttpUploadFile(Dictionary<string, object> files)
        {
            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            byte[] boundarybytes = Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");
            byte[] endbytes = Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");

            Encoding encoding = Encoding.GetEncoding(Encode);

            //1.HttpWebRequest
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = "multipart/form-data; boundary=" + boundary;
            request.Method = "POST";
            request.KeepAlive = true;
            request.Credentials = credentials;
            request.Timeout = Timeout;

            //头文件
            if (headers.Count > 0)
            {
                foreach (KeyValuePair<string, string> header in headers)
                    request.Headers.Add(header.Key, header.Value);
            }

            using (Stream stream = request.GetRequestStream())
            {
                //1.1 key/value
                string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
                if (parameters.Count > 0)
                {
                    foreach (var item in parameters)
                    {
                        stream.Write(boundarybytes, 0, boundarybytes.Length);
                        string formitem = string.Format(formdataTemplate, item.Key, item.Value);
                        byte[] formitembytes = encoding.GetBytes(formitem);
                        stream.Write(formitembytes, 0, formitembytes.Length);
                    }
                }

                //1.2 file
                string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: application/octet-stream\r\n\r\n";
                byte[] buffer = new byte[4096];
                int bytesRead = 0;

                foreach (var file in files)
                {
                    stream.Write(boundarybytes, 0, boundarybytes.Length);

                    string header = string.Empty;
                    string fileName = string.Empty;

                    if (file.Value is string)
                    {
                        fileName = file.Value.ToString();
                        header = string.Format(headerTemplate, file.Key, Path.GetFileName(fileName));
                    }
                    else if (file.Value is HttpPostedFileBase)
                    {
                        fileName = (file.Value as HttpPostedFileBase).FileName;
                        header = string.Format(headerTemplate, file.Key, Path.GetFileName(fileName));
                    }

                    byte[] headerbytes = encoding.GetBytes(header);
                    stream.Write(headerbytes, 0, headerbytes.Length);

                    if (file.Value is string) //文件路径
                    {
                        using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                        {
                            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                stream.Write(buffer, 0, bytesRead);
                            }
                        }
                    }
                    else if (file.Value is HttpPostedFileBase) //上传文件
                    {
                        HttpPostedFileBase httpPostedFileBase = file.Value as HttpPostedFileBase;

                        //临时上传文件
                        string tmpUploadFileName = string.Format("C:/tmp{0}.upload", Guid.NewGuid().ToString());

                        httpPostedFileBase.SaveAs(tmpUploadFileName);

                        using (FileStream fileStream = new FileStream(tmpUploadFileName, FileMode.Open, FileAccess.Read))
                        {
                            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) > 0)
                                stream.Write(buffer, 0, bytesRead);
                        }

                        //删除临时上传文件
                        File.Delete(tmpUploadFileName);
                    }
                }

                //1.3 form end
                stream.Write(endbytes, 0, endbytes.Length);
            }
            //2.WebResponse
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader stream = new StreamReader(response.GetResponseStream()))
            {
                return stream.ReadToEnd();
            }
        }

        #endregion

        #region 私用

        /// <summary>
        /// 把响应流转换为文本。
        /// </summary>
        /// <param name="rsp">响应流对象</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>响应文本</returns>
        static string GetResponseAsString(HttpWebResponse rsp, Encoding encoding)
        {
            System.IO.Stream stream = null;
            StreamReader reader = null;
            try
            {
                // 以字符流的方式读取HTTP响应
                stream = rsp.GetResponseStream();
                reader = new StreamReader(stream, encoding);
                return reader.ReadToEnd();
            }
            finally
            {
                // 释放资源
                if (reader != null) reader.Close();
                if (stream != null) stream.Close();
                if (rsp != null) rsp.Close();
            }
        }

        #endregion

        #region 公用

        /// <summary>
        /// 组装普通文本请求参数。
        /// </summary>
        /// <param name="parameters">Key-Value形式请求参数字典</param>
        /// <param name="ignoreNullValue">忽略参数名为空的参数</param>
        /// <param name="encode"></param>
        /// <returns>URL编码后的请求数据</returns>
        public static string BuildQuery(IDictionary<string, string> parameters, bool ignoreNullValue = false, string encode = "utf-8")
        {
            StringBuilder postData = new StringBuilder();
            bool hasParam = false;

            IEnumerator<KeyValuePair<string, string>> dem = parameters.GetEnumerator();
            while (dem.MoveNext())
            {
                string name = dem.Current.Key;
                string value = dem.Current.Value;
                // 忽略参数名或参数值为空的参数
                if (!string.IsNullOrWhiteSpace(name) && ignoreNullValue ? true : !string.IsNullOrWhiteSpace(value))
                {
                    if (hasParam)
                        postData.Append("&");
                    postData.Append(name);
                    postData.Append("=");
                    if (encode.Equals("string"))
                    {
                        postData.Append(value);
                    }
                    else
                    {
                        postData.Append(HttpUtility.UrlEncode(value, Encoding.GetEncoding(encode)));
                    }

                    hasParam = true;
                }
            }
            return postData.ToString();
        }

        /// <summary>
        /// Form表单转字典
        /// </summary>
        /// <param name="form">form表单集合</param>
        /// <returns></returns>
        public static IDictionary<string, object> Form2Dictionary(NameValueCollection form)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            for (int i = 0; i < form.Count; i++)
            {
                object value = form[i];
                if ((value != null && value.Equals("true,false")))
                    value = true;
                dict.Add(form.AllKeys[i], value);
            }

            return dict;
        }

        /// <summary>
        /// From表单转对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="form">from表单集合</param>
        /// <returns></returns>
        public static T Form2Object<T>(NameValueCollection form)
        {
            var req = HttpContext.Current.Request;
            T request = JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(Form2Dictionary(form)));
            foreach (var p in request.GetType().GetProperties())
            {
                if (p.PropertyType.Equals(typeof(HttpPostedFileBase))) //单文件
                {
                    var file =
                        req.Files[p.Name] != null &&
                        req.Files[p.Name].ContentLength > 0 ?
                        new HttpPostedFileWrapper(req.Files[p.Name]) :
                        null;

                    if (file != null)
                        p.SetValue(request, file);
                }
                else if (p.PropertyType.Equals(typeof(HttpPostedFileBase[]))) //多文件
                {
                    var mfiles = req.Files.GetMultiple(p.Name).Where(f => f != null && f.ContentLength > 0).ToList();
                    HttpPostedFileBase[] files = new HttpPostedFileBase[mfiles.Count];
                    for (int i = 0; i < mfiles.Count; i++)
                    {
                        var file = mfiles[i];
                        files[i] = new HttpPostedFileWrapper(file);
                    }

                    if (files.Length > 0)
                        p.SetValue(request, files);
                }
            }

            return request;
        }

        /// <summary>
        /// 请求追加参数
        /// </summary>
        /// <param name="parms">参数对象</param>
        /// <param name="filter">如果参数为JObject，则需要参数过滤</param>
        public void AddParms(object parms, List<string> filter = null)
        {
            if (!(parms is JObject))
            {
                var parmsList = parms.GetType().GetProperties().ToList().Where(p => p.GetValue(parms) != null).Select((p) =>
                {
                    return new
                    {
                        Key = p.Name,
                        Value = p.GetValue(parms)
                    };
                });

                foreach (var parm in parmsList)
                {
                    if (parm.Value is HttpPostedFileBase)
                    {
                        this.files.Add(parm.Key, parm.Value as HttpPostedFileBase);
                    }
                    else
                    {
                        this.parameters.Add(parm.Key, parm.Value.ToString());
                    }
                }
            }
            else
            {
                JToken jObject = parms as JObject;
                foreach (JProperty jProperty in jObject)
                {
                    //参数过滤器不为空 或 不区分大小写
                    if (filter == null || filter.Contains(jProperty.Name, StringComparer.OrdinalIgnoreCase))
                        this.parameters.Add(jProperty.Name, jProperty.Value.ToString());
                }
            }
        }

        #region POST 参数列表

        /// <summary>
        /// JSON POST请求
        /// </summary>
        /// <typeparam name="T">返回的结果类型</typeparam>
        /// <param name="parms">参数对象</param>
        /// <param name="filter">如果参数为JObject，则需要参数过滤</param>
        /// <returns></returns>
        public T PostJsonByParms<T>(JObject parms, List<string> filter = null) where T : class
        {
            return WebRequestByParms<T>(parms, filter);
        }

        /// <summary>
        /// Web POST请求
        /// </summary>
        /// <typeparam name="T">返回的结果类型</typeparam>
        /// <param name="parms">参数对象</param>
        /// <param name="filter">如果参数为JObject，则需要参数过滤</param>
        /// <returns></returns>
        public T WebRequestByParms<T>(object parms, List<string> filter = null) where T : class
        {
            AddParms(parms, filter);

            if (this.parameters.Count > 0)
                return JsonConvert.DeserializeObject<T>(this.SendPost());

            return null;
        }

        #endregion

        #endregion

        #endregion
        #region 时间戳处理
        /// 获取时间戳
        /// </summary>
        /// <returns></returns>
        public static string GetTimeStamp(System.DateTime time)
        {
            long ts = ConvertDateTimeToInt(time);
            return ts.ToString();
        }
        /// <summary>  
        /// 将c# DateTime时间格式转换为Unix时间戳格式  
        /// </summary>  
        /// <param name="time">时间</param>  
        /// <returns>long</returns>  
        public static long ConvertDateTimeToInt(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
            long t = (time.Ticks - startTime.Ticks) / 10000;   //除10000调整为13位      
            return t;
        }
        #endregion
        #region SHA256自行校验
        public static string SHA256(string str)
        {
            byte[] SHA256Data = Encoding.UTF8.GetBytes(str);
            SHA256Managed Sha256 = new SHA256Managed();
            byte[] by = Sha256.ComputeHash(SHA256Data);
            return BitConverter.ToString(by).Replace("-", "");
        }
        #endregion
        public static string BuildQueryTest(IDictionary<string, string> parameters, bool ignoreNullValue = false, string encode = "utf-8")
        {
            StringBuilder postData = new StringBuilder();
            bool hasParam = false;

            IEnumerator<KeyValuePair<string, string>> dem = parameters.GetEnumerator();
            while (dem.MoveNext())
            {
                string name = dem.Current.Key;
                string value = dem.Current.Value;
                // 忽略参数名或参数值为空的参数
                if (!string.IsNullOrWhiteSpace(name) && ignoreNullValue ? true : !string.IsNullOrWhiteSpace(value))
                {
                    if (hasParam)
                        postData.Append("&"+ name+ "=");
                        //postData.Append(name);
                        //postData.Append("=");
                        postData.Append(value);
                    hasParam = true;
                }
            }
            return postData.ToString();
        }
    }
}