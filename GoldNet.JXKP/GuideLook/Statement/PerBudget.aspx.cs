using System;
using System.Collections;
using System.Configuration;
using System.Data;
using Goldnet.Ext.Web;
using GoldNet.Comm.ExportData;
using Goldnet.Dal;

namespace GoldNet.JXKP.GuideLook.Statement
{
    public partial class PerBudget : System.Web.UI.Page
    {
        private StatementDal dal = new StatementDal();
        private static DataTable Currdt = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            //检查是否已经登录，否则停止
            if (Session["CURRENTSTAFF"] == null)
            {
                Response.End();
                return;
            }

            if (!Ext.IsAjaxRequest)
            {
                string deptCode = Request.QueryString["DeptCode"].ToString();
                string fromDate = Request.QueryString["FromDate"].ToString();
                string ToDate = Request.QueryString["ToDate"].ToString();
                Currdt = dal.GetPerBudget(fromDate, ToDate, deptCode).Tables[0];
                CreateReportPanel(Currdt);
            }
        }

        /// <summary>
        /// 自定义报表
        /// </summary>
        /// <param name="ReportInfo">报表信息</param>
        /// <param name="ReportColName">指标名称</param>
        private void CreateReportPanel(DataTable ReportInfo)
        {

            this.Store1.RemoveFields();

            this.GridPanel_Show.Reconfigure();

            RecordField field = null;
            for (int i = 0; i < ReportInfo.Columns.Count; i++)
            {

                field = new RecordField(ReportInfo.Columns[i].ColumnName, RecordFieldType.String);
                
                this.Store1.AddField(field, i);

                Column col = new Column();
                col.Header = ReportInfo.Columns[i].ColumnName;
                col.Width = col.Header.Length * 40;
                col.Sortable = true;
                col.DataIndex = ReportInfo.Columns[i].ColumnName;
                col.Align = Alignment.Right;
                col.MenuDisabled = true;

                this.GridPanel_Show.AddColumn(col);
            }
            this.Store1.DataSource = ReportInfo;
            this.Store1.DataBind();
        }

        protected void OutExcel(object sender, EventArgs e)
        {
            ExportData ex = new ExportData();
            DataTable ExcelTable = new DataTable();
            ExcelTable = Currdt;
            ex.ExportToLocal(ExcelTable, this.Page, "xls", "收治情况明细");
        }
    }
}
