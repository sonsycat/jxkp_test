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
    public partial class DeptIncome_Detail : PageBase
    {
        DataTable table = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
               
                string stardate = Request["stardate"].ToString();
                string enddate = Request["enddate"].ToString();
                string tablename = Request["tablename"].ToString();
                string deptcode = Request["deptcode"].ToString();
                string groupbydept = Request["groupbydept"].ToString();
                string incomescolums = Request["incomescolums"].ToString();
                string accountdept = Request["accountdept"].ToString();

                data(stardate, enddate, tablename, deptcode, groupbydept, incomescolums, accountdept,"");
            }
        }
        protected void Store_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            string stardate = Request["stardate"].ToString();
            string enddate = Request["enddate"].ToString();
            string tablename = Request["tablename"].ToString();
            string deptcode = Request["deptcode"].ToString();
            string groupbydept = Request["groupbydept"].ToString();
            string incomescolums = Request["incomescolums"].ToString();
            string accountdept = Request["accountdept"].ToString();

            data(stardate, enddate, tablename, deptcode, groupbydept, incomescolums, accountdept, this.txt_SearchTxt.Text.Trim());
        }
        private void data(string stardate, string enddate, string tablename, string deptcode, string groupbydept, string incomescolums, string accountdept,string doctorname)
        {
            Report report_dal = new Report();
            table = report_dal.GetIncomDetail(stardate, enddate, tablename, deptcode, groupbydept, incomescolums, accountdept, doctorname);
            DataRow dr = table.NewRow();
            dr["DEPT_NAME"] = "合计";
            dr["COSTS"] = table.Compute("Sum(COSTS)", "");
            
            table.Rows.Add(dr);
            for (int i = 0; i < table.Columns.Count; i++)
            {
                RecordField record = new RecordField();
                record = new RecordField(table.Columns[i].ColumnName, RecordFieldType.String);
                this.Store2.AddField(record);
                Column cl = new Column();
                if (table.Columns[i].ColumnName == "DEPT_NAME")
                    cl.Header = "科室名称";
                else if (table.Columns[i].ColumnName == "ORDERED_BY_DOCTOR")
                    cl.Header = "医生";
                else if (table.Columns[i].ColumnName == "ST_DATE")
                    cl.Header = "时间";
                else if (table.Columns[i].ColumnName == "ITEM_NAME")
                    cl.Header = "项目名称";
                else if (table.Columns[i].ColumnName == "COSTS")
                    cl.Header = "收入";
                else if (table.Columns[i].ColumnName == "COUNT_UNITY")
                    cl.Header = "开单科室";
                else if (table.Columns[i].ColumnName == "PATIENT_ID")
                    cl.Header = "病人ID";
                else if (table.Columns[i].ColumnName == "PATIENT_NAME")
                    cl.Header = "病人姓名";
                cl.Sortable = true;
                cl.MenuDisabled = true;
                cl.DataIndex = table.Columns[i].ColumnName;
                //NumberField fils = new NumberField();
                //fils.ID = i.ToString();
                //fils.SelectOnFocus = true;
                //fils.DecimalPrecision = 2;
                //cl.Editor.Add(fils);
               

                this.GridPanel2.ColumnModel.Columns.Add(cl);
            }
            this.Store2.DataSource = table;
            this.Store2.DataBind();
        }
        protected void btnCancle_Click(object sender, EventArgs e)
        {
            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);

            scManager.AddScript("parent.Income_Detail.hide();");
        }
        protected void select_incomes(object sender, AjaxEventArgs e)
        {
            //string stardate = Request["stardate"].ToString();
            //string enddate = Request["enddate"].ToString();
            //string tablename = Request["tablename"].ToString();
            //string deptcode = Request["deptcode"].ToString();
            //string groupbydept = Request["groupbydept"].ToString();
            //string incomescolums = Request["incomescolums"].ToString();
            //string accountdept = Request["accountdept"].ToString();

            //data(stardate, enddate, tablename, deptcode, groupbydept, incomescolums, accountdept, this.txt_SearchTxt.Text.Trim());
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
                    textname = table.Rows[0]["DEPT_NAME"].ToString()+"--收入报表";
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    if (table.Columns[i].ColumnName == "DEPT_NAME")
                    {
                        table.Columns[i].ColumnName = "科室名称";
                    }
                    else if (table.Columns[i].ColumnName == "ORDERED_BY_DOCTOR")
                    {
                        table.Columns[i].ColumnName = "开单医生";
                    }
                    else if (table.Columns[i].ColumnName == "ST_DATE")
                    {
                        table.Columns[i].ColumnName = "时间";
                    }
                    else if (table.Columns[i].ColumnName == "ITEM_NAME")
                    {
                        table.Columns[i].ColumnName = "项目名称";
                    }
                    else if (table.Columns[i].ColumnName == "COSTS")
                    {
                        table.Columns[i].ColumnName = "收入";
                    }
                    else if (table.Columns[i].ColumnName == "COUNT_UNITY")
                    {
                        table.Columns[i].ColumnName = "开单科室";
                    }
                }
                //ex.ExportToLocal(table, this.Page, "xls", textname);
                this.outexcel(table, textname);
            }
        }
    }
}
