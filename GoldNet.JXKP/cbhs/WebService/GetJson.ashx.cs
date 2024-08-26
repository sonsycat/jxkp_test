using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using Goldnet.Dal;
using Goldnet.Ext.Web;

namespace GoldNet.JXKP.cbhs.WebService
{
    /// <summary>
    /// $codebehindclassname$ 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class GetJson : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string datetime = context.Request.QueryString["datetime"];
            FlowChart dal = new FlowChart();
            //执行过程检查完成状态
            string rtmsg = dal.Exec_Sp_Flow_Chart(datetime);
            //差选表获取json串
            string strJson = dal.GetJsonString(datetime);
            strJson = strJson.Replace("'", "\"");
            if (strJson == "")
            {
                strJson = "[]";
            }
            // context.Response.ContentType = "text/json"; 
            context.Response.Clear();
            context.Response.Write(strJson);
            context.Response.End();
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
