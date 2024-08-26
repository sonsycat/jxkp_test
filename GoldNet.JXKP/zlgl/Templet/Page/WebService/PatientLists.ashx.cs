using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using Goldnet.Ext.Web;

namespace GoldNet.JXKP.zlgl.Templet.Page.WebService
{
    /// <summary>
    /// $codebehindclassname$ 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class PatientLists : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/json";

            var start = 0;
            var limit = 10;
            var sort = string.Empty;
            var dir = string.Empty;
            var query = string.Empty;
            var patientid = string.Empty;
            var tablename = string.Empty;
            var fieldid = string.Empty;
            if (!string.IsNullOrEmpty(context.Request["fieldid"]))
            {
                fieldid = context.Request["fieldid"].ToString();
            }
            if (!string.IsNullOrEmpty(context.Request["tablename"]))
            {
                tablename = context.Request["tablename"].ToString();
            }
            if (!string.IsNullOrEmpty(context.Request["patientid"]))
            {
                patientid = context.Request["patientid"].ToString();
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

            Paging<PatientList> list = PatientList.PlantsPaging(start, limit, sort, dir, query, patientid,tablename,fieldid);

            context.Response.Write(string.Format("{{totalCount:{1},'list':{0}}}", JSON.Serialize(list.Data), list.TotalRecords));
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
