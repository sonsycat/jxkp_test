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
using GoldNet.Comm.ExportData;

namespace GoldNet.JXKP.cbhs.datagather
{
    public partial class account_data : PageBase
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
                Cbhs_dict dal = new Cbhs_dict();
                DataTable dt = dal.GetAccount_Signs().Tables[0];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    this.AccountSign.Items.Add(new Goldnet.Ext.Web.ListItem(dt.Rows[i]["ACCOUNT_TYPE"].ToString(), dt.Rows[i]["ID"].ToString()));
                }
                this.AccountSign.SelectedItem.Value = "1";
                Bindlist(System.DateTime.Now.ToString("yyyyMM"), this.AccountSign.SelectedItem.Value);
            }
        }
        //查询绑定数据
        private void Bindlist(string date_time, string account_sign)
        {
            AccountingData dal = new AccountingData();
            DataTable dt = dal.GetAccountdata(date_time, account_sign).Tables[0];
            this.Store1.DataSource = dt;
            this.Store1.DataBind();

        }
        //时间条件改变
        protected void Date_SelectOnChange(object sender, EventArgs e)
        {
            string date_time = this.years.SelectedItem.Value + this.months.SelectedItem.Value;
            string account_sign = this.AccountSign.SelectedItem.Value;
            Bindlist(date_time, account_sign);
        }
        //结算标识改变
        protected void AccountSign_SelectOnChange(object sender, EventArgs e)
        {
            string date_time = this.years.SelectedItem.Value + this.months.SelectedItem.Value;
            string account_sign = this.AccountSign.SelectedItem.Value;
            Bindlist(date_time, account_sign);
        }
        //核算数据生成
        protected void Button_create_click(object sender, EventArgs e)
        {
            string date_time = this.years.SelectedItem.Value + this.months.SelectedItem.Value;
            string account_sign = this.AccountSign.SelectedItem.Value;
            string ypcode = System.Configuration.ConfigurationManager.AppSettings["YPCODE"];
            AccountingData dal = new AccountingData();
            try
            {
                string rtmsg = dal.AccountDataCreate(date_time, ypcode);
                if (rtmsg == "")
                {
                    Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                    {
                        Title = "信息提示",
                        Message = "生成成功",
                        Buttons = MessageBox.Button.OK,
                        Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                    });
                }
                else
                {
                    this.ShowDataError(rtmsg, Request.Path, "Button_create_click");
                }

                Bindlist(date_time, account_sign);
            }
            catch (Exception ex)
            {
                this.ShowDataError(ex.Message.ToString(), Request.Path, "Button_create_click");
            }
        }
        //删除核算数据
        protected void Button_del_click(object sender, EventArgs e)
        {
            string date_time = this.years.SelectedItem.Value + this.months.SelectedItem.Value;
            string account_sign = this.AccountSign.SelectedItem.Value;
            AccountingData dal = new AccountingData();
            try
            {
                dal.DelAccountData(date_time);
                Bindlist(date_time, account_sign);
            }
            catch(Exception ex)
            {
                this.ShowDataError(ex.Message.ToString(), Request.Path, "Button_del_click");
            }
        }
        //导出Excel
        protected void Button_OutExcel_click(object sender, EventArgs e)
        {
            ExportData ex = new ExportData();
            AccountingData dal = new AccountingData();
            string date_time = this.years.SelectedItem.Text + this.months.SelectedItem.Text;
            string account_sign = this.AccountSign.SelectedItem.Value;
            DataTable dt = dal.GetAccountdata(date_time, account_sign).Tables[0];
            if (dt.Rows.Count > 0)
            {
                dt.Columns.Remove("DEPT_CODE");
                dt.Columns["DEPT_NAME"].ColumnName = "科室名称";
                dt.Columns["INCOMES_CHARGES"].ColumnName = "实际收入";
                dt.Columns["INCOME_COUNT"].ColumnName = "减免收入";
                dt.Columns["INCOMES"].ColumnName = "总收入";
                dt.Columns["COST_FAC"].ColumnName = "实际成本";
                dt.Columns["COST_ARM"].ColumnName = "减免成本";
                dt.Columns["COSTS"].ColumnName = "总成本";
                dt.Columns["GROSS_INCOME"].ColumnName = "地方收益";
                dt.Columns["NET_INCOME"].ColumnName = "收益";
                dt.Columns["DEPT_LRL"].ColumnName = "收益率";
                dt.Columns["DEPT_CBL"].ColumnName = "成本率";
                dt.Columns["DEPT_GDCBL"].ColumnName = "固定成本率";
                dt.Columns["DEPT_BDCBL"].ColumnName = "非固定成本率";
                dt.Columns["DEPT_YPSRL"].ColumnName = "药品收入比";
                ex.ExportToLocal(dt, this.Page, "xls", "奖金核算数据");
            }
        }
    }
}
