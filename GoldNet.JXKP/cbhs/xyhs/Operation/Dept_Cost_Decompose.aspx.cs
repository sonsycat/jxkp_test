using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Goldnet.Dal;
using Goldnet.Ext.Web;
using System.Text;

namespace GoldNet.JXKP.cbhs.xyhs.Operation
{
    public partial class Dept_Cost_Decompose : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                //检查是否已经登录，否则停止
                if (Session["CURRENTSTAFF"] == null)
                {
                    Response.End();
                }

                for (int i = 0; i < 10; i++)
                {
                    int years = System.DateTime.Now.Year - i;
                    this.years.Items.Add(new Goldnet.Ext.Web.ListItem(years.ToString(), years.ToString()));
                }
                this.years.SelectedItem.Value = System.DateTime.Now.ToString("yyyy");
                this.months.SelectedItem.Value = System.DateTime.Now.ToString("MM");
                setDict();
                Bindlist();
                
            }
        }
        //查询绑定数据
        private void Bindlist()
        {
            string date_time = this.years.SelectedItem.Value + this.months.SelectedItem.Value;
            string dept_type = this.Combo_DeptType.SelectedItem.Value.ToString();
            XyhsOperation dal = new XyhsOperation();
            //科室过滤条件
            string deptFilter = DeptFilter("");
            DataTable dt = dal.GetDealedCosts(date_time, dept_type,deptFilter).Tables[0];
            this.Store1.DataSource = dt;
            this.Store1.DataBind();
        }
        //初始化科室类别查询条件下拉框
        private void setDict()
        {
            XyhsOperation dal = new XyhsOperation();
            DataTable dt = dal.GetDeptType().Tables[0];
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    this.Combo_DeptType.Items.Add(new Goldnet.Ext.Web.ListItem(dt.Rows[i]["XYHS_DEPT_TYPE"].ToString(), dt.Rows[i]["ID"].ToString()));
                }
            }
        }
        //界面查询事件
        protected void Button_find_click(object sender, EventArgs e)
        {
            Bindlist();
        }
        //日期选择改变事件
        protected void Data_SelectOnChange(object sender, EventArgs e)
        {
            Bindlist();
        }
        //分摊
        protected void Button_create_click(object sender, EventArgs e)
        {
            string date_time = this.years.SelectedItem.Value + this.months.SelectedItem.Value;
            XyhsOperation dal = new XyhsOperation();
           
            string rtMsg = "";
            try
            {
                rtMsg = dal.Exec_Sp_Cost_Deal(date_time);
                if (rtMsg == "")
                {
                    this.ShowMessage("系统提示","分摊成功！");
                    Bindlist();
                }
                else
                {
                    this.ShowDataError(rtMsg, Request.Path, "Button_create_click");
                }

            }
            catch (Exception ex)
            {
                this.ShowDataError(ex.Message.ToString(), Request.Path, "Button_create_click");
            }

        }
        //明细、构成比显示事件
        protected void Btn_Command_Click(object sender, AjaxEventArgs e)
        {
            string command = e.ExtraParams["command"].ToString();
            string dept_code = e.ExtraParams["Dept_code"].ToString();
            string dept_name = e.ExtraParams["Dept_name"].ToString();
            string date_time = this.years.SelectedItem.Value + this.months.SelectedItem.Value;
            if (command.Equals("DETAIL"))
            {
                LoadConfig loadcfg = getLoadConfig("dept_costs_deal_detail.aspx");
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("dept_code", dept_code));
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("dept_name", dept_name));
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("op_type", "成本"));
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("date_time", this.years.SelectedItem.Value + this.months.SelectedItem.Value));
                DetailWin.ClearContent();
                DetailWin.Show();
                DetailWin.LoadContent(loadcfg);

            }
            else
            {
                if (command.Equals("SCALE"))
                {
                    LoadConfig loadcfg = getLoadConfig("dept_costs_deal_scale.aspx");
                    loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("dept_code", dept_code));
                    loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("dept_name", dept_name));
                    loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("op_type", "成本"));
                    loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("date_time", this.years.SelectedItem.Value + this.months.SelectedItem.Value));
                    ScaleWin.ClearContent();
                    ScaleWin.Show();
                    ScaleWin.LoadContent(loadcfg);
                }
            }
        }

    }
}
