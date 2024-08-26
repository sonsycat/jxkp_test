using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.IO;

namespace Goldnet.Comm
{
    public class Msg106
    {
        public string Site { get; set; } //短信接口网址
        public string Msg { get; set; }  //用于函数返回的额外信息
        public static string Userid { get; set; }
        public static string Account { get; set; }
        public static string Password { get; set; }
        public static string Validate { get; set; } //验证码模板

        public Msg106()
        {
            Site = "http://106.yhg.cc/sms.aspx";
            Userid = "3546"; //userid
            Account = "test"; //用户名
            Password = "test"; //密码
            Validate = ""; //验证码模板
        }

        public virtual bool SendMsg(string mobile, string content)
        {
            return SendMsg(Userid, Account, Password, mobile, content, "", "post");
        }

        //定时发送
        public virtual bool SendMsg(string mobile, string content, string time)
        {
            return SendMsg(Userid, Account, Password, mobile, content, time, "post");
        }

        public virtual bool SendMsg(string userid, string account, string password, string mobile, string content, string sendTime, string postType)
        {
            var b = false;
            var site = Site;
            var param = string.Format("action=send&userid={0}&account={1}&password={2}&mobile={3}&content={4}&sendTime={5}", userid, account, password, mobile, content, sendTime);
            string url = "";
            string xml = "";

            if (postType == "get")
            {
                url = site + "?" + param;
                xml = GetUrl(url);
                //下面判断返回值
            }
            else //post方式发送数据
            {
                xml = HttpPost(site, param);
            }
            b = IsSuccess(xml);
            Msg = GetReturnMsg(xml);
            return b;
        }

        //打开一个特定的网站，返回结果
        public virtual string GetUrl(string url)
        {
            WebClient MyWebClient = new WebClient();
            MyWebClient.Credentials = CredentialCache.DefaultCredentials;//获取或设置用于向Internet资源的请求进行身份验证的网络凭据
            Byte[] pageData = MyWebClient.DownloadData(url); //从指定网站下载数据

            //string pageHtml = Encoding.Default.GetString(pageData);  //如果获取网站页面采用的是GB2312，则使用这句            

            string pageHtml = Encoding.UTF8.GetString(pageData); //如果获取网站页面采用的是UTF-8，则使用这句
            return pageHtml;
        }

        protected virtual bool IsSuccess(string xml)
        {
            var b = false;
            int begin = xml.IndexOf("<returnstatus>");
            int end = xml.IndexOf("</returnstatus>");
            if (begin > 0)
            {
                var s = xml.Substring(begin, end - begin);
                if (s.ToLower().IndexOf("success") > 0 || s.ToLower().IndexOf("ok") > 0)
                {
                    b = true;
                }
            }
            return b;
        }

        /// <summary>
        /// 得到发送短信后的返回信息
        /// </summary>
        /// <param name="xml">返回的xml主体内容</param>
        /// <returns>解析xml，取得返回信息</returns>
        protected string GetReturnMsg(string xml)
        {
            string s = null;
            int begin = xml.IndexOf("<message>");
            int end = xml.IndexOf("</message>");
            if (begin > 0)
            {
                s = xml.Substring(begin + 10, end - begin - 10);
            }
            return s;
        }

        /// <summary>
        /// HTTP POST方式请求数据
        /// </summary>
        /// <param name="url">URL.</param>
        /// <param name="param">POST的数据</param>
        /// <returns></returns>
        public string HttpPost(string url, string param)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Accept = "*/*";
            request.Timeout = 15000;
            request.AllowAutoRedirect = false;

            StreamWriter requestStream = null;
            WebResponse response = null;
            string responseStr = null;

            try
            {
                requestStream = new StreamWriter(request.GetRequestStream());
                requestStream.Write(param);
                requestStream.Close();

                response = request.GetResponse();
                if (response != null)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                    responseStr = reader.ReadToEnd();
                    reader.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                request = null;
                requestStream = null;
                response = null;
            }

            return responseStr;
        }
    }
}
