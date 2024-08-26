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
    public partial class ClinicPerformValuationIncome : PageBase
    {
        private Report report_dal = new Report();
        /// <summary>
        /// 门诊执行计价收入
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
                cbbDeptLevel.Value = 0;
            }
        }
        protected void Btn_Query_Click(object sender, AjaxEventArgs e)
        {
            Query();
        }
        private void Query()
        {
            string depttype = cbbDeptLevel.SelectedItem.Value;
            string deptcode = this.DeptFilter("");
            if (deptcode != "")
            {
                deptcode = "where dept_code in (" + deptcode + ")";
            }
            DataTable table = report_dal.GetClinicPerformValuationIncome(GetBeginDate(), GetEndDate(), deptcode, depttype);
            if (table != null && table.Rows.Count > 0)
            {
                GoldNet.JXKP.cbhs.Report.BuildControl bc = new GoldNet.JXKP.cbhs.Report.BuildControl();
                DataTable STable = bc.DataBind(table, GridPanel_Show, SReport);
                Session.Remove("ClinicPerformValuationIncome");
                Session["ClinicPerformValuationIncome"] = STable;
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
            if (Session["ClinicPerformValuationIncome"] != null)
            {
                ExportData ex = new ExportData();
                DataTable dt = (DataTable)Session["ClinicPerformValuationIncome"];
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (dt.Columns[i].ColumnName == "*")
                    {
                        dt.Columns[i].ColumnName = "未知项目";
                    }
                    else if (dt.Columns[i].ColumnName == "DEPT_NAME")
                    {
                        dt.Columns[i].ColumnName = "科室";
                    }
                }
                ex.ExportToLocal(dt, this.Page, "xls", "门诊执行科室计价收入统计表");
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
        //双击设置
        protected void DbRowClick(object sender, AjaxEventArgs e)
        {

            RowSelectionModel sm = this.GridPanel_Show.SelectionModel.Primary as RowSelectionModel;
            if (sm.SelectedRows.Count < 1)
            {
                this.ShowMessage("提示", "请选择一条记录！");
            }
            else
            {
                string deptcode = sm.SelectedRow.RecordID;
                string deptname = "OUTP_CLASS2_INCOME";
                string accountdept = cbbDeptLevel.SelectedItem.Value == "0" ? "account_dept_code" : "DEPT_CODE_SECOND";

                LoadConfig loadcfg = getLoadConfig("DeptIncome_Detail.aspx");

                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("stardate", GetBeginDate()));//开始时间
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("enddate", GetEndDate()));//结束时间
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("tablename", deptname));//表
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("deptcode", deptcode));//科室
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("groupbydept", "PERFORMED_BY"));//开单科室
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("incomescolums", "COUNT_INCOME"));//计价0/实收1
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("accountdept", accountdept));//二级科/三级科

                showCenterSet(this.Income_Detail, loadcfg);
            }
        }
    }
}
