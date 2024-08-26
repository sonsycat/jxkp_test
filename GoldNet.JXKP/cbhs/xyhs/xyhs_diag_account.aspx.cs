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
using GoldNet.Model;


namespace GoldNet.JXKP.cbhs.xyhs
{
    public partial class xyhs_diag_account : PageBase
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
                //Bindlist();
                if (IsEdit())
                {
                    this.Button_create.Disabled = false;
                }
                else
                {
                    this.Button_create.Disabled = true;
                }
                XyhsDict dal = new XyhsDict();
                DataTable dt = dal.GetDiag().Tables[0];
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        this.Combo_diag.Items.Add(new Goldnet.Ext.Web.ListItem(dt.Rows[i]["DIAGNOSIS_NAME"].ToString(), dt.Rows[i]["DIAGNOSIS_CODE"].ToString()));
                    }
                }
            }
        }
        //查询绑定数据
        private void Bindlist()
        {
            string date_time = this.years.SelectedItem.Value + this.months.SelectedItem.Value;
            string dept_type = this.Combo_DeptType.SelectedItem.Value.ToString();
            XyhsDetail dal = new XyhsDetail();
            //科室过滤条件
            DataTable dt = dal.GetXyhsDiagaccount(date_time).Tables[0];
            DataRow dr = dt.NewRow();
            dr["DIAGNOSIS_NAME"] = "合计";
            dr["COSTS"] = dt.Compute("Sum(COSTS)", "");
            dr["DIAGNOSIS_COSTS"] = dt.Compute("Sum(DIAGNOSIS_COSTS)", "");
            dr["DIAGNOSIS"] = dt.Compute("Sum(DIAGNOSIS)", "");
            dt.Rows.Add(dr);
            this.Store1.DataSource = dt;
            this.Store1.DataBind();
        }

        //界面查询事件
        protected void Button_find_click(object sender, EventArgs e)
        {
            Bindlist();
        }
        protected void select_dept(object sender, EventArgs e)
        {
            Bindlist();
        }
        //日期选择改变事件
        protected void Data_SelectOnChange(object sender, EventArgs e)
        {
            // Bindlist();
        }
        //病种核算
        protected void Button_create_click(object sender, EventArgs e)
        {
            LoadConfig loadcfg = getLoadConfig("patient_account.aspx");

            showCenterSet(this.BuideWin, loadcfg);
        }
        //明细、构成比显示事件
        protected void Btn_Command_Click(object sender, AjaxEventArgs e)
        {
            string command = e.ExtraParams["command"].ToString();
            string diag_code = e.ExtraParams["diag_code"].ToString();
            string diag_name = e.ExtraParams["diag_name"].ToString();
            string date_time = this.years.SelectedItem.Value + this.months.SelectedItem.Value;
            if (command.Equals("DETAIL"))
            {
                LoadConfig loadcfg = getLoadConfig("xyhs_diag_account_detail.aspx");
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("diag_code", diag_code));
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("diag_name", diag_name));
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
                    loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("dept_code", diag_code));
                    loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("dept_name", diag_name));
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
