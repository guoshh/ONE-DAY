using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;

namespace TencentAPI.Utils
{
    public class APIHelper
    {
        private void button1_Click(object sender, EventArgs e)
        {
            string ss = HttpPost("http://14.17.22.55:8028/public-api/sodm-svc/stock/pushSupplyPart", "{Code:\"test089\",Name:\"test1\"}");
        }

        public static string HttpPost(string url, string body)
        {
            //ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
            Encoding encoding = Encoding.UTF8;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            //request.Accept = "text/html, application/xhtml+xml, */*";
            request.ContentType = "application/json";
            request.Host = "xxx.tencent.com";
            request.Headers.Add("Cache-Control", "no-cache");
            byte[] buffer = encoding.GetBytes(body);
            request.ContentLength = buffer.Length;
            request.GetRequestStream().Write(buffer, 0, buffer.Length);
            HttpWebResponse response = null;
            try
            {
                 response = (HttpWebResponse)request.GetResponse();
            }
            catch (Exception ex)
            {
                throw;
            }
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
