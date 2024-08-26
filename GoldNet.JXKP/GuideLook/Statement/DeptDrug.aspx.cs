using System;
using System.Collections;
using System.Configuration;
using System.Data;
using Goldnet.Ext.Web;
using Goldnet.Dal;
using System.Collections.Generic;
using GoldNet.Comm.ExportData;

namespace GoldNet.JXKP.GuideLook.Statement
{
    public partial class DeptDrug : PageBase
    {
        private StatementDal dal = new StatementDal();
        private static DataTable Currdt = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            //检查是否已经登录，否则停止
            if (Session["CURRENTSTAFF"] == null)
            {
                Response.End();
                return;
            }

            if (!Ext.IsAjaxRequest)
            {
                setTimeContrl();
            }
        }

        /// <summary>
        /// 初始化时间空件
        /// </summary>
        private void setTimeContrl()
        {
            //开始时间
            for (int i = 0; i < 10; i++)
            {
                int years = System.DateTime.Now.Year - i;
                this.Comb_StartYear.Items.Add(new Goldnet.Ext.Web.ListItem(years.ToString(), years.ToString()));
            }
            for (int i = 1; i <= 12; i++)
            {
                this.Comb_StartMonth.Items.Add(new Goldnet.Ext.Web.ListItem(i.ToString(), i.ToString()));
            }
            this.Comb_StartMonth.SelectedIndex = 0;

            for (int i = 1; i <= 31; i++)
            {
                this.Comb_StartDate.Items.Add(new Goldnet.Ext.Web.ListItem(i.ToString(), i.ToString()));
            }
            this.Comb_StartDate.SelectedIndex = 0;

            //结束时间
            for (int i = 0; i < 10; i++)
            {
                int years = System.DateTime.Now.Year - i;
                this.Comb_EndYear.Items.Add(new Goldnet.Ext.Web.ListItem(years.ToString(), years.ToString()));
            }
            for (int i = 1; i <= 12; i++)
            {
                this.Comb_EndMonth.Items.Add(new Goldnet.Ext.Web.ListItem(i.ToString(), i.ToString()));
            }
            this.Comb_EndMonth.SelectedIndex = DateTime.Now.Month - 1;

            for (int i = 1; i <= 31; i++)
            {
                this.Comb_EndDate.Items.Add(new Goldnet.Ext.Web.ListItem(i.ToString(), i.ToString()));
            }
            this.Comb_EndDate.SelectedIndex = DateTime.Now.Day - 1;

            this.cboType.SelectedIndex = 0;

        }

        /// <summary>
        /// 查询按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Data_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {

            string dd1 = DateTimeConvert(this.Comb_StartYear.SelectedItem.Value, this.Comb_StartMonth.SelectedItem.Value, this.Comb_StartDate, 2);

            string dd2 = DateTimeConvert(this.Comb_EndYear.SelectedItem.Value, this.Comb_EndMonth.SelectedItem.Value, this.Comb_EndDate, 1);

            DateTime time1 = default(DateTime);
            DateTime time2 = default(DateTime);
            if ((!DateTime.TryParse(dd1, out time1)) || (!DateTime.TryParse(dd2, out time2)))
            {
                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                {
                    Title = "信息提示",
                    Message = "日期不正确,请重新选择条件",
                    Buttons = MessageBox.Button.OK,
                    Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                });
                return;
            }
            if (dd1.CompareTo(dd2) > 0)
            {
                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                {
                    Title = "信息提示",
                    Message = "日期范围不正确,请重新选择条件",
                    Buttons = MessageBox.Button.OK,
                    Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                });
                return;
            }

            this.Store1.DataSource = dal.getDeptDrug(dd1,dd2,this.DeptCodeCombo.SelectedItem.Value,this.DeptFilter(""),this.cboType.SelectedItem.Value).Tables[0];
            this.Store1.DataBind();
        }


        /// <summary>
        /// 查询按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void QueryPerDrug(object sender, AjaxEventArgs e)
        {
            string col = e.ExtraParams["col"].ToString();
            string dd1 = DateTimeConvert(this.Comb_StartYear.SelectedItem.Value, this.Comb_StartMonth.SelectedItem.Value, this.Comb_StartDate, 2);

            string dd2 = DateTimeConvert(this.Comb_EndYear.SelectedItem.Value, this.Comb_EndMonth.SelectedItem.Value, this.Comb_EndDate, 1);
            //定义一个HashTable,将前台编辑按钮所选中的行数据复制到定义的HashTable对象selectRow中
            Dictionary<string, string>[] selectRow = GetSelectRow(e);
            //判断如果selectRow里为null，则返回空
            if (selectRow == null || selectRow[0]["DEPT_CODE"] == "999999" || Convert.ToInt32(col) < 3)
            {
                return;
            }
            //PageResponse(this.getPageResponseUrl("DeptDrugInfo.aspx", dd1, dd2, col, selectRow[0]["DEPT_CODE"].ToString(), this.cbxType.SelectedItem.Value), "药品明细---" + selectRow[0]["DEPT_NAME"].ToString());
        }

        //反序列化得到客户端提交的gridpanel数据行
        private Dictionary<string, string>[] GetSelectRow(AjaxEventArgs e)
        {
            string row = e.ExtraParams["value"].ToString();
            Dictionary<string, string>[] selectRow = JSON.Deserialize<Dictionary<string, string>[]>(row);
            if (selectRow.Length <= 0)
            {
                return null;
            }
            else
            {
                return selectRow;
            }
        }



        protected void OutExcel(object sender, EventArgs e)
        {
            ExportData ex = new ExportData();
            DataTable ExcelTable = new DataTable();
            ExcelTable = Currdt;
            ex.ExportToLocal(ExcelTable, this.Page, "xls", "用药管理");
        }

        /// <summary>
        /// 时间转化格式
        /// </summary>
        /// <param name="year">yyyy</param>
        /// <param name="month">m or mm</param>
        /// <param name="date">日期选项</param>
        /// <param name="i">2:开始日1:结束日</param>
        /// <returns>yyyyMMdd</returns>
        private string DateTimeConvert(string year, string month, ComboBox cbo, int i)
        {
            string l_month = month;
            string l_date = cbo.SelectedItem.Value;
            if (Convert.ToInt32(l_month) < 10)
            {
                l_month = "0" + l_month;
            }
            else if (Convert.ToInt32(l_month) > 12)
            {
                l_month = "12";
            }
            l_date = l_date.Length == 1 ? "0" + l_date : l_date;
            return year + "-" + l_month + "-" + l_date;
        }

        /// <summary>
        /// 取得地址参数
        /// </summary>
        /// <param name="PageUrl">跳转地址</param>
        /// <returns>跳转地址以及参数</returns>
        private string getPageResponseUrl(string PageUrl, string fromDate, string toDate, string col, string deptCode,string type)
        {
            string ResponseUrl = "" + PageUrl + "?DeptCode=" + deptCode + "&FromDate=" + fromDate + "&ToDate=" + toDate + "&Type=" + type + "&Col" + col;
            return ResponseUrl;
        }

    }
}
