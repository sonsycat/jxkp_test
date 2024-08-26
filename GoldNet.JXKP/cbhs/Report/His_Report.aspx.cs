using System;
using System.Collections.Generic;
using Goldnet.Ext.Web;
using Goldnet.Dal;
using System.Data;
using GoldNet.Comm.ExportData;

namespace GoldNet.JXKP
{
    public partial class His_Report : PageBase
    {
        DataTable table;
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
                cbbType.Value = 1;
                costtype.Value = 1;
                incomestype.Value = 0;

                data(DateTime.Now.AddMonths(-1).ToString("yyyy-MM") + "-01", DateTime.Now.AddMonths(-1).ToString("yyyy-MM") + "-01");
            }
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="stdate"></param>
        /// <param name="enddate"></param>
        private void data(string stdate, string enddate)
        {
            string deptcode = this.DeptFilter("");
            Report report_dal = new Report();
            string balance = this.cbbType.SelectedItem.Value == "" ? "0" : this.cbbType.SelectedItem.Value;
            string incomestype = this.incomestype.SelectedItem.Value == "" ? "0" : this.incomestype.SelectedItem.Value;
            string costtype = this.costtype.SelectedItem.Value == "" ? "1" : this.costtype.SelectedItem.Value;
            string depttype = this.deptType.SelectedItem.Value.ToString() == "" ? "0" : this.deptType.SelectedItem.Value.ToString();
            table = report_dal.GetHisreport(stdate, enddate, deptcode, balance, costtype, depttype, incomestype);

            for (int i = 0; i < table.Columns.Count; i++)
            {
                RecordField record = new RecordField();
                record = new RecordField(table.Columns[i].ColumnName, RecordFieldType.String);
                this.Store1.AddField(record);
                Column cl = new Column();
                cl.Header = table.Columns[i].ColumnName;
                cl.Sortable = false;
                cl.MenuDisabled = true;
                cl.Align = Alignment.Right;
                cl.DataIndex = table.Columns[i].ColumnName;
                TextField fils = new TextField();
                fils.ReadOnly = true;

                fils.ID = i.ToString();
                fils.SelectOnFocus = false;
                //fils.DecimalPrecision = 2;
                cl.Editor.Add(fils);
                if (cl.Header.Equals("科室名称") || cl.Header.Equals("DEPT_CODE"))
                {
                    cl.Align = Alignment.Left;
                }
                else
                {
                    cl.Renderer.Fn = "rmbMoney";
                }
                //if (cl.Header.Equals("DEPT_CODE") || cl.Header.Equals("DEPT_NAME"))
                //{
                //    cl.Align = Alignment.Right;
                //}

                this.GridPanel2.ColumnModel.Columns.Add(cl);
            }

            this.Store1.DataSource = table;
            this.Store1.DataBind();
        }

        /// <summary>
        /// 查询处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Query_Click(object sender, AjaxEventArgs e)
        {
            data(GetBeginDate(), GetEndDate());

            //Session.Remove("HisReport");
            //Session["HisReport"] = table;
        }

        /// <summary>
        /// EXCEL导出处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OutExcel(object sender, EventArgs e)
        {
            string deptcode = this.DeptFilter("");
            Report report_dal = new Report();
            string balance = this.cbbType.SelectedItem.Value == "" ? "1" : this.cbbType.SelectedItem.Value;
            string incomestype = this.incomestype.SelectedItem.Value == "" ? "0" : this.incomestype.SelectedItem.Value;
            string costtype = this.costtype.SelectedItem.Value == "" ? "1" : this.costtype.SelectedItem.Value;

            ExportData ex = new ExportData();
            DataTable dt = report_dal.GetHisreport(GetBeginDate(), GetEndDate(), deptcode, balance, costtype, this.deptType.SelectedItem.Value.ToString(), incomestype);
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                if (dt.Columns[i].ColumnName == "*")
                {
                    dt.Columns[i].ColumnName = "未知项目";
                }
                else if (dt.Columns[i].ColumnName == "DEPT_NAME")
                {
                    dt.Columns[i].ColumnName = "科室";
                }
            }
            this.outexcel(dt, "军卫收入统计表");

            //if (Session["HisReport"] != null)
            //{
            //    DataTable table2 = (DataTable)Session["HisReport"];
            //    this.outexcel(table2, "军卫收入统计表");
            //}
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
            string benginDate = year + "-" + month + "-01";
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
            string endDate = year + "-" + month + "-01";
            return endDate;
        }

        //反序列化得到客户端提交的gridpanel数据行      
        private Dictionary<string, string>[] GetSelectRow(AjaxEventArgs e)
        {
            string row = e.ExtraParams["Values"].ToString();
            Dictionary<string, string>[] selectRow = JSON.Deserialize<Dictionary<string, string>[]>(row);
            if (selectRow.Length <= 0) { return null; } else { return selectRow; }
        }

    }
}
