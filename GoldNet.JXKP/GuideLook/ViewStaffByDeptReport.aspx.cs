using System;
using System.Collections;
using System.Configuration;
using System.Data;
using Goldnet.Ext.Web;
using System.Text;
using GoldNet.Comm;
using GoldNet.Model;
using Goldnet.Dal;

namespace GoldNet.JXKP.GuideLook
{
    public partial class ViewStaffByDeptReport : System.Web.UI.Page
    {
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

                GuideDalDict dal = new GuideDalDict();

                ReportDalDict rptDal = new ReportDalDict();

                string Reportid = Request.QueryString["ReportCode"].ToString();

                string DeptCode = Request.QueryString["DEPT_CODE"].ToString();

                string dd1 = Request.QueryString["FromDate"].ToString();

                string dd2 = Request.QueryString["ToDate"].ToString();

                string incount = ((User)Session["CURRENTSTAFF"]).StaffId;

                if (incount == "" || incount == null) incount = "NotUserid";

                DataTable l_dt = rptDal.getReportDept(incount, Reportid, DeptCode).Tables[0];

                DataTable l_dtName = dal.GuideName().Tables[0];

                CreateReportPanel(l_dt, l_dtName);

            }

        }

        /// <summary>
        /// 自定义报表
        /// </summary>
        /// <param name="ReportInfo">报表信息</param>
        /// <param name="ReportColName">指标名称</param>
        private void CreateReportPanel(DataTable ReportInfo, DataTable ReportColName)
        {

            this.Store1.RemoveFields();

            this.GridPanel1.Reconfigure();

            for (int i = 0; i < ReportInfo.Columns.Count; i++)
            {
                RecordField field = new RecordField(ReportInfo.Columns[i].ColumnName, RecordFieldType.String);

                this.Store1.AddField(field, i);

                Column col = new Column();

                if (ReportInfo.Columns[i].ColumnName.Equals("STAFF_ID"))
                {
                    col.Hidden = true;
                    col.MenuDisabled = true;
                }
                if (ReportInfo.Columns[i].ColumnName.Equals("NAME"))
                {
                    col.Header = "人员名称";
                    col.Width = col.Header.Length * 50;
                    col.Sortable = true;
                    col.DataIndex = ReportInfo.Columns[i].ColumnName;
                    col.Align = Alignment.Right;
                    col.MenuDisabled = true;

                }
                if (ReportInfo.Columns[i].ColumnName.Substring(0, 1).Equals("A"))
                {
                    string guideCode = ReportInfo.Columns[i].ColumnName.Substring(1, ReportInfo.Columns[i].ColumnName.Length - 1);

                    if (guideCode != "") 
                    {
                        DataRow[] l_dr = ReportColName.Select("GUIDE_CODE = " + guideCode);
                        if (l_dr.Length > 0) 
                        {
                            col.Header = l_dr[0]["GUIDE_NAME"].ToString().Replace("（人）", "").Replace("(人)", "").Replace("（人)", "").Replace("(人）", "");
                        }
                    }
                    col.MenuDisabled = true;
                    col.Width = col.Header.Length * 15;
                    col.Sortable = true;
                    col.DataIndex = ReportInfo.Columns[i].ColumnName;
                    col.Align = Alignment.Right;

                }

                this.GridPanel1.AddColumn(col);

            }

            this.Store1.DataSource = ReportInfo;
            this.Store1.DataBind();

        }
    }
}
