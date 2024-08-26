using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web.Services;
using Goldnet.Dal;
using System.Text;
using System.Web;

namespace GoldNet.JXKP.RLZY.BaseInfoMaintain
{
    public partial class GraphHightChart : System.Web.UI.Page
    {
        
        [WebMethod(EnableSession = true)]
        public static string GetDate(string terms)
        {
            string chartsTerms = HttpContext.Current.Server.UrlDecode(terms).ToString();
            string type = terms.Split('*')[0].ToString();
            string deptCode = terms.Split('*')[1].ToString();
            BaseInfoMaintainDal dal = new BaseInfoMaintainDal();
            DataTable l_dt = new DataTable();
            string Categories = "";
            string Series = "";
            string title = "";
            switch (type) 
            {
                case "1":
                    l_dt = dal.getGraphJobMediStat(Graph.DeptPower, Graph.StaffSort).Tables[0];
                    if (l_dt.Rows.Count > 0) 
                    {
                        //Categories = getChartsCategories(l_dt);
                        //Series = getColChartsSeries(l_dt);
                        Series = getPieChartsSeries(l_dt, "医师分布图");
                        title = "医师分布图";
                    }
     
                    break;
                case "2":
                    l_dt = dal.getGraphEduStat(deptCode, Graph.DeptPower, Graph.StaffSort).Tables[0];
                    if (l_dt.Rows.Count > 0)
                    {
                        Series = getPieChartsSeries(l_dt, "学历层次分布图");
                        title = "学历层次分布图";
                    }
                    break;
                case "3":
                    l_dt = dal.getGraphAgeStat(deptCode, Graph.DeptPower, Graph.StaffSort).Tables[0];
                    if (l_dt.Rows[0][0].ToString() != "") 
                    {
                        Series = getPieChartsSeries(l_dt, "年龄结构分布图", CreateTableForPie());
                        title = "年龄结构分布图";
                        
                    }
                    type = "2";
                    break;
                case "4":
                    l_dt = dal.getGraphJobStat(deptCode, Graph.DeptPower, Graph.StaffSort).Tables[0];
                    if (l_dt.Rows.Count > 0)
                    {
                        Series = getPieChartsSeries(l_dt, "职称结构分布图");
                        title = "职称结构分布图";
                        
                    }
                    type = "2";
                    break;
            }
            string chartInfo = type + "*" + Categories + "*" + Series + "*" + title;

            return chartInfo;
        }


        private static string getChartsCategories(DataTable dt)
        {
            StringBuilder Categories = new StringBuilder();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Categories.Append("'" + dt.Rows[i]["UNIT_NAME"].ToString() + "',");
            }

            return Categories.ToString().TrimEnd(new char[] { ',' });
        }

        private static string getColChartsSeries(DataTable dt)
        {
            StringBuilder Series = new StringBuilder();
            Series.Append("{name:'医师分布图',data: [");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Series.Append("" + dt.Rows[i]["VALUE"].ToString() + ",");
            }
            Series.Remove(Series.Length - 1, 1).Append("]}");
            return Series.ToString();
        }

        private static string getPieChartsSeries(DataTable dt,string name)
        {
            StringBuilder Series = new StringBuilder();

            Series.Append("{ type:'pie',name:'" + name + "', data: [");

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Series.Append("{name:'");
                Series.Append(dt.Rows[i]["UNIT_NAME"].ToString());
                Series.Append("',y:");
                Series.Append(dt.Rows[i]["VALUE"].ToString());
                Series.Append("},");
            }
            Series.Remove(Series.Length - 1, 1).Append("]}");
            return Series.ToString();
        }

        private static string getPieChartsSeries(DataTable dt,string name,DataTable l_dt)
        {
            StringBuilder Series = new StringBuilder();

            Series.Append("{ type:'pie',name:'" + name + "', data: [");

            for (int i = 0; i < dt.Columns.Count; i++)
            {
                Series.Append("{name:'");
                Series.Append(l_dt.Rows[0][i].ToString());
                Series.Append("',y:");
                Series.Append(dt.Rows[0][i].ToString());
                Series.Append("},");
            }
            Series.Remove(Series.Length - 1, 1).Append("]}");
            return Series.ToString();
        }

        /// <summary>
        /// 创建集合
        /// </summary>
        private static DataTable CreateTableForPie()
        {
            DataTable l_dt = new DataTable();
            l_dt.Columns.Add("UNIT_NAME");
            l_dt.Columns.Add("UNIT_NAME1");
            l_dt.Columns.Add("UNIT_NAME2");
            l_dt.Columns.Add("UNIT_NAME3");
            l_dt.Columns.Add("UNIT_NAME4");
            l_dt.Columns.Add("UNIT_NAME5");

            DataRow l_dr = l_dt.NewRow();

            l_dr[0] = "30岁以下";
            l_dr[1] = "31-40";
            l_dr[2] = "41-50";
            l_dr[3] = "51-55";
            l_dr[4] = "56-60";
            l_dr[5] = "60岁以上";

            l_dt.Rows.Add(l_dr);

            return l_dt;
        }
    }
}
