using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Goldnet.Ext.Web;
using Goldnet.Dal;
using System.Data;
using GoldNet.Comm;
using GoldNet.Model;
using GoldNet.Comm.ExportData;
namespace GoldNet.JXKP
{
    public partial class Dept_MedPro : PageBase
    {
        private Report report_dal = new Report();
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
                Query();
            }
        }

        private void Query()
        {
            string balance = cbbType.SelectedItem.Value;

            DataTable table = report_dal.GetDeptMedPro(balance,this.cbbYear.SelectedItem.Value.ToString()+this.cbbmonth.SelectedItem.Value.ToString(),this.ccbYearTo.SelectedItem.Value.ToString()+this.ccbMonthTo.SelectedItem.Value.ToString());
            DataRow dr = table.NewRow();
            dr["ACCOUNT_DEPT_NAME"] = "合计";
            dr["MZ_CHARGES"] = table.Compute("Sum(MZ_CHARGES)","");
            dr["MZY_CHARGES"] = table.Compute("Sum(MZY_CHARGES)", "");
            dr["MZY_CHARGES_RAL"] = "0";
            if (table.Compute("Sum(MZY_CHARGES)", "").ToString() != ""&&table.Compute("Sum(MZY_CHARGES)", "").ToString() != "0")
            {
                dr["MZY_CHARGES_RAL"] = Math.Round(double.Parse(table.Compute("Sum(MZY_CHARGES)/Sum(MZ_CHARGES)", "").ToString()) * 100, 2).ToString()+"%";
            }
            dr["ZY_CHARGES"] = table.Compute("Sum(ZY_CHARGES)", "");
            dr["ZYY_CHARGES"] = table.Compute("Sum(ZYY_CHARGES)", "");
            dr["ZYY_CHARGES_RAL"] = "0";
            if (table.Compute("Sum(ZYY_CHARGES)", "").ToString() != "" && table.Compute("Sum(ZYY_CHARGES)", "").ToString() != "0")
            {
                dr["ZYY_CHARGES_RAL"] = Math.Round(double.Parse(table.Compute("Sum(ZYY_CHARGES)/Sum(ZY_CHARGES)", "").ToString()) * 100, 2).ToString() + "%";
            }
            dr["MZ_INCOMES"] = table.Compute("Sum(MZ_INCOMES)", "");
            dr["MZY_INCOMES"] = table.Compute("Sum(MZY_INCOMES)", "");
            dr["MZY_INCOMES_RAL"] = "0";
            if (table.Compute("Sum(MZY_INCOMES)", "").ToString() != "" && table.Compute("Sum(MZY_INCOMES)", "").ToString() != "0")
            {
                dr["MZY_INCOMES_RAL"] = Math.Round(double.Parse(table.Compute("Sum(MZY_INCOMES)/Sum(MZ_INCOMES)", "").ToString()) * 100, 2).ToString() + "%";
            }
            dr["ZY_INCOMES"] = table.Compute("Sum(ZY_INCOMES)", "");
            dr["ZYY_INCOMES"] = table.Compute("Sum(ZYY_INCOMES)", "");
            dr["ZYY_INCOMES_RAL"] = "0";
            if (table.Compute("Sum(ZYY_INCOMES)", "").ToString() != "" && table.Compute("Sum(ZYY_INCOMES)", "").ToString() != "0")
            {
                dr["ZYY_INCOMES_RAL"] = Math.Round(double.Parse(table.Compute("Sum(ZYY_INCOMES)/Sum(ZY_INCOMES)", "").ToString()) * 100, 2).ToString() + "%";
            }
            table.Rows.Add(dr);
            SReport.DataSource = table;
            SReport.DataBind();
        }
        protected void Btn_Query_Click(object sender, AjaxEventArgs e)
        {
            Query();
        }
        protected void OutExcel(object sender, EventArgs e)
        {
            if (Session["Account_Subject_Income"] != null)
            {
                ExportData ex = new ExportData();
                DataTable dt = (DataTable)Session["Account_Subject_Income"];

                string hospro = System.Configuration.ConfigurationManager.AppSettings["HOSPRO"];
                if (hospro == "1")
                {
                    dt.Columns.Remove("ARMACCOUNTINCOMES");
                    dt.Columns.Remove("CVALATIONINCOMES");
                    dt.Columns.Remove("HVALATIONINCOMES");
                }

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (dt.Columns[i].ColumnName == "CLASS_NAME")
                    {
                        dt.Columns[i].ColumnName = "核算科目";
                    }
                    else if (dt.Columns[i].ColumnName == "ACCOUNTINCOMES")
                    {
                        dt.Columns[i].ColumnName = "总收入";
                    }
                    else if (dt.Columns[i].ColumnName == "FACACCOUNTINCOMES")
                    {
                        dt.Columns[i].ColumnName = "实际收入小计";
                    }
                    else if (dt.Columns[i].ColumnName == "CREALINCOMES")
                    {
                        dt.Columns[i].ColumnName = "实际收入门诊";
                    }
                    else if (dt.Columns[i].ColumnName == "HREALINCOMES")
                    {
                        dt.Columns[i].ColumnName = "实际收入住院";
                    }
                    else if (dt.Columns[i].ColumnName == "ARMACCOUNTINCOMES")
                    {
                        dt.Columns[i].ColumnName = "计价收入小计";
                    }
                    else if (dt.Columns[i].ColumnName == "CVALATIONINCOMES")
                    {
                        dt.Columns[i].ColumnName = "计价收入门诊";
                    }
                    else if (dt.Columns[i].ColumnName == "HVALATIONINCOMES")
                    {
                        dt.Columns[i].ColumnName = "计价收入住院";
                    }
                    else if (dt.Columns[i].ColumnName == "CLASS_CODE")
                    {
                        dt.Columns[i].ColumnName = "项目代码";
                    }
                    else if (dt.Columns[i].ColumnName == "ID")
                    {
                        dt.Columns[i].ColumnName = "顺序";
                    }
                }
                ex.ExportToLocal(dt, this.Page, "xls", "核算科目医疗收入统计报表");
            }
        }
       

    }
}
