using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web.Services;
using Goldnet.Dal;
using System.Text;
using GoldNet.Model;
using System.Web;

namespace GoldNet.JXKP.GuideLook
{
    public partial class GuideByTimeChart : System.Web.UI.Page
    {
        private static string incount = null;
        private static string offset = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            incount = ((User)Session["CURRENTSTAFF"]).StaffId;
            if (incount == "" || incount == null) incount = "NotUserid";
        }

        [WebMethod(EnableSession = true)]
        public static string GetDate(string terms)
        {
            string chartsTerms = HttpContext.Current.Server.UrlDecode(terms).ToString();

            string[] Terms = chartsTerms.Split('*');

            string ChartsType = Terms[0];

            string organ = Terms[1];

            string depttype = Terms[2];

            string deptattr = Terms[3];

            string years = Terms[4];

            string station = Terms[5];

            string where = Terms[6];

            string guidecode = Terms[7];

            offset = Terms[8];

            ChartsDalDict dal = new ChartsDalDict();

            string title = "";

            string Categories = "";

            string Series = "";

            DataTable l_dt = dal.FlashChartSql(guidecode, organ, depttype, deptattr, years, station, where, incount, offset, GuideLook.DeptPower,GuideByTime.FromDate,GuideByTime.ToDate).Tables[0];

            if (l_dt.Rows.Count > 0)
            {
                title = l_dt.Rows[0]["guide_name"].ToString();

                l_dt.Columns.Remove("guide_name");

                switch (ChartsType)
                {
                    case "pie":

                        Series = getPieChartsSeries(l_dt);
                        break;
                    case "bar":

                        Categories = getChartsCategories(l_dt);

                        Series = getColChartsSeries(l_dt);

                        break;
                }
            }
            
            string chartInfo = ChartsType + "$" + title + "$" + Categories + "$" + Series;

            return chartInfo;
        }

        private static string getColChartsSeries(DataTable dt)
        {
            StringBuilder Series = new StringBuilder();
            Series.Append("{name:'条型分析图',data: [");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Series.Append("{y:" + dt.Rows[i]["GUIDE_VALUE"].ToString() + ",");

                Series.Append("id:" + dt.Rows[i]["unit_code"].ToString() + "},");
            }
            Series.Remove(Series.Length - 1, 1).Append("]}");

            return Series.ToString();
        }

        private static string getPieChartsSeries(DataTable dt)
        {
            StringBuilder Series = new StringBuilder();

            Series.Append("{ type:'pie',name:'饼型分析图', data: [");

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Series.Append("{name:'");
                Series.Append(dt.Rows[i]["DEPT_NAME"].ToString());
                Series.Append("',y:");
                Series.Append(dt.Rows[i]["GUIDE_VALUE"].ToString());
                Series.Append(",id:");
                Series.Append(dt.Rows[i]["unit_code"].ToString());
                Series.Append("},");
            }
            Series.Remove(Series.Length - 1, 1).Append("]}");
            return Series.ToString();
        }

        private static string getChartsCategories(DataTable dt)
        {
            StringBuilder Categories = new StringBuilder();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Categories.Append("'" + dt.Rows[i]["DEPT_NAME"].ToString() + "',");
            }
            return Categories.ToString().TrimEnd(new char[] { ',' });
        }
    }
}
