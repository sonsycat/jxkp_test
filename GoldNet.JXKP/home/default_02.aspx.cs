using System;
using System.Data;
using Goldnet.Ext.Web;
using GoldNet.Comm;
using GoldNet.Model;
using Goldnet.Dal;
using GoldNet.JXKP;

namespace Goldnet.JXKP.home
{
    public partial class _default_02 : PageBase
    {
        private static int dateoff = Convert.ToInt32(GetConfig.GetConfigString("dateoffset"));
        /// <summary>
        /// 初始化处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //检查是否已经登录，否则退出
            if (Session["CURRENTSTAFF"] == null)
            {
                Response.Redirect("/auth/login/login.aspx");
                Response.End();
            }

            if (!Ext.Web.Ext.IsAjaxRequest)
            {
                //页面各控件准备
                if (!ComponentInit())
                {
                    this.Portlet_KPI_mz.AutoLoad.Url = "";
                    this.Portlet_KPI_zy.AutoLoad.Url = "";
                    this.Portlet_KPI_ss.AutoLoad.Url = "";
                    this.Portlet_KPI_yzb.AutoLoad.Url = "";

                    this.Portlet_jjhs.AutoLoad.Url = "";
                    this.Portlet_gzl.AutoLoad.Url = "";
                    this.Portlet_zlxl.AutoLoad.Url = "";
               }
            }
        }

        /// <summary>
        /// 页面各控件机能初始化
        /// </summary>
        /// <returns></returns>
        protected bool ComponentInit()
        {
            string year = DateTime.Now.AddMonths(0).Year.ToString();
            string month = DateTime.Now.AddMonths(0).Month.ToString();

            BoundComm boundcomm = new BoundComm();
            //DateTime dtime = new DateTime();
            //HomeDal dal = new HomeDal();

            //数据详情portlet 隐藏
            //this.Portlet_Detail.Hide();

            //年度下拉框内容
            SYear.DataSource = boundcomm.getYears();
            SYear.DataBind();
            this.Combo_Year.Value = year;

            //月份下拉框内容
            SMonth.DataSource = boundcomm.getMonth();
            SMonth.DataBind();
            this.cbbmonth.Value = month;

            //this.Combo_Year.Value = System.DateTime.Now.Year.ToString();

            User user = ((User)Session["CURRENTSTAFF"]);
            string DeptType = ((User)Session["CURRENTSTAFF"]).DeptType;
            string DeptCode = ((User)Session["CURRENTSTAFF"]).AccountDeptCode;
            string StaffId = ((User)Session["CURRENTSTAFF"]).StaffId;
            string userId = ((User)Session["CURRENTSTAFF"]).UserId;
            string StationCode = ((User)Session["CURRENTSTAFF"]).GetStationCode(StaffId, DateTime.Now.Year.ToString());

            //获取人员菜单权限
            DataTable powertable = ((User)Session["CURRENTSTAFF"]).GetUserPower;
            Session["menu"] = powertable;

            this.PanelTop.AutoLoad.Url = "/home/header.aspx?" + userId;

            //string meunid = System.Configuration.ConfigurationManager.AppSettings["PageMeunid"].ToString();
            //string modid = System.Configuration.ConfigurationManager.AppSettings["modid"].ToString();

            return true;
        }

        //页面刷新，保持Session会话状态
        protected void RefreshTime(object sender, AjaxEventArgs e)
        {
            return;
        }

        /// <summary>
        /// 查询按钮触发事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GetQueryPortalet(object sender, AjaxEventArgs e)
        {
            User user = ((User)Session["CURRENTSTAFF"]);
            string DeptType = ((User)Session["CURRENTSTAFF"]).DeptType;
            string DeptCode = ((User)Session["CURRENTSTAFF"]).AccountDeptCode;
            string StaffId = ((User)Session["CURRENTSTAFF"]).StaffId;
            string userId = ((User)Session["CURRENTSTAFF"]).UserId;
            string StationCode = ((User)Session["CURRENTSTAFF"]).GetStationCode(StaffId, DateTime.Now.Year.ToString());

            Session["curpersonid"] = userId;
            Session["curstationcode"] = StationCode;
            Session["curdeptcode"] = DeptCode;
            Session["CorrelationDept"] = DeptCode;
            Session["curdateyear"] = GetEndDate();
            Session["curmonths"] = GetBeginDate();

            this.Portlet_KPI_mz.Reload();
            this.Portlet_KPI_zy.Reload();
            this.Portlet_KPI_ss.Reload();
            this.Portlet_KPI_yzb.Reload();

            this.Portlet_jjhs.Reload();
            this.Portlet_gzl.Reload();
            this.Portlet_zlxl.Reload();
        }

        /// <summary>
        /// 获取开始时间
        /// </summary>
        /// <returns></returns>
        private string GetBeginDate()
        {
            string month = cbbmonth.SelectedItem.Value.ToString();
            if (month.Length == 1)
            {
                month = "0" + month;
            }
            return month;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private string GetEndDate()
        {
            string year = Combo_Year.SelectedItem.Value.ToString();
            return year;
        }

    }
}
