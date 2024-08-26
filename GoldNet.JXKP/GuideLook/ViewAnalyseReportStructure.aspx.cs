using System;
using System.Collections;
using System.Configuration;
using System.Data;
using GoldNet.Comm;
using GoldNet.Model;
using Goldnet.Ext.Web;
using Goldnet.Dal;

namespace GoldNet.JXKP.GuideLook
{
    public partial class ViewAnalyseReportStructure : System.Web.UI.Page
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
                string rpt_id = Request.QueryString["Reportid"].ToString();
                string incount = ((User)Session["CURRENTSTAFF"]).StaffId;
                if (incount == "" || incount == null) incount = "NotUserid";
                this.Store1.DataSource = dal.getAnalyseReport(incount, rpt_id);
                this.Store1.DataBind();
            }
        }
    }
}
