using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Collections.Generic;
using Goldnet.Ext.Web;
using Goldnet.Dal;
using GoldNet.Comm;
using GoldNet.Model;

namespace GoldNet.JXKP.GuideLook
{
    public partial class ViewSatffByMonth : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
             //检查是否已经登录，否则停止
            if (Session["CURRENTSTAFF"] == null)
            {
                Response.End();
            }

            if (!Ext.IsAjaxRequest)
            {
                ReportDalDict dal = new ReportDalDict();

                string DeptCode = Request.QueryString["DEPT_CODE"].ToString();

                string reportid = Request.QueryString["ReportCode"].ToString();

                string dd1 = Request.QueryString["FromDate"].ToString();

                string dd2 = Request.QueryString["ToDate"].ToString();

                string GuideCode = Request.QueryString["GuideCode"].ToString();

                string incount = ((User)Session["CURRENTSTAFF"]).UserId;

                if (incount == "") incount = "NotUserid";

                DataTable ReportInfo = dal.getStaffByMonthReport(dd1, dd2, GuideCode, DeptCode, "R", reportid).Tables[0];

                SortedList<string, string> list = CreateYearsTable(dd1, dd2);

                GuideDalDict GuideDal = new GuideDalDict();

                DataTable l_dtName = GuideDal.GuideName().Tables[0];

                DataTable l_dt = ConvertReportTable(ReportInfo,list);

                CreateReportPanel(l_dt,l_dtName);
                
            }
        }

        /// <summary>
        /// 自定义按月查询报表
        /// </summary>
        /// <param name="ReportInfo">报表信息</param>
        /// <param name="list">月份名称</param>
        private void CreateReportPanel(DataTable ReportInfo, DataTable ReportColName)
        {

            this.Store1.RemoveFields();

            this.GridPanel_Show.Reconfigure();

            for (int i = 0; i < ReportInfo.Columns.Count; i++)
            {
                RecordField field = new RecordField(ReportInfo.Columns[i].ColumnName, RecordFieldType.String);

                this.Store1.AddField(field, i);

                Column col = new Column();

                if (ReportInfo.Columns[i].ColumnName.Equals("UNIT_CODE"))
                {
                    col.Hidden = true;
                    col.Width = col.Header.Length * 50;
                    col.Sortable = true;
                    col.DataIndex = ReportInfo.Columns[i].ColumnName;
                    col.Align = Alignment.Right;
                    col.MenuDisabled = true;
                }
                if (ReportInfo.Columns[i].ColumnName.Equals("TJYF"))
                {
                    col.Header = "日期";
                    col.Width = col.Header.Length * 50;
                    col.Sortable = true;
                    col.DataIndex = ReportInfo.Columns[i].ColumnName;
                    col.Align = Alignment.Right;
                    col.MenuDisabled = true;
                }
                if (ReportInfo.Columns[i].ColumnName.Substring(0, 1).Equals("A"))
                {
                    DataRow[] l_dr = ReportColName.Select("GUIDE_CODE = " + ReportInfo.Columns[i].ColumnName.Substring(1, ReportInfo.Columns[i].ColumnName.Length - 1));
                    if(l_dr.Length > 0) 
                    {
                        col.Header = l_dr[0]["GUIDE_NAME"].ToString().Replace("（人）", "").Replace("(人)", "").Replace("（人)", "").Replace("(人）", "");
                    }
                    col.Width = col.Header.Length * 25;
                    col.Sortable = true;
                    col.DataIndex = ReportInfo.Columns[i].ColumnName;
                    col.Align = Alignment.Right;
                    col.MenuDisabled = true;
                }
                this.GridPanel_Show.AddColumn(col);
            }
            this.Store1.DataSource = ReportInfo;
            this.Store1.DataBind();
        }

        /// <summary>
        /// 转化DataTable
        /// </summary>
        /// <param name="dt">集合</param>
        /// <param name="list">时间集合</param>
        /// <returns>集合</returns>
        private DataTable ConvertReportTable(DataTable dt, SortedList<string, string> list) 
        {
            int i = 0;
            //item.Key:190002,item.Value:1900年2月
            foreach (KeyValuePair<string, string> item in list)
            {   
                DataRow[] l_dr = dt.Select("TJYF='" + item.Key+"'");
                if (l_dr.Length == 0)
                {
                    dt.Rows.InsertAt(InsertEmyptRowsInTable(item.Value, dt), i);
                }
                else 
                {
                    dt.Rows[i][1] = item.Value;
                }
                i++;
            }
            return dt;
        }

        /// <summary>
        /// 添加空行
        /// </summary>
        /// <param name="time">日期</param>
        /// <param name="l_dt">集合</param>
        /// <returns></returns>
        private DataRow InsertEmyptRowsInTable(string time,DataTable l_dt) 
        {
            DataRow l_dr = l_dt.NewRow();
            for (int i = 0; i < l_dt.Columns.Count;i++ )
            {
                if (i == 1)
                {
                    l_dr[i] = time;
                }
                else 
                {
                    l_dr[i] = "0";
                }
            }
            return l_dr;
        }




        /// <summary>
        /// 虚拟月份
        /// </summary>
        /// <param name="dd1">开始时间</param>
        /// <param name="dd2">结束时间</param>
        /// <returns>时间集合</returns>
        private SortedList<string, string> CreateYearsTable(string dd1, string dd2)
        {
            DateTime dt1 = Convert.ToDateTime(ConvertStringToTime(dd1, "-"));
            DateTime dt2 = Convert.ToDateTime(ConvertStringToTime(dd2, "-"));
            //计算开始到结束差值
            int Month = (dt2.Year - dt1.Year) * 12 + (dt2.Month + 1) - dt1.Month;

            SortedList<string, string> TimeList = new SortedList<string, string>();

            for (int i = 0; i < Month; i++)
            {
                TimeList.Add(dt1.AddMonths(i).ToString("yyyyMM"), "" + dt1.AddMonths(i).Year + "年" + dt1.AddMonths(i).Month + "月");
            }

            return TimeList;
        }

        /// <summary>
        /// 时间转化
        /// </summary>
        /// <param name="time">字符串</param>
        /// <param name="split">时间分割符</param>
        /// <returns></returns>
        private string ConvertStringToTime(string time, string split)
        {
            string y = time.Substring(0, 4);

            string m = time.Substring(4, 2);

            string d = time.Substring(6);

            time = y + split + m + split + d;

            return time;
        }
    }
}
