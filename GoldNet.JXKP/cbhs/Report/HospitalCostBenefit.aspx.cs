using System;
using Goldnet.Ext.Web;
using Goldnet.Dal;
using System.Data;
using GoldNet.Comm.ExportData;

namespace GoldNet.JXKP
{
    /// <summary>
    /// 全院成本核算效益
    /// </summary>
    public partial class HospitalCostBenefit : PageBase
    {
        Report dal_report = new Report();

        /// <summary>
        /// 初始化处理
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
                SetDict();
                string hospro = System.Configuration.ConfigurationManager.AppSettings["HOSPRO"];
                if (hospro == "1")
                {
                    this.GridPanel_Show.ColumnModel.Columns[4].Hidden = true;
                    this.GridPanel_Show.ColumnModel.Columns[5].Hidden = true;
                }
                //Column bonusColumn = new Column();
                //this.GridPanel_Show.ColumnModel.Columns.Add(bonusColumn);
            }
        }

        /// <summary>
        /// 初始化科室类型列表
        /// </summary>
        public void SetDict()
        {
            Goldnet.Dal.SYS_DEPT_DICT dal = new Goldnet.Dal.SYS_DEPT_DICT();
            DataTable table = dal.GetDeptType().Tables[0];
            if (table.Rows.Count > 0)
            {
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    this.deptType.Items.Add(new Goldnet.Ext.Web.ListItem(table.Rows[i]["ATTRIBUE"].ToString(), table.Rows[i]["id"].ToString()));
                }
            }
            this.deptType.SelectedIndex = 1;
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
            DataTable dt = dal_report.GetHospitalCostBenefit(GetBeginDate(), GetEndDate(), balance, this.deptType.SelectedItem.Value.ToString(), deptcode);
            if (dt != null)
            {
                SReport.DataSource = dt;
                SReport.DataBind();
                Session.Remove("HospitalCostBenefit");
                Session["HospitalCostBenefit"] = dt;
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
        /// EXCEL导出功能
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OutExcel(object sender, EventArgs e)
        {
            if (Session["HospitalCostBenefit"] != null)
            {
                ExportData ex = new ExportData();
                DataTable dt = (DataTable)Session["HospitalCostBenefit"];

                string hospro = System.Configuration.ConfigurationManager.AppSettings["HOSPRO"];
                if (hospro == "1")
                {
                    dt.Columns.Remove("ARMACCOUNTINCOMES");
                    dt.Columns.Remove("ARMYCOSTS");
                }

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (dt.Columns[i].ColumnName == "DEPT_NAME")
                    {
                        dt.Columns[i].ColumnName = "科室";
                    }
                    else if (dt.Columns[i].ColumnName == "FACACCOUNTINCOMES")
                    {
                        dt.Columns[i].ColumnName = "实际收入";
                    }
                    else if (dt.Columns[i].ColumnName == "COSTS")
                    {
                        dt.Columns[i].ColumnName = "实际成本";
                    }
                    else if (dt.Columns[i].ColumnName == "PROFIT")
                    {
                        dt.Columns[i].ColumnName = "实际收益";
                    }
                    else if (dt.Columns[i].ColumnName == "ARMACCOUNTINCOMES")
                    {
                        dt.Columns[i].ColumnName = "计价收入";
                    }
                    else if (dt.Columns[i].ColumnName == "ARMYCOSTS")
                    {
                        dt.Columns[i].ColumnName = "计价成本";
                    }
                    else if (dt.Columns[i].ColumnName == "DEPT_CODE")
                    {
                        dt.Columns[i].ColumnName = "科室代码";
                    }
                    else if (dt.Columns[i].ColumnName == "MEDINCOMES")
                    {
                        dt.Columns[i].ColumnName = "药品收益";
                    }
                }
                ex.ExportToLocal(dt, this.Page, "xls", "医疗收支,药品效益汇总表");
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

        /// <summary>
        /// 双击设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                //string deptcode = this.cbbdept.SelectedItem.Value.ToString() != "" ? "'" + this.cbbdept.SelectedItem.Value.ToString() + "'" : this.DeptFilter("");

                LoadConfig loadcfg = getLoadConfig("DeptCostAccount_detail_new.aspx");
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("stardate", GetBeginDate()));
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("enddate", GetEndDate()));
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("deptcode", deptcode));
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("balances", this.cbbType.SelectedItem.Value));
                showCenterSet(this.Cost_Detail, loadcfg);
            }
        }
    }
}
