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

namespace GoldNet.JXKP.cbhs.xyhs
{
    public partial class benefit_account : PageBase
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
                Bindlist(this.years.SelectedItem.Value + this.months.SelectedItem.Value);
            }
        }
        //查询绑定数据
        private void Bindlist(string date_time)
        {
            XyhsDetail dal = new XyhsDetail();
            DataTable dt = dal.GetXyhsDeptData(date_time).Tables[0];
            this.Store1.DataSource = dt;
            this.Store1.DataBind();
        }
        //查询条件改变
        protected void Data_SelectOnChange(object sender, EventArgs e)
        {
            string date_time = this.years.SelectedItem.Value + this.months.SelectedItem.Value;
            Bindlist(date_time);
        }
        //查询
        protected void Button_find_click(object sender, EventArgs e)
        {
            string date_time = this.years.SelectedItem.Value + this.months.SelectedItem.Value;
            Bindlist(date_time);
        }
        //生成
        protected void Button_create_click(object sender, EventArgs e)
        {
            string date_time = this.years.SelectedItem.Value + this.months.SelectedItem.Value;
            GoldNet.Model.User user = (GoldNet.Model.User)Session["CURRENTSTAFF"];
            string opuser = user.UserId;
            string rtMsg = "";
            XyhsDetail dal = new XyhsDetail();
            try
            {
                rtMsg = dal.Exec_Sp_Dept_Account(date_time, opuser);
                if (rtMsg == null || rtMsg == "")
                {
                    this.ShowMessage("系统提示","生成成功！");
                }
                else
                {
                    this.ShowDataError(rtMsg, Request.Path, "Button_create_click");
                }

            }
            catch (Exception ex)
            {
                this.ShowDataError(ex.Message, Request.Path, "Button_create_click");
            }
            Bindlist(date_time);
        }
        //明细、构成比显示事件
        protected void Btn_Command_Click(object sender, AjaxEventArgs e)
        {
            string command = e.ExtraParams["command"].ToString();
            string dept_code = e.ExtraParams["Dept_code"].ToString();
            string dept_name = e.ExtraParams["Dept_name"].ToString();
            string date_time = this.years.SelectedItem.Value + this.months.SelectedItem.Value;
            if (command.Equals("SR_SCALE"))
            {
                LoadConfig loadcfg = getLoadConfig("dept_costs_deal_scale.aspx");
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("dept_code", dept_code));
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("dept_name", dept_name));
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("op_type", "收入"));
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("date_time", this.years.SelectedItem.Value + this.months.SelectedItem.Value));
                SRScaleWin.ClearContent();
                SRScaleWin.Show();
                SRScaleWin.LoadContent(loadcfg);
            }
            if (command.Equals("CB_SCALE"))
            {
                LoadConfig loadcfg = getLoadConfig("dept_costs_deal_scale.aspx");
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("dept_code", dept_code));
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("dept_name", dept_name));
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("op_type", "成本"));
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("date_time", this.years.SelectedItem.Value + this.months.SelectedItem.Value));
                CBScaleWin.ClearContent();
                CBScaleWin.Show();
                CBScaleWin.LoadContent(loadcfg);
            }
            if(command.Equals("DETAIL"))
            {
                LoadConfig loadcfg = getLoadConfig("benefit_line.aspx");
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("dept_code", dept_code));
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("dept_name", dept_name));
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("date_time", this.years.SelectedItem.Value + this.months.SelectedItem.Value));
                DETAILWin.ClearContent();
                DETAILWin.Show();
                DETAILWin.LoadContent(loadcfg);
            }
        }
    }
}
