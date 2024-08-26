using System;
using System.IO;
using System.Web.UI;
using System.Web;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Web.Compilation;
using System.Reflection;
using System.Collections.Specialized;
using System.Web.UI.WebControls;

namespace GoldNet.Comm
{
    public class FilterStrFactoryHandler : IHttpHandler 
    {
        public void ProcessRequest(HttpContext context)
        {           
            //if (context.Request.RequestType == "POST")
            //{
            //    Page page = context.CurrentHandler as Page;
            //    NameValueCollection postData = page.Request.Form;
            //    foreach (string postKey in postData)
            //    {
            //        Control ctl = page.FindControl(postKey);
            //        if (ctl as TextBox != null)
            //        {
            //            ((TextBox)ctl).Text = DataBase.InputText(((TextBox)ctl).Text);
            //            continue;
            //        } 
            //    }
            //} 
        }

        /// <summary>
        /// 获取一个值，该值指示其他请求是否可以使用 IHttpHandler 实例。
        /// </summary>
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
    public static class DataBase
    {
        public static string InputText(string text)
        {
            text = text.Trim();
            if (string.IsNullOrEmpty(text))
                return string.Empty;
            text = Regex.Replace(text, "[\\s]{2,}", " "); //two or more spaces
            text = Regex.Replace(text, "(<[b|B][r|R]/*>)+|(<[p|P](.|\\n)*?>)", "\n"); //<br>
            text = Regex.Replace(text, "(\\s*&[n|N][b|B][s|S][p|P];\\s*)+", " "); // 
            text = Regex.Replace(text, "<(.|\\n)*?>", string.Empty); //any other tags
            text = text.Replace("'", "''");
            return text;
        }
    }

}
