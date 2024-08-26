using System;
using System.Collections;
using System.Configuration;
using System.Data;
using Goldnet.Ext.Web;
using Goldnet.Dal;

namespace GoldNet.JXKP.GuideLook
{
    public partial class ReportDetailGuide : System.Web.UI.Page
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

                string DeptCode = Request.QueryString["DEPT_CODE"].ToString();

                string dd1 = Request.QueryString["FromDate"].ToString();

                string dd2 = Request.QueryString["ToDate"].ToString();

                string GuideCode = Request.QueryString["GuideCode"].ToString();

                GuideDalDict dal = new GuideDalDict();

                DataTable l_dt = dal.getGuideExpressions(GuideCode).Tables[0];

                if (l_dt.Rows.Count > 0) 
                {
                    if (l_dt.Rows[0]["GUIDE_SQL_DETAIL"] == null) 
                    {
                        return;
                    }

                    string DetailSql = l_dt.Rows[0]["GUIDE_SQL_DETAIL"].ToString();

                    if (!DetailSql.Equals("")) 
                    {
                        DataTable l_detail = new DataTable();

                        l_detail = dal.getGuideExpressionsDetail(DetailSql, GuideCode, DeptCode, dd1, dd2).Tables[0];

                        CreateReportPanel(l_detail);
                    }
                }          
            }
        }

        /// <summary>
        /// 自定义明细化报表
        /// </summary>
        /// <param name="ReportInfo">报表信息</param>
        private void CreateReportPanel(DataTable ReportInfo)
        {

            this.Store1.RemoveFields();

            this.GridPanel1.Reconfigure();


            for (int i = 0; i < ReportInfo.Columns.Count; i++)
            {
                RecordField field = new RecordField(ReportInfo.Columns[i].ColumnName, RecordFieldType.String);

                this.Store1.AddField(field, i);

                Column col = new Column();

                col.Header = ReportInfo.Columns[i].ColumnName;
                col.Width = col.Header.Length * 20;
                col.Sortable = true;
                col.DataIndex = ReportInfo.Columns[i].ColumnName;
                col.Align = Alignment.Right;

                this.GridPanel1.AddColumn(col);

            }
            this.Store1.DataSource = ReportInfo;
            this.Store1.DataBind();
        }        
    }
}
