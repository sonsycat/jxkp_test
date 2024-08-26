using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web.Services;
using Goldnet.Dal;
using System.Text;

namespace GoldNet.JXKP.GuideLook
{
    public partial class GuideByDeptChart : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod(EnableSession = true)]
        public static string GetDate(string terms)
        {
            string[] temp = terms.Split(';');

            ChartsDalDict dal = new ChartsDalDict();

            string title = "";
            string Categories = "";
            string Series = "";

            DataTable l_dt = dal.getGuideByDeptChart(temp[0], temp[1],"", temp[2]).Tables[0];

            if (l_dt.Rows.Count > 0)
            {
                title = l_dt.Rows[0]["NAME"].ToString();

                Categories = getChartsCategories(temp[2], 0);

                Series = getLineChartsSeries(l_dt, Categories);
            }

            string chartInfo = "line" + "$" + title + "$" + Categories + "$" + Series;

            return chartInfo;
        }


        private static string getChartsCategories(string year, int instance)
        {

            string Categories = "";

            DateTime TimeforGuide = Convert.ToDateTime(year + "-01-01").AddMonths(instance);

            for (int i = 0; i < 12; i++)
            {
                Categories = Categories + "'" + TimeforGuide.AddMonths(i).ToString("yyyyMM") + "',";
            }

            return Categories.TrimEnd(new char[] { ',' });
        }

        private static string getLineChartsSeries(DataTable dt, string Categories)
        {

            Hashtable Name = new Hashtable();

            StringBuilder ChartData = new StringBuilder();

            string[] years = Categories.Split(',');

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string ChartsItemsName = dt.Rows[i]["GUIDE_NAME"].ToString();

                if (!Name.ContainsValue(ChartsItemsName))
                {
                    Name.Add(Name.Count, dt.Rows[i]["GUIDE_NAME"].ToString());
                }
            }

            for (int j = 0; j < Name.Count; j++)
            {
                DataRow[] drCount = dt.Select("GUIDE_NAME='" + Name[j] + "'");

                ChartData.Append("{ name:'" + Name[j] + "' , data: [");

                for (int x = 0; x < years.Length; x++)
                {
                    string filter = "GUIDE_NAME='" + Name[j] + "' and TJYF=" + years[x] + "";

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
                        ChartData.Append("{y:" + value + ",id:'" + dr[0]["GUIDE_CODE"] + "'},");
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
    }
}
