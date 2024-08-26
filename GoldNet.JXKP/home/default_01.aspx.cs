using System;
using System.Data;
using Goldnet.Ext.Web;
using GoldNet.Comm;
using GoldNet.Model;
using Goldnet.Dal.home;
using GoldNet.JXKP;

namespace Goldnet.JXKP.home
{
    public partial class _default_01 : PageBase
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
                    this.Portlet_KPI.AutoLoad.Url = "";
                    //this.Portlet_MZL.AutoLoad.Url = "";
                    //this.Portlet_ZYL.AutoLoad.Url = "";
                    //this.Portlet_GWZBJK.AutoLoad.Url = "";
                    //this.Portlet_SSL.AutoLoad.Url = "";
                    //this.Portlet_YLFY.AutoLoad.Url = "";
                    //this.Portlet_ZLJK.AutoLoad.Url = "";
                    this.Portlet_ZRYL.AutoLoad.Url = "";
                    this.Portlet_Detail.AutoLoad.Url = "";
                }
            }
        }

        //页面各控件机能初始化
        protected bool ComponentInit()
        {
            DateTime dtime = new DateTime();
            if (dateoff == 0)
                dtime = DateTime.Parse(DateTime.Now.Year.ToString() + "-01" + "-01");
            else if (DateTime.Now.Month > 12 + dateoff)
                dtime = DateTime.Parse(DateTime.Now.Year.ToString() + "-" + (12 + dateoff + 1) + "-01");
            else
                dtime = DateTime.Parse(DateTime.Now.Year.ToString() + "-01" + "-01").AddMonths(dateoff);

            Label6.Text = "  数据日期：" + dtime.ToString("yyyy-MM-dd") + "  至  " + DateTime.Now.Date.AddDays(-1).ToString("yyyy-MM-dd");
            Portlet_GWZBJK.Title = Portlet_GWZBJK.Title + "(" + dtime.ToString("yyyy-MM-dd") + "  至  " + DateTime.Now.Date.AddDays(-1).ToString("yyyy-MM-dd") + ")";

            HomeDal dal = new HomeDal();

            //数据详情portlet 隐藏
            this.Portlet_Detail.Hide();

            //年度下拉框内容
            for (int i = 0; i < 10; i++)
            {
                int years = System.DateTime.Now.Year - i;
                this.Combo_Year.Items.Add(new Ext.Web.ListItem(years.ToString(), years.ToString()));
            }

            this.Combo_Year.Value = System.DateTime.Now.Year.ToString();

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
            if (DeptCode == "" || StaffId == "" || StationCode == "")
            {
                Goldnet.Ext.Web.Ext.Msg.Show(new MessageBox.Config { Title = SystemMsg.msgtitle1, Message = "用户未进岗，不能查看运行监控，请联系管理员进岗", Buttons = MessageBox.Button.OK, Icon = MessageBox.Icon.INFO, AnimEl = this.BtnQuery.ClientID });
                return false;
            }
            string meunid = System.Configuration.ConfigurationManager.AppSettings["PageMeunid"].ToString();
            string modid = System.Configuration.ConfigurationManager.AppSettings["modid"].ToString();

            DataTable deptTypetable = dal.getDeptType(user.GetUserDeptFilter("", meunid, modid)).Tables[0];
            for (int i = 0; i < deptTypetable.Rows.Count; i++)
            {
                this.Combo_DeptType.Items.Insert(i, new Ext.Web.ListItem(deptTypetable.Rows[i]["DEPT_TYPE"].ToString(), deptTypetable.Rows[i]["ID"].ToString()));
            }
            this.Combo_DeptType.SelectedItem.Value = DeptType;

            //获取权限范围内的科室
            DataTable depttable = dal.getDeptDict(user.GetUserDeptFilter("", meunid, modid), DeptType).Tables[0];
            for (int i = 0; i < depttable.Rows.Count; i++)
            {
                this.Combo_Dept.Items.Insert(i, new Ext.Web.ListItem(depttable.Rows[i]["DEPT_NAME"].ToString(), depttable.Rows[i]["DEPT_CODE"].ToString()));
            }
            this.Combo_Dept.SelectedItem.Value = DeptCode;

            //获取科室范围内的岗位
            DataTable Stationdt = dal.GetStateStionBydept(DeptCode, DateTime.Now.Year.ToString()).Tables[0];
            for (int i = 0; i < Stationdt.Rows.Count; i++)
            {
                this.Combo_Station.Items.Insert(i, new Ext.Web.ListItem(Stationdt.Rows[i]["STATION_NAME"].ToString(), Stationdt.Rows[i]["STATION_CODE"].ToString()));
            }
            this.Combo_Station.SelectedItem.Value = StationCode;

            //获取岗位关联的人员
            DataTable PersonDt = dal.GetStatePersonByStion(StationCode, DateTime.Now.Year.ToString()).Tables[0];
            for (int i = 0; i < PersonDt.Rows.Count; i++)
            {
                this.Combo_Person.Items.Insert(i, new Ext.Web.ListItem(PersonDt.Rows[i]["person_name"].ToString(), PersonDt.Rows[i]["person_id"].ToString()));
            }
            this.Combo_Person.SelectedItem.Value = userId;

            return true;
        }

        //科室类别下拉框触发事件
        protected void SelectedDeptType(object sender, AjaxEventArgs e)
        {
            this.Combo_Dept.Disabled = false;
            this.BtnQuery.Disabled = true;
            //通过脚本的方式清除原有的数据，清除选中的数据，使用AddItem方式生成数据脚本
            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
            scManager.AddScript(Combo_Dept.ClientID + ".store.removeAll();");
            scManager.AddScript(Combo_Dept.ClientID + ".clearValue();");
            scManager.AddScript(Combo_Station.ClientID + ".store.removeAll();");
            scManager.AddScript(Combo_Station.ClientID + ".clearValue();");
            scManager.AddScript(Combo_Person.ClientID + ".store.removeAll();");
            scManager.AddScript(Combo_Person.ClientID + ".clearValue();");
            HomeDal dal = new HomeDal();
            Session["curdateyear"] = this.Combo_Year.SelectedItem.Value.ToString();

            User user = (User)Session["CURRENTSTAFF"];

            string meunid = System.Configuration.ConfigurationManager.AppSettings["PageMeunid"].ToString();
            string modid = System.Configuration.ConfigurationManager.AppSettings["modid"].ToString();

            DataTable depttable = dal.getDeptDict(user.GetUserDeptFilter("", meunid, modid), this.Combo_DeptType.SelectedItem.Value.ToString()).Tables[0];// GoldNet.Comm.Power.Power.GetDeptByType(this.Combo_DeptType.SelectedItem.Value.ToString()).Tables[0];
            if (depttable.Rows.Count > 0)
            {
                for (int i = 0; i < depttable.Rows.Count; i++)
                {
                    this.Combo_Dept.AddItem(depttable.Rows[i]["dept_name"].ToString(), depttable.Rows[i]["dept_code"].ToString());
                }

            }

        }

        //科室下拉框触发事件
        protected void SelectedDept(object sender, AjaxEventArgs e)
        {
            this.Combo_Station.Disabled = false;
            this.BtnQuery.Disabled = true;
            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
            scManager.AddScript(Combo_Station.ClientID + ".store.removeAll();");
            scManager.AddScript(Combo_Station.ClientID + ".clearValue();");
            scManager.AddScript(Combo_Person.ClientID + ".store.removeAll();");
            scManager.AddScript(Combo_Person.ClientID + ".clearValue();");

            HomeDal dal = new HomeDal();
            Session["curdeptcode"] = this.Combo_Dept.SelectedItem.Value.ToString();
            Session["curdateyear"] = this.Combo_Year.SelectedItem.Value.ToString();
            DataTable stationtable = dal.GetStateStionBydept(this.Combo_Dept.SelectedItem.Value, this.Combo_Year.SelectedItem.Value).Tables[0];// GoldNet.Comm.Power.Power.GetStateStionBydept(this.Combo_Dept.SelectedItem.Value.ToString(), this.Combo_Year.SelectedItem.Value.ToString()).Tables[0];
            if (stationtable.Rows.Count > 0)
            {
                for (int i = 0; i < stationtable.Rows.Count; i++)
                {
                    this.Combo_Station.AddItem(stationtable.Rows[i]["station_name"].ToString(), stationtable.Rows[i]["station_code"].ToString());
                }
            }

        }

        //岗位下拉框触发事件
        protected void SelectedStation(object sender, AjaxEventArgs e)
        {
            this.Combo_Person.Disabled = false;

            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
            scManager.AddScript(Combo_Person.ClientID + ".store.removeAll();");
            scManager.AddScript(Combo_Person.ClientID + ".clearValue();");

            HomeDal dal = new HomeDal();
            Session["curstationcode"] = this.Combo_Station.SelectedItem.Value.ToString();
            Session["curdateyear"] = this.Combo_Year.SelectedItem.Value.ToString();

            DataTable persontable = dal.GetStatePersonByStion(this.Combo_Station.SelectedItem.Value, this.Combo_Year.SelectedItem.Value).Tables[0];//GoldNet.Comm.Power.Power.GetStatePersonByStion(this.Combo_Station.SelectedItem.Value.ToString(), this.Combo_Year.SelectedItem.Value.ToString()).Tables[0];
            if (persontable.Rows.Count > 0)
            {
                this.BtnQuery.Disabled = false;
                for (int i = 0; i < persontable.Rows.Count; i++)
                {
                    this.Combo_Person.AddItem(persontable.Rows[i]["person_name"].ToString(), persontable.Rows[i]["person_id"].ToString());
                }
            }
        }

        //页面刷新，保持Session会话状态
        protected void RefreshTime(object sender, AjaxEventArgs e)
        {
            return;
        }

        //查询按钮触发事件
        protected void GetQueryPortalet(object sender, AjaxEventArgs e)
        {
            if (this.Combo_Dept.SelectedItem.Value.ToString().Equals(""))
            {
                Goldnet.Ext.Web.Ext.Msg.Show(new MessageBox.Config { Title = SystemMsg.msgtitle1, Message = "科室" + SystemMsg.msgtipnull, Buttons = MessageBox.Button.OK, Icon = MessageBox.Icon.INFO, AnimEl = this.BtnQuery.ClientID });
                return;
            }
            else if (this.Combo_Station.SelectedItem.Value.ToString().Equals(""))
            {
                Goldnet.Ext.Web.Ext.Msg.Show(new MessageBox.Config { Title = SystemMsg.msgtitle1, Message = "岗位" + SystemMsg.msgtipnull, Buttons = MessageBox.Button.OK, Icon = MessageBox.Icon.INFO, AnimEl = this.BtnQuery.ClientID });
                return;
            }
            else if (this.Combo_Person.SelectedItem.Value.ToString().Equals(""))
            {
                Goldnet.Ext.Web.Ext.Msg.Show(new MessageBox.Config { Title = SystemMsg.msgtitle1, Message = "人员" + SystemMsg.msgtipnull, Buttons = MessageBox.Button.OK, Icon = MessageBox.Icon.INFO, AnimEl = this.BtnQuery.ClientID });
                return;
            }
            Session["curpersonid"] = this.Combo_Person.SelectedItem.Value.ToString();
            Session["curstationcode"] = this.Combo_Station.SelectedItem.Value.ToString();
            Session["curdeptcode"] = this.Combo_Dept.SelectedItem.Value.ToString();
            Session["CorrelationDept"] = this.Combo_Dept.SelectedItem.Value.ToString();
            Session["curdateyear"] = this.Combo_Year.SelectedItem.Value.ToString();

            this.Portlet_KPI.Reload();
            //this.Portlet_MZL.Reload();
            //this.Portlet_ZYL.Reload();
            //this.Portlet_GWZBJK.Reload();
            //this.Portlet_SSL.Reload();
            //this.Portlet_YLFY.Reload();
            //this.Portlet_ZLJK.Reload();
            this.Portlet_ZRYL.Reload();
            this.Portlet_Detail.Hide();
        }
    }
}