using System;
using System.Data;
using Goldnet.Ext.Web;
using GoldNet.Comm;
using GoldNet.Comm.DAL.Oracle;
using GoldNet.Model;
using System.Text;


namespace GoldNet.JXKP.mainpage
{
    public partial class main_zljk : System.Web.UI.Page
    {
        /// <summary>
        /// 初始化管理
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
                //日期选择范围
                //开始时间
                for (int i = 0; i < 10; i++)
                {
                    int years = System.DateTime.Now.Year - i;
                    this.Comb_StartYear.Items.Add(new Goldnet.Ext.Web.ListItem(years.ToString(), years.ToString()));
                }
                for (int i = 1; i <= 12; i++)
                {
                    this.Comb_StartMonth.Items.Add(new Goldnet.Ext.Web.ListItem(i.ToString(), i.ToString()));
                }
                this.Comb_StartMonth.SelectedIndex = 0;

                //结束时间
                for (int i = 0; i < 10; i++)
                {
                    int years = System.DateTime.Now.Year - i;
                    this.Comb_EndYear.Items.Add(new Goldnet.Ext.Web.ListItem(years.ToString(), years.ToString()));
                }
                for (int i = 1; i <= 12; i++)
                {
                    this.Comb_EndMonth.Items.Add(new Goldnet.Ext.Web.ListItem(i.ToString(), i.ToString()));
                }
                this.Comb_EndMonth.SelectedIndex = DateTime.Now.Month - 1;

                string dd1 = DateTimeConvert(DateTime.Now.Year.ToString(), "01");

                string dd2 = Convert.ToDateTime(DateTimeConvert(DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString())).AddMonths(1).ToString("yyyy-MM-dd");

                this.Store1.DataSource = GetStoreData(dd1, dd2, "0");
                this.Store1.DataBind();
            }
        }

        /// <summary>
        /// 质量监控数据
        /// </summary>
        private DataTable GetStoreData(string startime, string endtime, string flags)
        {
            string meunid = System.Configuration.ConfigurationManager.AppSettings["PageMeunid"].ToString();
            string modid = System.Configuration.ConfigurationManager.AppSettings["modid"].ToString();
            string staffid = Session["curpersonid"] == null ? ((User)Session["CURRENTSTAFF"]).UserId : Session["curpersonid"].ToString();
            string power = ((User)Session["CURRENTSTAFF"]).GetUserDeptFilterByStaff("", meunid, modid, staffid);
            User UserInstance = new User(staffid);
            string DeptCode = power == "" ? "" : UserInstance.AccountDeptCode;
            //取得传入的选择的会话状态
            //string deptcode = Session["curdeptcode"] == null ? DeptCode : Session["curdeptcode"].ToString();
            DataTable dt = GetQulityList(startime, endtime, flags, DeptCode).Tables[0];
            return dt;
        }

        //查询按钮触发事件
        protected void GetQueryPortalet(object sender, AjaxEventArgs e)
        {
            string dd1 = DateTimeConvert(this.Comb_StartYear.SelectedItem.Value, this.Comb_StartMonth.SelectedItem.Value);
            string dd2 = Convert.ToDateTime(DateTimeConvert(this.Comb_EndYear.SelectedItem.Value, this.Comb_EndMonth.SelectedItem.Value)).AddMonths(1).ToString("yyyy-MM-dd");
            string flags = this.pflg.SelectedItem.Value.ToString();
            if (flags.Equals(""))
            {
                flags = "0";
            }
            this.Store1.DataSource = GetStoreData(dd1, dd2, flags);
            this.Store1.DataBind();
        }

        /// <summary>
        /// 时间转化格式
        /// </summary>
        /// <param name="year">yyyy</param>
        /// <param name="month">m or mm</param>
        /// <param name="date">日期选项</param>
        /// <param name="i">2:开始日1:结束日</param>
        /// <returns>yyyyMMdd</returns>
        private string DateTimeConvert(string year, string month)
        {
            string l_month = month;
            if (Convert.ToInt32(month) < 10)
            {
                l_month = "0" + l_month.TrimStart(new char[] { '0' });
            }
            else if (Convert.ToInt32(l_month) > 12)
            {
                l_month = "12";
            }
            return year + "-" + l_month + "-" + "01";
        }

        /// <summary>
        /// 质量监控
        /// </summary>
        /// <returns></returns>
        public DataSet GetQulityList(string startime, string endtime, string flags, string deptcode)
        {
            StringBuilder str = new StringBuilder();

            deptcode = deptcode == "" ? "" : "AND DUTY_DEPT_ID IN (SELECT DEPT_CODE FROM COMM.SYS_DEPT_DICT WHERE ACCOUNT_DEPT_CODE = '" + deptcode + "') ";
            str.AppendFormat(@"SELECT *
                                  FROM {0}.QUALITY_ERROR_LIST 
                                 WHERE FLAGS = '{1}'
                                   AND IS_CHECKLOOK = '1'
                                   AND DATE_TIME >= '{2}'
                                   AND DATE_TIME < '{3}' {4}", DataUser.ZLGL, flags, startime, endtime, deptcode);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

    }
}
