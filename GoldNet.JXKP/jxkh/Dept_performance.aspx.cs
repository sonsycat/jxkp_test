using System;
using System.Collections.Generic;
using Goldnet.Ext.Web;
using Goldnet.Dal;
using System.Data;
using GoldNet.Comm.ExportData;

namespace GoldNet.JXKP
{
    public partial class Dept_performance : PageBase
    {
        DataTable table;
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
                SMonth.DataSource = boundcomm.getMonth();
                SMonth.DataBind();
                cbbmonth.Value = month;
                data(DateTime.Now.AddMonths(-2).ToString("yyyyMM"), DateTime.Now.AddMonths(-2).ToString("yyyyMM"));
            }
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="stdate"></param>
        /// <param name="enddate"></param>
        private void data(string stdate,string endDate)
        {

            //string deptcode = this.DeptFilter("");
            string deptcode = this.DeptFilter("");
            Goldnet.Dal.Distribute report_dal = new Goldnet.Dal.Distribute();
            table = report_dal.GetBonusBSC(stdate,endDate, deptcode);
            for (int i = 0; i < table.Columns.Count; i++)
            {
                RecordField record = new RecordField();
                record = new RecordField(table.Columns[i].ColumnName, RecordFieldType.String);
                this.Store1.AddField(record);
                Column cl = new Column();
                cl.Header = table.Columns[i].ColumnName;
                cl.Sortable = false;
                cl.Align = Alignment.Right;
                cl.MenuDisabled = true;
                cl.DataIndex = table.Columns[i].ColumnName;
                TextField fils = new TextField();
                fils.ReadOnly = true;
                fils.ID = i.ToString();
                fils.SelectOnFocus = false;
                //fils.DecimalPrecision = 2;
                cl.Editor.Add(fils);
                if (cl.Header.Equals("DEPT_SNAP_DATE") || cl.Header.Equals("DEPT_CODE"))
                {
                    cl.Hidden = true;
                }
                else if (cl.Header.Equals("科室名称"))
                {
                    cl.Align = Alignment.Left;
                }
                else if (cl.Header.Equals("次均费用奖惩") || cl.Header.Equals("药占比奖惩") || cl.Header.Equals("卫材占比奖惩") || cl.Header.Equals("有效收入奖惩") || cl.Header.Equals("转诊率奖惩") || cl.Header.Equals("满意度奖惩") || cl.Header.Equals("医疗服务收入") || cl.Header.Equals("临床满意度") || cl.Header.Equals("报告及时率"))
                {
                    cl.Renderer.Fn = "rmbMoney";
                    cl.Renderer.Fn = "namecolor";
                }
                else
                {
                    cl.Renderer.Fn = "rmbMoney";
                }

                this.GridPanel2.ColumnModel.Columns.Add(cl);
            }

            this.Store1.DataSource = table;
            this.Store1.DataBind();
            this.GridPanel2.Reconfigure();  //清除列内容重新加载
        }

        protected void Btn_Query_Click(object sender, AjaxEventArgs e)
        {
            data(GetBeginDate(),GetEndDate());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OutExcel(object sender, EventArgs e)
        {
            string deptcode = this.DeptFilter("");
            //Report report_dal = new Report();
            Goldnet.Dal.Distribute report_dal = new Goldnet.Dal.Distribute();
            DataTable dt = report_dal.GetBonusBSC(GetBeginDate(), GetEndDate(), deptcode);
            // DataTable dt = report_dal.GETperformance(GetBeginDate());
            ExportData ex = new ExportData();
            this.outexcel(dt, "平衡记分卡汇总");
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
            string benginDate = year + month;
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
            string endDate = year + "-" + month;
            return endDate;
        }


    }
}