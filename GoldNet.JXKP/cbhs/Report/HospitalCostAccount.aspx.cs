using System;
using Goldnet.Ext.Web;
using Goldnet.Dal;
using System.Data;
using GoldNet.Comm.ExportData;

namespace GoldNet.JXKP
{
    public partial class HospitalCostAccount :PageBase
    {
        Report dal_report = new Report();

        /// <summary>
        /// 全院成本核算信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                cbbType.Value = 0;

                string hospro = System.Configuration.ConfigurationManager.AppSettings["HOSPRO"];
                if (hospro == "1")
                {
                    this.GridPanel_Show.ColumnModel.Columns[6].Hidden = true;
                    this.GridPanel_Show.ColumnModel.Columns[7].Hidden = true;
                    this.GridPanel_Show.ColumnModel.Columns[8].Hidden = true;
                    this.GridPanel_Show.ColumnModel.Columns[9].Hidden = true;
                }
              
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Query_Click(object sender, AjaxEventArgs e)
        {
            string balance = cbbType.SelectedItem.Value;
            string deptcode = this.DeptFilter("");
            string hospro = System.Configuration.ConfigurationManager.AppSettings["HOSPRO"];
            DataTable dt = dal_report.GetCostAccountTotal(GetBeginDate(), GetEndDate(), balance, deptcode);
            if (dt != null)
            {
                SReport.DataSource = dt;
                SReport.DataBind();
                Session.Remove("HospitalCostAccount");
                Session["HospitalCostAccount"] = dt;
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
            if (Session["HospitalCostAccount"] != null)
            {
                ExportData ex = new ExportData();
                DataTable dt = (DataTable)Session["HospitalCostAccount"];

                string hospro = System.Configuration.ConfigurationManager.AppSettings["HOSPRO"];
                if (hospro == "1")
                {
                    dt.Columns.Remove("CVALATIONINCOMES");
                    dt.Columns.Remove("HVALATIONINCOMES");
                    dt.Columns.Remove("ARMACCOUNTINCOMES");
                    dt.Columns.Remove("ARMYCOSTS");
                }

             
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (dt.Columns[i].ColumnName == "DEPT_NAME")
                    {
                        dt.Columns[i].ColumnName = "科室";
                    }                   
                    else if (dt.Columns[i].ColumnName == "CREALINCOMES")
                    {
                        dt.Columns[i].ColumnName = "实际收入门诊";
                    }
                    else if (dt.Columns[i].ColumnName == "HREALINCOMES")
                    {
                        dt.Columns[i].ColumnName = "实际收入住院";
                    }
                    else if (dt.Columns[i].ColumnName == "FACACCOUNTINCOMES")
                    {
                        dt.Columns[i].ColumnName = "实际收入合计";
                    }
                    else if (dt.Columns[i].ColumnName == "COSTS")
                    {
                        dt.Columns[i].ColumnName = "实际医疗成本";
                    }
                    else if (dt.Columns[i].ColumnName == "PROFIT")
                    {
                        dt.Columns[i].ColumnName = "实际医疗收益";
                    }
                    else if (dt.Columns[i].ColumnName == "CVALATIONINCOMES")
                    {
                        dt.Columns[i].ColumnName = "计价收入门诊";
                    }
                    else if (dt.Columns[i].ColumnName == "HVALATIONINCOMES")
                    {
                        dt.Columns[i].ColumnName = "计价收入住院";
                    }
                    else if (dt.Columns[i].ColumnName == "ARMACCOUNTINCOMES")
                    {
                        dt.Columns[i].ColumnName = "计价收入合计";
                    }
                    else if (dt.Columns[i].ColumnName == "ARMYCOSTS")
                    {
                        dt.Columns[i].ColumnName = "计价医疗成本";
                    }
                    else if (dt.Columns[i].ColumnName == "DEPT_CODE")
                    {
                        dt.Columns[i].ColumnName = "科室代码";
                    }
                }
                ex.ExportToLocal(dt, this.Page, "xls", "全院成本核算信息汇总表");
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
