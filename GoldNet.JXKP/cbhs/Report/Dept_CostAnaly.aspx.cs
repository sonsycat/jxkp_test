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
    public partial class Dept_CostAnaly : PageBase
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

            DataTable table = report_dal.GetDeptCostAnaly(balance, this.cbbYear.SelectedItem.Value.ToString() + this.cbbmonth.SelectedItem.Value.ToString(), this.ccbYearTo.SelectedItem.Value.ToString() + this.ccbMonthTo.SelectedItem.Value.ToString());
            DataRow dr = table.NewRow();
            dr["DEPT_NAME_SECOND"] = "合计";
            dr["MZ_CHARGES"] = table.Compute("Sum(MZ_CHARGES)", "");
            dr["DFMJZRC"] = table.Compute("Sum(DFMJZRC)", "");
            dr["MZ_CHARGES_RAL"] = "0";
            if (table.Compute("Sum(DFMJZRC)", "").ToString() != "" && table.Compute("Sum(DFMJZRC)", "").ToString() != "0")
            {
                dr["MZ_CHARGES_RAL"] = Math.Round(double.Parse(table.Compute("Sum(MZ_CHARGES)/Sum(DFMJZRC)", "").ToString()), 2);
            }
            dr["ZY_CHARGES"] = table.Compute("Sum(ZY_CHARGES)", "");
            dr["DFZYRC"] = table.Compute("Sum(DFZYRC)", "");
            dr["ZY_CHARGES_RAL"] = "0";
            if (table.Compute("Sum(DFZYRC)", "").ToString() != "" && table.Compute("Sum(DFZYRC)", "").ToString() != "0")
            {
                dr["ZY_CHARGES_RAL"] = Math.Round(double.Parse(table.Compute("Sum(ZY_CHARGES)/Sum(DFZYRC)", "").ToString()), 2);
            }
            dr["MZ_INCOMES"] = table.Compute("Sum(MZ_INCOMES)", "");
            dr["JMMJZRC"] = table.Compute("Sum(JMMJZRC)", "");
            dr["MZ_INCOMES_RAL"] = "0";
            if (table.Compute("Sum(JMMJZRC)", "").ToString() != "" && table.Compute("Sum(JMMJZRC)", "").ToString() != "0")
            {
                dr["MZ_INCOMES_RAL"] = Math.Round(double.Parse(table.Compute("Sum(MZ_INCOMES)/Sum(JMMJZRC)", "").ToString()), 2);
            }
            dr["ZY_INCOMES"] = table.Compute("Sum(ZY_INCOMES)", "");
            dr["JMZYRC"] = table.Compute("Sum(JMZYRC)", "");
            dr["ZY_INCOMES_RAL"] = "0";
            if (table.Compute("Sum(JMZYRC)", "").ToString() != "" && table.Compute("Sum(JMZYRC)", "").ToString() != "0")
            {
                dr["ZY_INCOMES_RAL"] = Math.Round(double.Parse(table.Compute("Sum(ZY_INCOMES)/Sum(JMZYRC)", "").ToString()), 2);
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
