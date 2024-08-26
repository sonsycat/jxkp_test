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
    public partial class Doctor_Report_Detail : PageBase
    {
        DataTable table = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {

                string stardate = Request["stardate"].ToString();
                string enddate = Request["enddate"].ToString();
                string tablename = Request["tablename"].ToString();
                string doctorname = Request["doctorname"].ToString();
                string patientid = Request["patientid"].ToString();
                string accountdeptcode = Request["accountdeptcode"].ToString();
                data(stardate, enddate, tablename, doctorname, patientid,accountdeptcode);
            }
        }
        protected void Store_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            string stardate = Request["stardate"].ToString();
            string enddate = Request["enddate"].ToString();
            string tablename = Request["tablename"].ToString();
            string doctorname = Request["doctorname"].ToString();
            string patientid = Request["patientid"].ToString();
            string accountdeptcode = Request["accountdeptcode"].ToString();
            data(stardate, enddate, tablename, doctorname, this.txt_SearchTxt.Text.Trim(),accountdeptcode);
        }
        private void data(string stardate, string enddate, string tablename, string doctorname,string patientid,string accountdeptcode)
        {
            Report report_dal = new Report();
            table = report_dal.GetDoctorDetail(stardate, enddate, tablename, doctorname, patientid,accountdeptcode);
            DataRow dr = table.NewRow();
            dr["PATIENT_NAME"] = "合计";
            dr["COSTS"] = table.Compute("Sum(COSTS)", "");
            dr["CHARGES"] = table.Compute("Sum(CHARGES)", "");
            table.Rows.Add(dr);
            for (int i = 0; i < table.Columns.Count; i++)
            {
                RecordField record = new RecordField();
                record = new RecordField(table.Columns[i].ColumnName, RecordFieldType.String);
                this.Store2.AddField(record);
                Column cl = new Column();
                if (table.Columns[i].ColumnName == "BILLING_DATE_TIME")
                    cl.Header = "时间";
                else if (table.Columns[i].ColumnName == "PATIENT_ID")
                    cl.Header = "病人ID";
                else if (table.Columns[i].ColumnName == "PATIENT_NAME")
                    cl.Header = "病人";
                else if (table.Columns[i].ColumnName == "CLASS_NAME")
                    cl.Header = "项目类型";
                else if (table.Columns[i].ColumnName == "ITEM_NAME")
                    cl.Header = "项目名称";
                else if (table.Columns[i].ColumnName == "AMOUNT")
                    cl.Header = "数量";
                else if (table.Columns[i].ColumnName == "ORDERED_BY")
                    cl.Header = "开单科室";
                else if (table.Columns[i].ColumnName == "PERFORMED_BY")
                    cl.Header = "执行科室";
                else if (table.Columns[i].ColumnName == "COSTS")
                    cl.Header = "计价";
                else if (table.Columns[i].ColumnName == "CHARGES")
                    cl.Header = "实收";
                cl.Sortable = true;
                cl.MenuDisabled = true;
                cl.DataIndex = table.Columns[i].ColumnName;
                


                this.GridPanel2.ColumnModel.Columns.Add(cl);
            }
            this.Store2.DataSource = table;
            this.Store2.DataBind();
        }
        protected void btnCancle_Click(object sender, EventArgs e)
        {
            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);

            scManager.AddScript("parent.Doctor_Detail.hide();");
        }
        protected void select_incomes(object sender, AjaxEventArgs e)
        {
            //string stardate = Request["stardate"].ToString();
            //string enddate = Request["enddate"].ToString();
            //string tablename = Request["tablename"].ToString();
            //string doctorname = Request["doctorname"].ToString();
            //string patientid = Request["patientid"].ToString();
            //data(stardate, enddate, tablename, doctorname, this.txt_SearchTxt.Text.Trim());
            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
            scManager.AddScript("RefreshData();");
        }
        protected void OutExcel(object sender, EventArgs e)
        {
            if (table != null)
            {
                string textname = "";
                ExportData ex = new ExportData();
                if (table.Rows.Count > 0)
                    textname ="收入明细报表";
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    if (table.Columns[i].ColumnName == "BILLING_DATE_TIME")
                    {
                        table.Columns[i].ColumnName = "时间";
                    }
                    else if (table.Columns[i].ColumnName == "PATIENT_ID")
                    {
                        table.Columns[i].ColumnName = "病人ID";
                    }
                    else if (table.Columns[i].ColumnName == "PATIENT_NAME")
                    {
                        table.Columns[i].ColumnName = "病人";
                    }
                    else if (table.Columns[i].ColumnName == "CLASS_NAME")
                    {
                        table.Columns[i].ColumnName = "项目类型";
                    }
                    else if (table.Columns[i].ColumnName == "ITEM_NAME")
                    {
                        table.Columns[i].ColumnName = "项目名称";
                    }
                    else if (table.Columns[i].ColumnName == "AMOUNT")
                    {
                        table.Columns[i].ColumnName = "数量";
                    }
                    else if (table.Columns[i].ColumnName == "ORDERED_BY")
                    {
                        table.Columns[i].ColumnName = "开单科室";
                    }
                    else if (table.Columns[i].ColumnName == "PERFORMED_BY")
                    {
                        table.Columns[i].ColumnName = "执行科室";
                    }
                    else if (table.Columns[i].ColumnName == "COSTS")
                    {
                        table.Columns[i].ColumnName = "计价";
                    }
                    else if (table.Columns[i].ColumnName == "CHARGES")
                    {
                        table.Columns[i].ColumnName = "实收";
                    }
                }
                //ex.ExportToLocal(table, this.Page, "xls", textname);
                this.outexcel(table, textname);
            }
        }
    }
}
