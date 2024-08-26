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
    /// <summary>
    /// 项目区间分布情况
    /// </summary>
    public partial class SimpleIncomeDetail :PageBase
    {
        Report dal_report = new Report();
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
                SetStoreProxy();

                cbbType.SelectedItem.Value = "0";
                //cbbType.SelectedItem.Text = "收付实现";
            }
        }
        private void SetStoreProxy()
        {

            //查找科室信息
            //HttpProxy pro = new HttpProxy();
            //pro.Method = HttpMethod.POST;
            //pro.Url = "../WebService/ReckItems.ashx";
            //this.SCostitem.Proxy.Add(pro);
            //JsonReader jr = new JsonReader();
            //jr.ReaderID = "CLASS_CODE";
            //jr.Root = "itemlist";
            //jr.TotalProperty = "totalitems";
            //RecordField rf = new RecordField();
            //rf.Name = "CLASS_CODE";
            //jr.Fields.Add(rf);
            //RecordField rfn = new RecordField();
            //rfn.Name = "CLASS_NAME";
            //jr.Fields.Add(rfn);
            //this.SCostitem.Reader.Add(jr);

            DataTable table = dal_report.GetINcomeType();
            if (table.Rows.Count > 0)
            {
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    this.cbb_ReckItem.Items.Add(new Goldnet.Ext.Web.ListItem(table.Rows[i]["INCOM_TYPE_NAME"].ToString(), table.Rows[i]["INCOM_TYPE_CODE"].ToString()));
                }
            }
            this.cbb_ReckItem.SelectedIndex = 1;
        }

        protected void Btn_Query_Click(object sender, AjaxEventArgs e)
        {
            string begin = cbbYear.SelectedItem.Value.ToString() + "-" + cbbmonth.SelectedItem.Value.ToString() + "-01";
            string end = ccbYearTo.SelectedItem.Value.ToString() + "-" + ccbMonthTo.SelectedItem.Value.ToString() + "-01";
            DateTime dtBegin = Convert.ToDateTime(begin);
            DateTime dtEnd = Convert.ToDateTime(end);
            if (dtEnd.CompareTo(dtBegin) < 0)
            {
                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                {
                    Title = "提示",
                    Message = "开始月份大于结束月份",
                    Buttons = MessageBox.Button.OK,
                    Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                });
                return;
            }
            int months = Convert.ToInt32((Convert.ToDateTime(dtEnd).Year - Convert.ToDateTime(dtBegin).Year) * 12 + Convert.ToDateTime(dtEnd).Month - Convert.ToDateTime(dtBegin).Month);

            string balance = cbbType.SelectedItem.Value;
            string deptcode = this.DeptFilter("");
            string reck = cbb_ReckItem.SelectedItem.Value;
            string hospro = System.Configuration.ConfigurationManager.AppSettings["HOSPRO"];
            DataTable dt = dal_report.GetSimpleIncomeDetail(dtBegin, balance, deptcode, reck, months + 1, hospro);
            if (dt != null)
            {
                SReport.RemoveFields();
                GridPanel_Show.Reconfigure();
                GridPanel_Show.ColumnModel.Columns.Clear();
                GoldNet.JXKP.cbhs.Report.BuildControl bc = new GoldNet.JXKP.cbhs.Report.BuildControl();
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    
                        bc.AddRecord(dt.Columns[i].ColumnName, SReport);
                        ExtColumn Column = new ExtColumn();
                        Column.ColumnID = dt.Columns[i].ColumnName;
                        Column.Header = "<div style='text-align:center;'>" + dt.Columns[i].ColumnName + "</div>";
                        if (dt.Columns[i].ColumnName == "DEPT_CODE")
                        {
                            Column.Header = "<div style='text-align:center;'>" + "科室代码" + "</div>";
                        }
                        Column.Align = Alignment.Left;
                        if (dt.Columns[i].ColumnName != "科室" && dt.Columns[i].ColumnName != "DEPT_CODE")
                        {
                            Column.Renderer.Fn = "rmbMoney";
                            Column.Align = Alignment.Right;
                        }
                        Column.DataIndex = dt.Columns[i].ColumnName;
                        Column.MenuDisabled = true;
                        Column.Width = 120;
                        GridPanel_Show.ColumnModel.Columns.Add(Column);
                        GridPanel_Show.AddColumn(Column);
                    
                }

                SReport.DataSource = dt;
                SReport.DataBind();
                Session.Remove("SimpleIncomeDetail");
                Session["SimpleIncomeDetail"] = dt;
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
        //反序列化得到客户端提交的gridpanel数据行      
        private Dictionary<string, string>[] GetSelectRow(AjaxEventArgs e)
        {
            string row = e.ExtraParams["Values"].ToString();
            Dictionary<string, string>[] selectRow = JSON.Deserialize<Dictionary<string, string>[]>(row);
            if (selectRow.Length <= 0) { return null; } else { return selectRow; }
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
                string begin = cbbYear.SelectedItem.Value.ToString() + "-" + cbbmonth.SelectedItem.Value.ToString() + "-01";
                string end = ccbYearTo.SelectedItem.Value.ToString() + "-" + ccbMonthTo.SelectedItem.Value.ToString() + "-01";
                DateTime dtBegin = Convert.ToDateTime(begin);
                DateTime dtEnd = Convert.ToDateTime(end);
                if (dtEnd.CompareTo(dtBegin) < 0)
                {
                    Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                    {
                        Title = "提示",
                        Message = "开始月份大于结束月份",
                        Buttons = MessageBox.Button.OK,
                        Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                    });
                    return;
                }
                int months = Convert.ToInt32((Convert.ToDateTime(dtEnd).Year - Convert.ToDateTime(dtBegin).Year) * 12 + Convert.ToDateTime(dtEnd).Month - Convert.ToDateTime(dtBegin).Month);

                string balance = cbbType.SelectedItem.Value;
                string deptcode = sm.SelectedRow.RecordID;
                string reck = cbb_ReckItem.SelectedItem.Value;
                string hospro = System.Configuration.ConfigurationManager.AppSettings["HOSPRO"];
                LoadConfig loadcfg = getLoadConfig("SimpleIncome_Dept_Detail.aspx");
               // DataTable dt = dal_report.GetSimpleIncomeDetail(dtBegin, balance, deptcode, reck, months + 1, hospro);

                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("dtBegin", dtBegin.ToString("yyyy-MM-dd")));
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("balance", balance));
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("deptcode", deptcode));
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("reck", reck));
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("months", (months + 1).ToString()));
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("hospro", hospro));

                showCenterSet(this.Income_Detail, loadcfg);
            }
        }
        protected void OutExcel(object sender, EventArgs e)
        {
            if (Session["SimpleIncomeDetail"] != null)
            {
                ExportData ex = new ExportData();
                DataTable dt = (DataTable)Session["SimpleIncomeDetail"];
                //dt.Columns.Remove("DEPT_CODE");
                ex.ExportToLocal(dt, this.Page, "xls", "单项收入构成明细表");
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
