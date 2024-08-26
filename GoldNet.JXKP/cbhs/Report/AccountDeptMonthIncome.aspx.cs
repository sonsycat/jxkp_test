using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Goldnet.Ext.Web;
using Goldnet.Dal;
using System.Data;
using GoldNet.Comm;
using GoldNet.Comm.ExportData;

namespace GoldNet.JXKP
{
    /// <summary>
    /// 核算单位成本报表
    /// </summary>
    public partial class AccountDeptMonthIncome : PageBase
    {
        Report dal_report = new Report();
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
                ccbYearTo.Value = year;
                SMonth.DataSource = boundcomm.getMonth();
                SMonth.DataBind();
                cbbmonth.Value = month;
                ccbMonthTo.Value = month;

                cbbType.SelectedItem.Value = "0";
                //cbbType.SelectedItem.Text = "收付实现";

            }
        }

        protected void Btn_Query_Click(object sender, AjaxEventArgs e)
        {
            string begin = cbbYear.SelectedItem.Value.ToString() + "-" + cbbmonth.SelectedItem.Value.ToString() + "-01";
            string end = ccbYearTo.SelectedItem.Value.ToString() + "-" + ccbMonthTo.SelectedItem.Value.ToString() + "-01";
            DateTime dtBegin = Convert.ToDateTime(begin);
            DateTime dtEnd = Convert.ToDateTime(end);
            if (dtEnd.CompareTo(dtBegin) < 0)
            {
                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                {
                    Title = "提示",
                    Message = "开始月份大于结束月份",
                    Buttons = MessageBox.Button.OK,
                    Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                });
                return;
            }
            int months = Convert.ToInt32((Convert.ToDateTime(dtEnd).Year - Convert.ToDateTime(dtBegin).Year) * 12 + Convert.ToDateTime(dtEnd).Month - Convert.ToDateTime(dtBegin).Month);

            string balance = cbbType.SelectedItem.Value;
            string deptcode = this.DeptFilter("");
            string hospro = System.Configuration.ConfigurationManager.AppSettings["HOSPRO"];
            DataTable dt = dal_report.GetAccountDeptMonthIncome(dtBegin, balance, deptcode, months + 1, hospro);
            if (dt != null)
            {
                SReport.RemoveFields();
                GridPanel_Show.Reconfigure();
                GridPanel_Show.ColumnModel.Columns.Clear();
                GoldNet.JXKP.cbhs.Report.BuildControl bc = new GoldNet.JXKP.cbhs.Report.BuildControl();
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (dt.Columns[i].ColumnName != "DEPT_CODE")
                    {
                        bc.AddRecord(dt.Columns[i].ColumnName, SReport);
                        ExtColumn Column = new ExtColumn();
                        Column.ColumnID = dt.Columns[i].ColumnName;
                        Column.Header = "<div style='text-align:center;'>" + dt.Columns[i].ColumnName + "</div>";
                        Column.Align = Alignment.Left;
                        if (dt.Columns[i].ColumnName != "科室")
                        {
                            Column.Renderer.Fn = "rmbMoney";
                            Column.Align = Alignment.Right;
                        }
                        Column.DataIndex = dt.Columns[i].ColumnName;
                        Column.MenuDisabled = true;
                        Column.Width = 120;
                        GridPanel_Show.ColumnModel.Columns.Add(Column);
                        GridPanel_Show.AddColumn(Column);
                    }
                }

                SReport.DataSource = dt;
                SReport.DataBind();
                Session.Remove("AccountDeptMonthIncome");
                Session["AccountDeptMonthIncome"] = dt;
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
            if (Session["AccountDeptMonthIncome"] != null)
            {
                ExportData ex = new ExportData();
                DataTable dt = (DataTable)Session["AccountDeptMonthIncome"];
                dt.Columns.Remove("DEPT_CODE");
                ex.ExportToLocal(dt, this.Page, "xls", "各单位核算收入表");
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
            string benginDate = year + month + "01";
            return benginDate;
        }
        /// <summary>
        /// 结束时间
        /// </summary>
        /// <returns></returns>
        private string GetEndDate()
        {
            string year = ccbYearTo.SelectedItem.Value.ToString();
            string month = ccbMonthTo.SelectedItem.Value.ToString();
            if (month.Length == 1)
            {
                month = "0" + month;
            }
            string endDate = year + month + "01";
            return endDate;
        }
    }
}
