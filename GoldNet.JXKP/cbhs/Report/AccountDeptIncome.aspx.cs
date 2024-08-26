using System;
using Goldnet.Ext.Web;
using Goldnet.Dal;
using System.Data;
using GoldNet.Comm.ExportData;

namespace GoldNet.JXKP
{
    public partial class AccuntDeptIncome : PageBase
    {
        Report dal_report = new Report();
        /// <summary>
        /// 核算单位收入报表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                string year = DateTime.Now.Year.ToString();
                string month = DateTime.Now.Month.ToString();
                BoundComm boundcomm = new BoundComm();
                SYear.DataSource = boundcomm.getYears();
                SYear.DataBind();
                cbbYear.Value = year;
                ccbYearTo.Value = year;
                SMonth.DataSource = boundcomm.getMonth();
                SMonth.DataBind();
                cbbmonth.Value = month;
                ccbMonthTo.Value = month;

                cbbType.Value = 1;
            }
        }

        /// <summary>
        /// 查询处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Query_Click(object sender, AjaxEventArgs e)
        {
            string balance = cbbType.SelectedItem.Value;
            string deptcode = this.DeptFilter("");
            string hospro = System.Configuration.ConfigurationManager.AppSettings["HOSPRO"];
            DataTable dt = dal_report.GetAccountDeptIncome(GetBeginDate(), GetEndDate(), balance, deptcode, hospro);
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
                        if (dt.Columns[i].ColumnName != "科室" && dt.Columns[i].ColumnName != "收入类别")
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
                Session.Remove("AccuntDeptIncome");
                Session["AccuntDeptIncome"] = dt;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OutExcel(object sender, EventArgs e)
        {
            if (Session["AccuntDeptIncome"] != null)
            {
                ExportData ex = new ExportData();
                DataTable dt = (DataTable)Session["AccuntDeptIncome"];
                dt.Columns.Remove("DEPT_CODE");
                //ex.ExportToLocal(dt, this.Page, "xls", "核算单位收入明细表");
                this.outexcel(dt, "核算单位收入明细表");
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
