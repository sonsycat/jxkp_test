using System;
using System.Collections;
using System.Configuration;
using System.Data;
using Goldnet.Ext.Web;
using GoldNet.Comm.ExportData;
using Goldnet.Dal;
using System.Collections.Generic;

namespace GoldNet.JXKP.GuideLook.Statement
{
    public partial class DeptBenefit : PageBase
    {
        private StatementDal dal = new StatementDal();
        private static DataTable Currdt = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            //检查是否已经登录，否则停止
            if (Session["CURRENTSTAFF"] == null)
            {
                Response.End();
            }
            if (!Ext.IsAjaxRequest)
            {
                string deptcode = this.DeptFilter("");
                HttpProxy proxy = new HttpProxy();
                proxy.Method = HttpMethod.POST;
                proxy.Url = "/GuideLook/WebService/DeptInfo.ashx?deptfilter=" + deptcode;
                this.Store3.Proxy.Add(proxy);

                setTimeContrl();
            }
        }

        /// <summary>
        /// 刷新数据
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

            Currdt = dal.getDeptBenefit(dd1.Replace("-", ""), dd2.Replace("-", ""), this.DeptFilter(""), 
                                this.DeptCodeCombo.SelectedItem.Value, this.cboStaffInfo.SelectedItem.Value).Tables[0];
            this.Store1.DataSource = Currdt;
            this.Store1.DataBind();
            this.btnExcel.Disabled = false;
        }

        protected void OutExcel(object sender, EventArgs e)
        {
            ExportData ex = new ExportData();
            DataTable ExcelTable = new DataTable();
            ExcelTable = Currdt;
            ex.ExportToLocal(ExcelTable, this.Page, "xls", "医生收入");
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
        /// 查询按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void QueryDeptBenefitInfo(object sender, AjaxEventArgs e)
        {
            string col = e.ExtraParams["col"].ToString();
            Dictionary<string, string>[] selectRow = GetSelectRow(e);
            //判断如果selectRow里为null，则返回空
            if (Convert.ToInt32(col) < 4 || selectRow[0]["DEPT_CODE"].Equals("999999"))
            {
                return;
            }

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
            PageResponse(getPageResponseUrl("DeptBenefitInfo.aspx", dd1.Replace("-", ""), dd2.Replace("-", ""), col, selectRow[0]["DEPT_CODE"], selectRow[0]["STAFF_ID"]), selectRow[0]["NAME"] + "(" + selectRow[0]["DEPT_NAME"] + ") 收入项目");
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


        /// <summary>
        /// 跳转页面
        /// </summary>
        /// <param name="url">跳转地址以及参数</param>
        /// <param name="title">窗体名称</param>
        private void PageResponse(string url, string title)
        {

            LoadConfig loadcfg = new LoadConfig();

            loadcfg.Url = url;

            loadcfg.Mode = LoadMode.IFrame;

            loadcfg.MaskMsg = "载入中...";

            loadcfg.ShowMask = true;

            loadcfg.NoCache = true;

            this.arcEditWindow.Title = title;

            arcEditWindow.ClearContent();

            arcEditWindow.Show();

            arcEditWindow.LoadContent(loadcfg);

        }

        /// <summary>
        /// 取得地址参数
        /// </summary>
        /// <param name="PageUrl">跳转地址</param>
        /// <returns>跳转地址以及参数</returns>
        private string getPageResponseUrl(string PageUrl, string fromDate, string toDate, string type, string deptCode,string staffid)
        {
            string ResponseUrl = "" + PageUrl + "?DeptCode=" + deptCode + "&FromDate=" + fromDate + "&ToDate=" + toDate + "&Type=" + type + "&StaffId=" + staffid;
            return ResponseUrl;
        }
    }
}
