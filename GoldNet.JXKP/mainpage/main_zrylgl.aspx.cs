using System;
using System.Data;
using Goldnet.Ext.Web;
using GoldNet.Comm;
using GoldNet.Comm.DAL.Oracle;
using System.Text;
using GoldNet.Model;


namespace GoldNet.JXKP.mainpage
{
    public partial class main_zrylgl : PageBase
    {
        /// <summary>
        /// 初始化处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //检查是否已经登录，否则停止
            if (Session["CURRENTSTAFF"] == null)
            {
                Response.End();
            }

            if (!Ext.IsAjaxRequest)
            {
                this.Store1.DataSource = GetStoreData("1");
                this.Store1.DataBind();
                for (int i = 1; i <= 10; i++)
                {
                    this.Combo_daydate.Items.Add(new Goldnet.Ext.Web.ListItem(i.ToString(), i.ToString()));
                }
                this.Combo_daydate.Value = "1";
            }

        }

        /// <summary>
        /// 昨日医疗数据
        /// </summary>
        private DataTable GetStoreData(string days)
        {
            string DeptCode = ((User)Session["CURRENTSTAFF"]).AccountDeptCode;

            //取得传入的选择的会话状态
            string deptcode = Session["curdeptcode"] == null ? DeptCode : Session["curdeptcode"].ToString();
            //string stationcode = Session["curstationcode"] == null ? "" : Session["curstationcode"].ToString();
            //string personid = Session["curpersonid"] == null ? "" : Session["curpersonid"].ToString();
            string yearstr = Session["curdateyear"] == null ? DateTime.Now.ToString("yyyy") : Session["curdateyear"].ToString();
            //string curguide = System.Configuration.ConfigurationManager.AppSettings["curguide"].ToString();

            //DataTable dt = GetYesterdayinfo(deptcode, days).Tables[0];
            User user = (User)Session["CURRENTSTAFF"];
            string deptcodes = user.GetUserDeptFilter("", "080001", "8");
            DataTable dt = GetYesterdayinfo(deptcodes, days).Tables[0];
            return dt;
        }

        /// <summary>
        /// 查询按钮触发事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GetQueryPortalet(object sender, AjaxEventArgs e)
        {
            string days = Combo_daydate.SelectedItem.Value.ToString();
            if (days == null | days.Equals(""))
            {
                days = "1";
            }
            this.Store1.DataSource = GetStoreData(days);
            this.Store1.DataBind();

        }

        /// <summary>
        /// 昨日医疗
        /// </summary>
        /// <returns></returns>
        public DataSet GetYesterdayinfo(string deptcode, string daydate)
        {

            int day = Convert.ToInt32(daydate) * -1;

            DateTime dt = DateTime.Now;

            string offset = System.Configuration.ConfigurationManager.AppSettings["dateoffset"].ToString();
            string start = "ADD_MONTHS(TO_DATE('" + new DateTime(dt.Year, dt.Month, 1).ToString("yyyyMMdd") + "','YYYYMMDD')," + offset + ")";     //月初日期 
            string end = "ADD_MONTHS(TO_DATE('" + new DateTime(dt.Year, dt.Month, 1).AddMonths(1).AddDays(day).ToString("yyyyMMdd") + "','YYYYMMDD')," + offset + ")";   //月底日期

            string startDay = "ADD_MONTHS(TO_DATE('" + dt.AddDays(day).ToString("yyyyMMdd") + "','YYYYMMDD')," + offset + ")";
            string endDay = "ADD_MONTHS(TO_DATE('" + dt.AddDays(-1).ToString("yyyyMMdd") + "','YYYYMMDD')," + offset + ")";
            deptcode = deptcode == "" ? "" : "AND DEPT_CODE IN (" + deptcode + ")";


            StringBuilder str = new StringBuilder();

            str.AppendFormat(@"SELECT   A.*, NULL RJGZL
    FROM (SELECT '门诊人数' ZBMC, SUM (nvl(MZRC,0)) SZ,
                 SUM (CASE WHEN ST_DATE >= {4} AND ST_DATE <= {5} THEN MZRC ELSE 0 END) MONTHS,10 SORTNUM
                 FROM {0}.DEPT_DAY_REPORT
           WHERE ST_DATE >= {1} AND ST_DATE <= {2} {3}
          
          UNION ALL
          SELECT '入院人数' ZBMC, SUM (nvl(RYRS,0)) SZ,
              SUM (CASE WHEN ST_DATE >= {4} AND ST_DATE <= {5} THEN RYRS ELSE 0 END) MONTHS,
                 40 SORTNUM
            FROM {0}.DEPT_DAY_REPORT
           WHERE ST_DATE >= {1} AND ST_DATE <= {2} {3}
          UNION ALL
          SELECT '出院人数' ZBMC, SUM (nvl(CYRS,0)) SZ,
              SUM (CASE WHEN ST_DATE >= {4} AND ST_DATE <= {5} THEN CYRS ELSE 0 END) MONTHS,
                 50 SORTNUM
            FROM {0}.DEPT_DAY_REPORT
           WHERE ST_DATE >= {1} AND ST_DATE <= {2} {3}
          UNION ALL
          SELECT '手术例数' ZBMC, SUM (nvl(SSTC,0)) SZ,
              SUM (CASE WHEN ST_DATE >= {4} AND ST_DATE <= {5} THEN SSTC ELSE 0 END) MONTHS,
                 60 SORTNUM
            FROM {0}.DEPT_DAY_REPORT
           WHERE ST_DATE >= {1} AND ST_DATE <= {2} {3}
          UNION ALL
          SELECT '检验项目数' ZBMC, SUM (nvl(JYXMS,0)) SZ,
              SUM (CASE WHEN ST_DATE >= {4} AND ST_DATE <= {5} THEN JYXMS ELSE 0 END) MONTHS,
                 70 SORTNUM
            FROM {0}.DEPT_DAY_REPORT
           WHERE ST_DATE >= {1} AND ST_DATE <= {2} {3}
          
          ) A
          ORDER BY SORTNUM", DataUser.HISFACT, start, end, deptcode, startDay, endDay);

          //  UNION ALL
          //SELECT '急诊人数' ZBMC, SUM (nvl(JZRS,0)) SZ,
          //  SUM (CASE WHEN ST_DATE >= {4} AND ST_DATE <= {5} THEN JZRS ELSE 0 END) MONTHS,
          //       30 SORTNUM
          //  FROM {0}.DEPT_DAY_REPORT
          // WHERE ST_DATE >= {1} AND ST_DATE <= {2} {3}

            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }
    }
}
