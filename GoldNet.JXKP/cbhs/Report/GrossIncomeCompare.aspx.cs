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
    public partial class GrossIncomeCompare : PageBase
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
                //cbbType.SelectedItem.Text = "收付实现";
            }
        }

        protected void Btn_Query_Click(object sender, AjaxEventArgs e)
        {
            if (cbbYear.SelectedItem.Value.ToString() != ccbYearTo.SelectedItem.Value.ToString())
            {
                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                {
                    Title = "提示",
                    Message = "时间范围必须为同一年度！",
                    Buttons = MessageBox.Button.OK,
                    Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                });
                return;
            }
            string balance = cbbType.SelectedItem.Value;
            string deptcode = this.DeptFilter("");
           
            string hospro = System.Configuration.ConfigurationManager.AppSettings["HOSPRO"];
            DataTable dt = report_dal.GetGrossIncomeCompare(GetBeginDate(), GetEndDate(), balance, deptcode, hospro);

            DataRow dr = dt.NewRow();
            dr["DEPT_NAME"] = "合计";
            dr["MZ1"] = dt.Compute("Sum(MZ1)", "");
            dr["ZY1"] = dt.Compute("Sum(ZY1)", "");
            dr["TOTAL1"] = dt.Compute("Sum(TOTAL1)", "");
            dr["MZ2"] = dt.Compute("Sum(MZ2)", "");
            dr["ZY2"] = dt.Compute("Sum(ZY2)", "");
            dr["TOTAL2"] = dt.Compute("Sum(TOTAL2)", "");
            dr["MZ3"] = dt.Compute("Sum(MZ3)", "");
            dr["ZY3"] = dt.Compute("Sum(ZY3)", "");
            dr["TOTAL3"] = dt.Compute("Sum(TOTAL3)", "");
            dr["TOTALADD"] = dt.Compute("Sum(TOTALADD)", "");
            dt.Rows.Add(dr);

            if (dt != null)
            {
                SReport.DataSource = dt;
                SReport.DataBind();
                Session.Remove("GrossIncomeCompare");
                Session["GrossIncomeCompare"] = dt;
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
            if (Session["GrossIncomeCompare"] != null)
            {
                ExportData ex = new ExportData();
                DataTable dt = (DataTable)Session["GrossIncomeCompare"];

                string hospro = System.Configuration.ConfigurationManager.AppSettings["HOSPRO"];
               

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (dt.Columns[i].ColumnName == "DEPT_TYPE")
                    {
                        dt.Columns[i].ColumnName = "科室类别";
                    }
                    else if (dt.Columns[i].ColumnName == "ID")
                    {
                        dt.Columns[i].ColumnName = "排序";
                    }
                    else if (dt.Columns[i].ColumnName == "DEPT_NAME")
                    {
                        dt.Columns[i].ColumnName = "科室";
                    }
                    else if (dt.Columns[i].ColumnName == "MZ1")
                    {
                        dt.Columns[i].ColumnName = "本期门诊";
                    }
                    else if (dt.Columns[i].ColumnName == "ZY1")
                    {
                        dt.Columns[i].ColumnName = "本期住院";
                    }
                    else if (dt.Columns[i].ColumnName == "TOTAL1")
                    {
                        dt.Columns[i].ColumnName = "本期合计";
                    }
                    else if (dt.Columns[i].ColumnName == "MZ2")
                    {
                        dt.Columns[i].ColumnName = "同期门诊";
                    }
                    else if (dt.Columns[i].ColumnName == "ZY2")
                    {
                        dt.Columns[i].ColumnName = "同期住院";
                    }
                    else if (dt.Columns[i].ColumnName == "TOTAL2")
                    {
                        dt.Columns[i].ColumnName = "同期合计";
                    }
                    else if (dt.Columns[i].ColumnName == "MZ3")
                    {
                        dt.Columns[i].ColumnName = "同比增长门诊";
                    }
                    else if (dt.Columns[i].ColumnName == "ZY3")
                    {
                        dt.Columns[i].ColumnName = "同比增长住院";
                    }
                    else if (dt.Columns[i].ColumnName == "TOTAL3")
                    {
                        dt.Columns[i].ColumnName = "同比增长总收入";
                    }
                    else if (dt.Columns[i].ColumnName == "TOTALADD")
                    {
                        dt.Columns[i].ColumnName = "同比增长额";
                    }
                }
                ex.ExportToLocal(dt, this.Page, "xls", "同期毛收入对比统计表");
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
            string benginDate = year+"-"+ month + "-01";
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
            string endDate = year+"-"+ month + "-01";
            return endDate;
        }
    }
}
