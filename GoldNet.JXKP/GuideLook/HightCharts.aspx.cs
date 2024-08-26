using System;
using System.Collections;
using System.Data;
using System.Web;
using System.Web.Services;
using Goldnet.Dal;
using System.Text;
using GoldNet.Model;

namespace GoldNet.JXKP.GuideLook
{
    public partial class HightCharts : System.Web.UI.Page
    {
        private static string incount = null;
        private static string offset = null;
        public static string seriesData = "111111111111";

        /// <summary>
        /// 初始化处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            incount = ((User)Session["CURRENTSTAFF"]).StaffId;
            if (incount == "" || incount == null) incount = "NotUserid";
        }

        /// <summary>
        /// 获取图表数据
        /// </summary>
        /// <param name="terms"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public static string GetDate(string terms)
        {
            string chartsTerms = HttpContext.Current.Server.UrlDecode(terms).ToString();

            string[] Terms = chartsTerms.Split('*');
            string ChartsType = Terms[0];
            string organ = Terms[1];
            string depttype = Terms[2];
            string deptattr = Terms[3];
            //查询时间范围
            string years = Terms[4];
            string station = Terms[5];
            string where = Terms[6];
            string guidecode = Terms[7];
            offset = Terms[8];

            ChartsDalDict dal = new ChartsDalDict();
            string title = "";
            string Categories = "";
            string Series = "";

            if (ChartsType.Equals("line"))
            {
                //获取趋势图数据
                DataTable l_dt = dal.GetChartsLine(years, organ, guidecode, deptattr, station, where, depttype, offset, GuideLook.DeptPower, incount).Tables[0];
                if (l_dt.Rows.Count > 0)
                {
                    title = l_dt.Rows[0]["guide_name"].ToString();
                    l_dt.Columns.Remove("guide_name");
                    Categories = getChartsCategories(years, 0);
                    Series = getLineChartsSeries(l_dt, Categories);
                    HightCharts.seriesData = Series;
                }
            }
            else if (ChartsType.Equals("pie"))
            {
                //获取饼图数据
                DataTable l_dt = dal.FlashChartSql(guidecode, organ, depttype, deptattr, years, station, where, incount, offset, GuideLook.DeptPower).Tables[0];
                if (l_dt.Rows.Count > 0)
                {
                    title = l_dt.Rows[0]["guide_name"].ToString();
                    l_dt.Columns.Remove("guide_name");
                    Series = getPieChartsSeries(l_dt);
                }
            }
            else
            {
                //获取条图数据
                DataTable l_dt = dal.FlashChartSqlBar(guidecode, organ, depttype, deptattr, years, station, where, incount, offset, GuideLook.DeptPower).Tables[0];
                if (l_dt.Rows.Count > 0)
                {
                    title = l_dt.Rows[0]["guide_name"].ToString();
                    l_dt.Columns.Remove("guide_name");

                    Categories = getChartsCategories(l_dt);
                    Series = getColChartsSeries(l_dt);
                }
            }
            string chartInfo = ChartsType + "$" + title + "$" + Categories + "$" + Series;
            return chartInfo;
        }

        /// <summary>
        /// 处理日期字符串
        /// </summary>
        /// <param name="year"></param>
        /// <param name="instance"></param>
        /// <returns></returns>
        private static string getChartsCategories(string year, int instance)
        {
            string[] years = year.Split('-');

            string Categories = "";
            DateTime TimeforGuide = Convert.ToDateTime(years[0] + "-" + years[1] + "-01").AddMonths(instance);
            DateTime TimeforGuideto = Convert.ToDateTime(years[2] + "-" + years[3] + "-01").AddMonths(instance);

            Categories = TimeforGuide.ToString("yyyyMM") + ",";
            while (TimeforGuide < TimeforGuideto)
            {
                TimeforGuide = TimeforGuide.AddMonths(1);
                Categories = Categories + "" + TimeforGuide.ToString("yyyyMM") + ",";
            }
            //for (int i = 0; i < 12; i++)
            //{
            //    Categories = Categories + "" + TimeforGuide.AddMonths(i).ToString("yyyyMM") + ",";
            //}
            return Categories.TrimEnd(new char[] { ',' });
        }

        /// <summary>
        /// 构成趋势图数据串
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="Categories"></param>
        /// <returns></returns>
        private static string getLineChartsSeries(DataTable dt, string Categories)
        {
            Hashtable Name = new Hashtable();
            StringBuilder ChartData = new StringBuilder();

            string[] years = Categories.Split(',');

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string ChartsItemsName = dt.Rows[i]["name"].ToString();

                if (!Name.ContainsValue(ChartsItemsName))
                {
                    Name.Add(Name.Count, dt.Rows[i]["name"].ToString());
                }
            }

            for (int j = 0; j < Name.Count; j++)
            {
                DataRow[] drCount = dt.Select("NAME='" + Name[j] + "'");
                ChartData.Append("{ name:'" + Name[j] + "',data:[");
                for (int x = 0; x < years.Length; x++)
                {
                    string filter = "NAME='" + Name[j] + "' and TJYF=" + years[x] + "";
                    DataRow[] dr = dt.Select(filter);
                    if (dr.Length != 0)
                    {
                        string value = "";
                        if (dr[0]["GUIDE_VALUE"] == null || dr[0]["GUIDE_VALUE"].ToString().Equals(""))
                        {
                            value = "0";
                        }
                        else
                        {
                            value = dr[0]["GUIDE_VALUE"].ToString();
                        }
                        ChartData.Append("{y:" + value + ",id:'" + dr[0]["unit_code"] + "'},");
                    }
                    else
                    {
                        if (years[x].CompareTo(drCount[drCount.Length - 1]["TJYF"].ToString()) > 0)
                        {
                            break;
                        }
                        else
                        {
                            ChartData.Append("0,");
                        }
                    }
                }
                ChartData.Remove(ChartData.Length - 1, 1).Append("]}*");
            }
            return ChartData.ToString().TrimEnd(new char[] { '*' });
        }

        /// <summary>
        /// 构成趋势图标题数据串
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private static string getChartsCategories(DataTable dt)
        {
            StringBuilder Categories = new StringBuilder();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Categories.Append("'" + dt.Rows[i]["DEPT_NAME"].ToString() + "',");
            }
            return Categories.ToString().TrimEnd(new char[] { ',' });
        }

        /// <summary>
        /// 构成条形图数据串
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private static string getColChartsSeries(DataTable dt)
        {
            StringBuilder Series = new StringBuilder();
            StringBuilder TQSeries = new StringBuilder();

            Series.Append("{name:'本期',data: [");
            TQSeries.Append("{name:'同期',data: [");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Series.Append("{y:" + dt.Rows[i]["GUIDE_VALUE"].ToString() + ",");
                Series.Append("id:" + dt.Rows[i]["unit_code"].ToString() + "},");

                TQSeries.Append("{y:" + dt.Rows[i]["TQ_GUIDE_VALUE"].ToString() + ",");
                TQSeries.Append("id:" + dt.Rows[i]["unit_code"].ToString() + "},");
            }
            Series.Remove(Series.Length - 1, 1).Append("]}");
            TQSeries.Remove(TQSeries.Length - 1, 1).Append("]}");

            return Series.ToString() + "," + TQSeries.ToString();
        }

        /// <summary>
        /// 构成饼图数据串
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 获取弹出明细窗口使用的数据
        /// </summary>
        /// <param name="terms"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public static string GetPopUpDate(string terms)
        {
            string PopUpChartsTerms = terms;
            string[] Info = PopUpChartsTerms.Split('*');
            string unit_code = Info[0];
            string organ = Info[1];
            string ChartsType = Info[2];
            //查询时间
            string year = Info[3];
            string guideCode = Info[4];
            ChartsDalDict dal = new ChartsDalDict();
            string title = "";
            string Categories = "";
            string Series = "";
            string PopUpGuideCode = "";

            if (ChartsType.Equals("line"))
            {
                //趋势图
                DataTable l_dt = dal.GetPopUpChartsLine(year, organ, guideCode, unit_code, offset, GuideLook.DeptPower, incount).Tables[0];
                if (l_dt.Rows.Count > 0)
                {
                    title = l_dt.Rows[0]["guide_name"].ToString();
                    l_dt.Columns.Remove("guide_name");
                    if (year.Length > 4)
                    {
                        year = year.Substring(0, 4);
                    }
                    Categories = getChartsCategories(year, 0);
                    Series = getLineChartsSeries(l_dt, Categories);
                    PopUpGuideCode = l_dt.Rows[0]["GUIDE_CODE"].ToString();
                }
            }
            else
            {
                //饼图和条形图
                DataTable l_dt = dal.GetPopUpChartsPieAndBar(year, organ, guideCode, unit_code, incount, offset, GuideLook.DeptPower).Tables[0];

                if (l_dt.Rows.Count > 0)
                {
                    title = l_dt.Rows[0]["guide_name"].ToString();
                    l_dt.Columns.Remove("guide_name");
                    switch (ChartsType)
                    {
                        case "pie":
                            Series = getPieChartsSeries(l_dt);
                            PopUpGuideCode = l_dt.Rows[0]["GUIDE_CODE"].ToString();

                            break;

                        case "bar":
                            Categories = getChartsCategories(l_dt);
                            Series = getColChartsSeries(l_dt);
                            PopUpGuideCode = l_dt.Rows[0]["GUIDE_CODE"].ToString();

                            break;
                    }
                }
            }
            string chartInfo = ChartsType + "$" + title + "$" + Categories + "$" + Series + "$" + PopUpGuideCode;
            return chartInfo;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="terms"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public static string GetPopUpDetailDate(string terms)
        {
            return "bar$$$";
        }
    }
}
