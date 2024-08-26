using System;
using System.Collections;
using System.Data;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using Goldnet.Ext.Web;

namespace GoldNet.JXKP.cbhs.WebService
{
    /// <summary>
    /// $codebehindclassname$ 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class HisUsers : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/json";

            var start = 0;
            var limit = 1000;
            var sort = string.Empty;
            var dir = string.Empty;
            var query = string.Empty;
            var itemfilter = string.Empty;
            if (!string.IsNullOrEmpty(context.Request["itemfilter"]))
            {
                itemfilter = context.Request["itemfilter"].ToString();
            }

            if (!string.IsNullOrEmpty(context.Request["start"]))
            {
                start = int.Parse(context.Request["start"]);
            }

            if (!string.IsNullOrEmpty(context.Request["limit"]))
            {
                limit = int.Parse(context.Request["limit"]);
            }

            if (!string.IsNullOrEmpty(context.Request["sort"]))
            {
                sort = context.Request["sort"];
            }

            if (!string.IsNullOrEmpty(context.Request["dir"]))
            {
                dir = context.Request["dir"];
            }

            if (!string.IsNullOrEmpty(context.Request["query"]))
            {
                query = context.Request["query"];
            }

            Paging<HisUser> hisuserlist = HisUser.PlantsPaging(start, limit, sort, dir, query, itemfilter);

            context.Response.Write(string.Format("{{TOTALCOUNT:{1},'STAFFINFO':{0}}}", JSON.Serialize(hisuserlist.Data), hisuserlist.TotalRecords));
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
