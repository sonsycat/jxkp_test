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

namespace GoldNet.JXKP.cbhs.Report
{
    public partial class New_Earnings : PageBase
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


            DataTable dt = dal_report.GetNew_Earnings(GetBeginDate());
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
                       
                        if (dt.Columns[i].ColumnName == "DEPT_NAME")
                        {
                            Column.Header = "<div style='text-align:center;'>" + "科室名称" + "</div>";
                        }
                        if (dt.Columns[i].ColumnName == "BQSJ")
                        {
                            Column.Header = "<div style='text-align:center;'>" + "本期数据" + "</div>";
                            Column.Renderer.Fn = "rmbMoney";
                            Column.Align = Alignment.Right;
                        }
                        if (dt.Columns[i].ColumnName == "QNTQ")
                        {
                            Column.Header = "<div style='text-align:center;'>" + "去年同期" + "</div>";
                            Column.Renderer.Fn = "rmbMoney";
                            Column.Align = Alignment.Right;
                        }
                        if (dt.Columns[i].ColumnName == "ZZL")
                        {
                            Column.Header = "<div style='text-align:center;'>" + "增长率" + "</div>";
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
                Session.Remove("New_CostsDetail");
                Session["New_CostsDetail"] = dt;
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
            if (Session["New_CostsDetail"] != null)
            {
                ExportData ex = new ExportData();
                DataTable dt = (DataTable)Session["New_CostsDetail"];
                //dt.Columns.Remove("DEPT_CODE");
                //ex.ExportToLocal(dt, this.Page, "xls", "核算单位成本报表");
                this.outexcel(dt, "医院医药成本明细表");
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
            string benginDate = year + "-" + month;
            return benginDate;
        }
    }
}
