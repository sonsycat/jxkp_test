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
    public partial class income_item_account : PageBase
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
                string date_time = this.years.SelectedItem.Value + this.months.SelectedItem.Value;
                Bindlist(date_time, this.AccountSign.SelectedItem.Value);
                AccountingData acc_dal = new AccountingData();
                string guide_code = System.Configuration.ConfigurationManager.AppSettings["MSG_GUIDE_CODE"];
                double msg_value= acc_dal.GetMsgCosts(date_time, guide_code);
                this.AllMagCosts.Value = msg_value;
            }

        }
        private void Bindlist(string date_time, string account_sign)
        {
            AccountingData dal = new AccountingData();
            DataTable dt = dal.GetItemAccountData(date_time,account_sign).Tables[0];
            this.Store1.DataSource = dt;
            this.Store1.DataBind();
        }
        protected void Data_SelectOnChange(object sender, EventArgs e)
        {
            string date_time = this.years.SelectedItem.Value + this.months.SelectedItem.Value;
            string account_sign = this.AccountSign.SelectedItem.Value;
            AccountingData acc_dal = new AccountingData();
            string guide_code = System.Configuration.ConfigurationManager.AppSettings["MSG_GUIDE_CODE"];
            double msg_value = acc_dal.GetMsgCosts(date_time, guide_code);
            this.AllMagCosts.Value = msg_value;
            Bindlist(date_time,account_sign);
        }
        protected void Button_create_click(object sender, EventArgs e)
        {
            string date_time = this.years.SelectedItem.Value + this.months.SelectedItem.Value;
            string account_sign = this.AccountSign.SelectedItem.Value;
            double costvalue;
            AccountingData dal = new AccountingData();
            string rtmsg = "";
            try
            {
                costvalue = Convert.ToDouble(this.AllMagCosts.Value);
                rtmsg = dal.Exec_Sp_Income_Item_Account(date_time, costvalue);
                if (rtmsg == "")
                {
                    Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                    {
                        Title = "信息提示",
                        Message = "生成成功",
                        Buttons = MessageBox.Button.OK,
                        Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                    });
                    Bindlist(date_time, account_sign);
                }
                else
                {
                    this.ShowDataError(rtmsg, Request.Path, "Button_create_click");
                }
            }
            catch (Exception ex)
            {
                this.ShowDataError(ex.Message.ToString(), Request.Path, "Button_create_click");
            }
        }
         //导出Excel
        protected void Button_OutExcel_click(object sender, EventArgs e)
        {
            ExportData ex = new ExportData();
            string date_time = this.years.SelectedItem.Text + this.months.SelectedItem.Text;
            string account_sign = this.AccountSign.SelectedItem.Value;
            AccountingData dal = new AccountingData();
            DataTable dt = dal.GetItemAccountData(date_time, account_sign).Tables[0];
            if (dt.Rows.Count > 0)
            {
                dt.Columns.Remove("ITEM_CODE");
                dt.Columns["ITEM_NAME"].ColumnName = "项目名称";
                dt.Columns["INCOMES_DF"].ColumnName = "实际收入";
                dt.Columns["INCOMES_JD"].ColumnName = "减免收入";
                dt.Columns["INCOMES"].ColumnName = "总收入";
                dt.Columns["COSTS_ZJ"].ColumnName = "直接成本";
                dt.Columns["COSTS_JJ"].ColumnName = "间接成本";
                dt.Columns["COSTS"].ColumnName = "总成本";
                dt.Columns["BENEFIT"].ColumnName = "效益";
                ex.ExportToLocal(dt, this.Page, "xls", "单项目核算数据");
            }
        }
    }
}
