using System;
using System.Collections;
using System.Data;
using System.Web.Services;
using Goldnet.Ext.Web;
using System.Web;

namespace GoldNet.JXKP.GuideLook.WebService
{
    /// <summary>
    /// $codebehindclassname$ 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class DeptInfo : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/json";

            var start = 0;
            var limit = 1000;
            var sort = string.Empty;
            var dir = string.Empty;
            var query = string.Empty;
            var deptfilter = string.Empty;

            if (!string.IsNullOrEmpty(context.Request["deptfilter"]))
            {
                deptfilter = context.Request["deptfilter"].ToString();
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

            Paging<DeptInfos> Staffdepts = DeptInfos.PlantsPaging(start, limit, sort, dir, query, deptfilter);

            context.Response.Write(string.Format("{{totalCount:{1},'Staffdepts':{0}}}", JSON.Serialize(Staffdepts.Data), Staffdepts.TotalRecords));
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
