using System;
using Goldnet.Ext.Web;
using Goldnet.Dal;
using System.Data;
using GoldNet.Comm.ExportData;

namespace GoldNet.JXKP.cbhs.Report
{
    public partial class New_IncomeDetail : PageBase
    {
        Goldnet.Dal.Report dal_report = new Goldnet.Dal.Report();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                string year = DateTime.Now.AddMonths(-1).Year.ToString();
                string month = DateTime.Now.AddMonths(-1).Month.ToString();
                BoundComm boundcomm = new BoundComm();
                SYear.DataSource = boundcomm.getYears();
                SYear.DataBind();
                cbbYear.Value = year;
                SMonth.DataSource = boundcomm.getMonth();
                SMonth.DataBind();
                cbbmonth.Value = month;


                cbbType.Value = 0;
            }
        }

        protected void Btn_Query_Click(object sender, AjaxEventArgs e)
        {
            string balance = cbbType.SelectedItem.Value;
          
            string hospro = System.Configuration.ConfigurationManager.AppSettings["HOSPRO"];
            DataTable dt = dal_report.GetNew_IncomeDetail(GetBeginDate());
            if (dt != null)
            {
                SReport.DataSource = dt;
                SReport.DataBind();
                Session.Remove("New_IncomeDetail");
                Session["New_IncomeDetail"] = dt;
            }
            else
            {
                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                {
                    Title = "提示",
                    Message = "未找到数据",
                    Buttons = MessageBox.Button.OK,
                    Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                });
            }
        }
        protected void OutExcel(object sender, EventArgs e)
        {
            if (Session["New_IncomeDetail"] != null)
            {
                ExportData ex = new ExportData();
                DataTable dt = (DataTable)Session["New_IncomeDetail"];


                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (dt.Columns[i].ColumnName == "ITEM_NAME")
                    {
                        dt.Columns[i].ColumnName = "项目";
                    }
                    else if (dt.Columns[i].ColumnName == "Z_B_INCOME")
                    {
                        dt.Columns[i].ColumnName = "总收入本期数据";
                    }
                    else if (dt.Columns[i].ColumnName == "Z_T_INCOME")
                    {
                        dt.Columns[i].ColumnName = "总收入去年同期";
                    }
                    else if (dt.Columns[i].ColumnName == "Z_ZZL")
                    {
                        dt.Columns[i].ColumnName = "总收入增长率";
                    }
                    else if (dt.Columns[i].ColumnName == "D_B_INCOME")
                    {
                        dt.Columns[i].ColumnName = "地方实收本期数据";
                    }
                    else if (dt.Columns[i].ColumnName == "D_T_INCOME")
                    {
                        dt.Columns[i].ColumnName = "地方实收去年同期";
                    }
                    else if (dt.Columns[i].ColumnName == "D_ZZL")
                    {
                        dt.Columns[i].ColumnName = "地方实收增长率";
                    }
                    else if (dt.Columns[i].ColumnName == "J_B_INCOME")
                    {
                        dt.Columns[i].ColumnName = "军人虚收本期数据";
                    }
                    else if (dt.Columns[i].ColumnName == "J_T_INCOME")
                    {
                        dt.Columns[i].ColumnName = "军人虚收去年同期";
                    }
                    else if (dt.Columns[i].ColumnName == "J_ZZL")
                    {
                        dt.Columns[i].ColumnName = "军人虚收增长率";
                    }
                }
                string dates = this.cbbYear.SelectedItem.Value + "年" + this.cbbmonth.SelectedItem.Value + "月";
                ex.ExportToLocal(dt, this.Page, "xls", "医院收入结构明细表(" + dates + ")");
            }
        }
        /// <summary>
        /// 开始时间
        /// </summary>
        /// <returns></returns>
        private string GetBeginDate()
        {
            string year = cbbYear.SelectedItem.Value.ToString();
            string month = cbbmonth.SelectedItem.Value.ToString();
            if (month.Length == 1)
            {
                month = "0" + month;
            }
            string benginDate = year +"-"+ month;
            return benginDate;
        }
    }
}
